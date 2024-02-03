using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MathNet.Numerics.IntegralTransforms;
using NAudio.Wave;

namespace Tools
{
    public class SoundAnalyzer : Processer
    {
        public static IEnumerable<string> GetWindows()
        {
            MethodInfo[] methodInfos = typeof(MathNet.Numerics.Window).GetMethods();

            for (int i = 0; i < methodInfos.Length; i++)
            {
                if (methodInfos[i].ReturnType == typeof(double[]))
                {
                    yield return methodInfos[i].Name;
                }                    
            }
        }

        private string windowName;
        private bool disposedValue;

        public event EventHandler<ProgressEventArgs>? OnProgress;
        public event EventHandler? OnComplete;

        public Frame[]? Frames { get; private set; }
        public int SampleRate { get; private set; }
        public TimeSpan TotalLength { get; private set; }
        public string SourceFilePath { get; private set; }
        public int FrameRate { get; private set; }
        public float MinimumAmplitude { get; private set; }
        public float MaximumAmplitude { get; private set; }
        public float FrequencyFidelity { get; private set; } //how many frequencies share the same position
        public float NotesTotalLength { get; private set; }
        public float MinimumNote { get; private set; }
        public float MaximumNote { get; private set; }
        public NoteSpan[]? NoteSpans { get; private set; } // how many notes does each frequency cover
        public int LookAround { get; private set; } //take future and past frames into consideration (get more fidelity)
        public int FrequencyCount { get; private set; }
        
        public SoundAnalyzer(string audiopath, int framerate, int lookaround = 0, string windowName = "HannPeriodic")
        {
            SourceFilePath = audiopath;
            FrameRate = framerate;
            LookAround = lookaround;
            this.windowName = windowName;
        }

        public void StartProcess()
        {
            float[] distances;
            int channels;
            double frameLength;

            MinimumAmplitude = int.MaxValue;
            MaximumAmplitude = int.MinValue;

            int lookFactor = 1 + LookAround * 2;
            FrequencyFidelity = FrameRate / (float)lookFactor; //when considering nearby frames, more data can be made
            int takes;
            int precount;
            int count;
            int countp2;
            int bufferLength;

            float[][] buffers;

            using (AudioFileReader reader = new AudioFileReader(SourceFilePath))
            {
                // lots of setup
                ISampleProvider provider = reader.ToSampleProvider();
                SampleRate = provider.WaveFormat.SampleRate;
                channels = provider.WaveFormat.Channels;
                TotalLength = reader.TotalTime;

                frameLength = 1 / (double)FrameRate;
                takes = (int)(reader.TotalTime.TotalSeconds * FrameRate);
                precount = (int)(SampleRate * frameLength * channels);
                count = FrequencyCount = precount * lookFactor;
                countp2 = count + 2; // bad variable name ik
                bufferLength = Math.Max(SampleRate, count) + 2;

                buffers = new float[takes][];

                // generating buffers for first x frames which cant look fully into the future
                // im assuming that lookAround is smaller than takes, which it always should be....
                for (int i = 0; i < LookAround; i++)
                {
                    float[] buffer = new float[bufferLength];
                    int earlyLookFactor = 1 + LookAround + i; // simulates "reading" bytes from negative positions
                    int earlycount = precount * earlyLookFactor + 2; // same thing but the classic +2 is here

                    int offset = precount * (LookAround - i);
                    provider.Read(buffer, offset, earlycount);
                    reader.Seek(0, SeekOrigin.Begin); // reset reader back to 0 cause were simulating real reading

                    buffers[i] = buffer;
                }
                // the real frame reading. note we dont need after-processing although we needed pre-processing cause
                // the stream can handle giving out less bytes in the end
                int seekMultiplier = reader.BlockAlign / channels;
                for (int i = LookAround; i < takes; i++)
                {
                    float[] buffer = new float[bufferLength];

                    //read additional 2 floats + lookaround for fft then scroll back a bit to not desync time
                    provider.Read(buffer, 0, countp2);
                    int backwardsCount = -(2 + precount * LookAround * 2);
                    reader.Seek(backwardsCount * seekMultiplier, SeekOrigin.Current);

                    buffers[i] = buffer;
                }
            }

            // all note distance stuff
            distances = new float[count + 1];
            distances[0] = NoteHelper.GetNoteDistance(1); //hackfix cause cant calculate distance to 0 frequencies
            for (int i = 1; i < distances.Length; i++)
            {
                distances[i] = NoteHelper.GetNoteDistance((int)(i * FrequencyFidelity));
            }
            MaximumNote = distances[distances.Length - 1];
            MinimumNote = distances[0];
            NotesTotalLength = MaximumNote - MinimumNote;
            float distanceScale = (bufferLength - 1) / NotesTotalLength;
            float distanceOffset = -MinimumNote;
            // calculating note spans
            NoteSpans = new NoteSpan[count];
            for (int i = 0; i < count; i++)
            {
                NoteSpans[i].start = distances[i];
                NoteSpans[i].end = distances[i + 1];
                NoteSpans[i].length = NoteSpans[i].end - NoteSpans[i].start;
            }

            Frames = new Frame[takes];
            List<BackgroundWorker> bgws = new List<BackgroundWorker>();
            object bufferLock = new object(); // used for threads to know what to take
            int bufferIndex = 0;

            MethodInfo method = typeof(MathNet.Numerics.Window).GetMethod(windowName);
            // really am doing this: MathNet.Numerics.Window.HannPeriodic(count);
            double[] window = method.Invoke(this, new object[] { count }) as double[];

            for (int i = 0; i < RenderInfo.ProcessorCount; i++)
            {
                BackgroundWorker k = new BackgroundWorker();
                bgws.Add(k);
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
                BackgroundWorker bgw = sender as BackgroundWorker;

                lock (bgws)
                {
                    bgws.Remove(bgw);
                    bgw.Dispose();
                    if (bgws.Count == 0)
                    {
                        DeclareComplete();
                    }
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

                        for (int k = 0; k < count; k++)
                        {
                            buffers[index][k] = (float)(buffers[index][k] * window[k]);
                        }

                        Fourier.ForwardReal(buffers[index], count, FourierOptions.NoScaling);

                        //buffer processing for next things
                        for (int k = 0; k < count; k++)
                        {
                            float abs = Math.Abs(buffers[index][k]) / count;
                            if (abs > MaximumAmplitude) MaximumAmplitude = abs;
                            if (abs < MinimumAmplitude) MinimumAmplitude = abs;

                            frequencies[k] = abs; //frequencies are just the necessary bytes from the buffer (for now)
                        }

                        // honestly dont need to give buffers
                        //frames[frameIndex].buffer = buffer;
                        Frames[index].frequencies = frequencies;

                        OnProgress?.Invoke(this, new ProgressEventArgs(index, takes));
                    }
                    else break;
                }
            }
        }

        private delegate void CalculateFourier();

        private void DeclareComplete()
        {
            OnComplete?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SoundAnalyzer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
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
