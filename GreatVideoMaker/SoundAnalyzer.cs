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

        private string filepath;
        private int framerate;

        private Frame[] frames;
        private float[] distances;
        private int samplerate;
        private int channels;
        private int count;
        private TimeSpan totalLength;
        private double frameLength;
        private float minFreq;
        private float maxFreq;

        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler OnComplete;

        public Frame[] Frames { get { return frames; } }
        public int SampleRate { get { return samplerate; } }
        public TimeSpan TotalLength { get { return totalLength; } }
        public string SourceFilePath { get { return filepath; } }
        public int FrameRate { get { return framerate; } }
        public float MinimumFrequency { get { return minFreq; } }
        public float MaximumFrequency { get { return maxFreq; } }

        public SoundAnalyzer(string audiopath, int framerate)
        {
            this.filepath = audiopath;
            this.framerate = framerate;
        }

        public void StartProcess()
        {
            Task task = new Task(() =>
            {
                minFreq = int.MaxValue;
                maxFreq = int.MinValue;

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

                    distances = new float[bufferLength];
                    distances[0] = GetNoteDistance(1); //hackfix cause cant calculate distance to 0 frequencies
                    for (int i = 1; i < bufferLength; i++)
                    {
                        distances[i] = GetNoteDistance(i);
                    }
                    double distanceScale = (bufferLength - 1) / (distances[distances.Length - 1] - distances[0]);
                    double distanceOffset = -distances[0];

                    for (int i = 0; i < takes; i++)
                    {
                        float[] buffer = new float[bufferLength];

                        //read additional 2 floats for fft then scroll back a bit to not disturb reading
                        provider.Read(buffer, 0, countp2);
                        reader.Seek(-2 * reader.BlockAlign, SeekOrigin.Current);

                        Fourier.ForwardReal(buffer, count);

                        float[] notes = new float[bufferLength];

                        for (int k = 0; k < buffer.Length; k++)
                        {
                            buffer[k] = Math.Abs(buffer[k]);
                            if (buffer[k] > maxFreq) maxFreq = buffer[k];
                            if (buffer[k] < minFreq) minFreq = buffer[k];

                            int index = (int)(distanceScale * (distances[k] + distanceOffset));
                            notes[index] = buffer[k];
                        }
                        frames[i].frequencies = buffer;
                        frames[i].notes = notes;

                        /*for (int k = 0; k < buffer.Length; k++) //calculate notes in the future
                        {
                            frames[i].notes
                        }*/

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

        public struct Frame
        {
            public float[] frequencies;
            public float[] notes;
        }
    }
}
