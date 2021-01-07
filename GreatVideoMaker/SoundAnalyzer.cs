using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;
using NAudio.Wave;

namespace GreatVideoMaker
{
    class SoundAnalyzer : Processer
    {
        private static double baseFrequency = 440;
        private static int threadCount = 4;

        private string filepath;
        private int framerate;

        private Frame[] frames;
        private float[] distances;
        private NoteSpan[] noteSpans; // how many notes does each frequency cover
        private float notesTotalLength;
        private float minNote;
        private float maxNote;
        private int samplerate;
        private int channels;
        private int count;
        private TimeSpan totalLength;
        private double frameLength;
        private float minAmplitude;
        private float maxAmplitude;
        private float frequencyFidelity; //how many frequencies share the same position
        private int lookAround; //FUTURE. take future frames into consideration (get more fidelity)

        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler OnComplete;

        public Frame[] Frames { get { return frames; } }
        public int SampleRate { get { return samplerate; } }
        public TimeSpan TotalLength { get { return totalLength; } }
        public string SourceFilePath { get { return filepath; } }
        public int FrameRate { get { return framerate; } }
        public float MinimumAmplitude { get { return minAmplitude; } }
        public float MaximumAmplitude { get { return maxAmplitude; } }
        public float FrequencyFidelity { get { return frequencyFidelity; } }
        public float NotesTotalLength { get { return notesTotalLength; } }
        public float MinimumNote { get { return minNote; } }
        public float MaximumNote { get { return maxNote; } }
        public NoteSpan[] NoteSpans { get { return noteSpans; } }
        public int LookAround { get { return lookAround; } }

        public SoundAnalyzer(string audiopath, int framerate, int lookaround = 0)
        {
            this.filepath = audiopath;
            this.framerate = framerate;
            this.lookAround = lookaround;
        }

        public void StartProcess()
        {
            minAmplitude = int.MaxValue;
            maxAmplitude = int.MinValue;

            int lookFactor = 1 + lookAround * 2;
            frequencyFidelity = framerate / (float)lookFactor; //when considering nearby frames, more data can be made
            int takes;
            int precount;
            int count;
            int countp2;
            int bufferLength;

            float[][] buffers;

            using (WaveFileReader reader = new WaveFileReader(filepath))
            {
                // lots of setup
                ISampleProvider provider = reader.ToSampleProvider();
                samplerate = provider.WaveFormat.SampleRate;
                channels = provider.WaveFormat.Channels;
                totalLength = reader.TotalTime;

                frameLength = 1 / (double)framerate;
                takes = (int)(reader.TotalTime.TotalSeconds * framerate);
                precount = (int)(samplerate * frameLength * channels);
                count = this.count = precount * lookFactor;
                countp2 = count + 2; // bad variable name ik
                bufferLength = Math.Max(samplerate, count) + 2;

                buffers = new float[takes][];

                // generating buffers for first x frames which cant look fully into the future
                // im assuming that lookAround is smaller than takes, which it always should be....
                for (int i = 0; i < lookAround; i++)
                {
                    float[] buffer = new float[bufferLength];
                    int earlyLookFactor = 1 + lookAround + i; // simulates "reading" bytes from negative positions
                    int earlycount = precount * earlyLookFactor + 2; // same thing but the classic +2 is here

                    int offset = precount * (lookAround - i);
                    provider.Read(buffer, offset, earlycount);
                    reader.Seek(0, SeekOrigin.Begin); // reset reader back to 0 cause were simulating real reading

                    buffers[i] = buffer;
                }
                // the real frame reading. note we dont need after-processing although we needed pre-processing cause
                // the stream can handle giving out less bytes in the end
                for (int i = lookAround; i < takes; i++)
                {
                    float[] buffer = new float[bufferLength];

                    //read additional 2 floats + lookaround for fft then scroll back a bit to not desync time
                    provider.Read(buffer, 0, countp2);
                    reader.Seek(-(2 + precount * lookAround * 2) * reader.BlockAlign, SeekOrigin.Current);

                    buffers[i] = buffer;
                }
            }

            // all note distance stuff
            distances = new float[count + 1];
            distances[0] = GetNoteDistance(1); //hackfix cause cant calculate distance to 0 frequencies
            for (int i = 1; i < distances.Length; i++)
            {
                distances[i] = GetNoteDistance((int)(i * frequencyFidelity));
            }
            maxNote = distances[distances.Length - 1];
            minNote = distances[0];
            notesTotalLength = maxNote - minNote;
            float distanceScale = (bufferLength - 1) / notesTotalLength;
            float distanceOffset = -minNote;
            // calculating note spans
            noteSpans = new NoteSpan[count];
            for (int i = 0; i < count; i++)
            {
                noteSpans[i].start = distances[i];
                noteSpans[i].end = distances[i + 1];
                noteSpans[i].length = noteSpans[i].end - noteSpans[i].start;
            }

            frames = new Frame[takes];
            List<BackgroundWorker> bgws = new List<BackgroundWorker>();
            object bufferLock = new object(); // used for threads to know what to take
            int bufferIndex = 0;

            for (int i = 0; i < threadCount; i++)
            {
                BackgroundWorker k = new BackgroundWorker();
                k.DoWork += K_DoWork;
                k.RunWorkerCompleted += K_RunWorkerCompleted;
                k.RunWorkerAsync();
            }

            void K_DoWork(object sender, DoWorkEventArgs e)
            {
                ProcessBuffer();
            }

            void K_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                bgws.Remove((BackgroundWorker)sender);
                if (bgws.Count == 0)
                {
                    DeclareComplete();
                }
            }

            // this function could be run without any threads at all but then whats the point
            void ProcessBuffer()
            {
                while (true)
                {
                    bool dostuff = false;
                    int index = -1;

                    lock (bufferLock)
                    {
                        if (bufferIndex < takes)
                        {
                            index = bufferIndex;
                            bufferIndex++;
                            dostuff = true;
                        }
                    }

                    if (dostuff)
                    {
                        float[] frequencies = new float[count];

                        Fourier.ForwardReal(buffers[index], count);

                        //buffer processing for next things
                        for (int k = 0; k < count; k++)
                        {
                            float abs = Math.Abs(buffers[index][k]);
                            if (abs > maxAmplitude) maxAmplitude = abs;
                            if (abs < minAmplitude) minAmplitude = abs;

                            frequencies[k] = abs; //frequencies are just the necessary bytes from the buffer (for now)
                        }

                        // honestly dont need to give buffers
                        //frames[frameIndex].buffer = buffer;
                        frames[index].frequencies = frequencies;

                        if (OnProgress != null) OnProgress.Invoke(this, new ProgressEventArgs(index, takes));
                    }
                    else break;
                }
            }
        }

        private delegate void CalculateFourier();

        private void DeclareComplete()
        {
            if (OnComplete != null) OnComplete.Invoke(this, EventArgs.Empty);
        }

        private float GetNoteDistance(int comparedFrequency)
        {
            float result;

            double divided = comparedFrequency / baseFrequency;
            double logged = Math.Log(divided, 2);
            result = (float)(logged * 12);

            return result;
        }

        private int GetNoteFrequency(float noteDistance)
        {
            int result;

            double divided = noteDistance / 12f;
            double exponented = Math.Pow(divided, 2);
            result = (int)(exponented * baseFrequency);

            return result;
        }

        public struct Frame
        {
            //public float[] buffer;
            public float[] frequencies; //basically done did a logarithmic
        }

        public struct NoteSpan
        {
            public float start;
            public float end;
            public float length;
        }
    }
}
