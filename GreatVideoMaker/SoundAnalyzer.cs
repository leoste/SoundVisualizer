using System;
using System.Collections.Generic;
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
        private static int lookAround = 0; //FUTURE. +past, take surrounding frames into consideration for generating a smoother visual. 0 = only current frame.

        private string filepath;
        private int framerate;

        private Frame[] frames;
        private float[] distances;
        private NoteSpan[] noteLengths; // how many notes does each frequency cover
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
        public NoteSpan[] NoteSpans { get { return noteLengths; } }

        public SoundAnalyzer(string audiopath, int framerate)
        {
            this.filepath = audiopath;
            this.framerate = framerate;
        }

        public void StartProcess()
        {
            Task task = new Task(() =>
            {
                minAmplitude = int.MaxValue;
                maxAmplitude = int.MinValue;

                int lookFactor = 1 + lookAround * 2;
                frequencyFidelity = framerate / (float)lookFactor; //when considering nearby frames, more data can be made

                using (WaveFileReader reader = new WaveFileReader(filepath))
                {
                    ISampleProvider provider = reader.ToSampleProvider();
                    samplerate = provider.WaveFormat.SampleRate;
                    channels = provider.WaveFormat.Channels;
                    totalLength = reader.TotalTime;

                    frameLength = 1 / (double)framerate;
                    int takes = (int)(reader.TotalTime.TotalSeconds * framerate);
                    int count = this.count = (int)(samplerate * frameLength * channels);
                    int countp2 = count + 2; // bad variable name ik
                    int bufferLength = Math.Max(samplerate, count) + 2;

                    frames = new Frame[takes];

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

                    noteLengths = new NoteSpan[count];
                    for (int i = 0; i < count; i++)
                    {
                        noteLengths[i].start = distances[i];
                        noteLengths[i].end = distances[i + 1];
                        noteLengths[i].length = noteLengths[i].end - noteLengths[i].start;
                    }

                    for (int i = 0; i < takes; i++)
                    {
                        float[] buffer = new float[bufferLength];
                        float[] frequencies = new float[count];                        

                        //read additional 2 floats for fft then scroll back a bit to not disturb reading
                        provider.Read(buffer, 0, countp2);
                        reader.Seek(-2 * reader.BlockAlign, SeekOrigin.Current);

                        Fourier.ForwardReal(buffer, count);

                        //buffer processing for next things
                        for (int k = 0; k < count; k++)
                        {
                            float abs = Math.Abs(buffer[k]);
                            if (abs > maxAmplitude) maxAmplitude = abs;
                            if (abs < minAmplitude) minAmplitude = abs;

                            frequencies[k] = abs; //frequencies are just the necessary bytes from the buffer (for now)
                        }
                        
                        /*for (int k = 0; k < count; k++)
                        {

                        }*/

                        /*for (int k = 0; k < buffer.Length; k++)
                        {
                            buffer[k] = Math.Abs(buffer[k]);
                            if (buffer[k] > maxFreq) maxFreq = buffer[k];
                            if (buffer[k] < minFreq) minFreq = buffer[k];

                            int index = (int)(distanceScale * (distances[k] + distanceOffset));
                            notes[index] = buffer[k];
                        }
                        frames[i].frequencies = buffer;
                        frames[i].notes = notes;*/

                        frames[i].buffer = buffer;
                        frames[i].frequencies = frequencies;

                        if (OnProgress != null) OnProgress.Invoke(this, new ProgressEventArgs(i, takes));
                    }
                }

                if (OnComplete != null) OnComplete.Invoke(this, EventArgs.Empty);
            });
            task.Start();
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
            public float[] buffer;
            public float[] frequencies;
        }

        public struct NoteSpan
        {
            public float start;
            public float end;
            public float length;
        }
    }
}
