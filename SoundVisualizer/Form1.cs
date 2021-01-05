using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using NAudio.Wave;
using ColorMine.ColorSpaces;
using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FFMpegCore;
using FFMpegCore.Pipes;
using FFMpegCore.Extend;

namespace SoundVisualizer
{    
    public partial class Form1 : Form
    {
        private int samplerate = 0;
        private int channels = 0;
        private int count = 0;
        private double seconds = 0;
        private double length = 0;

        public Form1()
        {
            InitializeComponent();

            //copypaste from stackoverflow A
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
                //DoThing();
                DoBetterThing();
            }
        }
                
        private void DoBetterThing()
        {
            List<float[]> buffers = new List<float[]>();

            using (WaveFileReader reader = new WaveFileReader(label1.Text))
            {
                ISampleProvider provider = reader.ToSampleProvider();
                samplerate = provider.WaveFormat.SampleRate;
                channels = provider.WaveFormat.Channels;
                length = reader.TotalTime.TotalSeconds;

                seconds = 1 / (double)numericUpDown1.Value;
                int takes = (int)(reader.TotalTime.TotalSeconds / seconds);
                int count = this.count = (int)(samplerate * seconds * channels);
                int countp2 = count + 2; // bad variable name ik
                int buffercount = Math.Max(samplerate, count) + 2;

                for (int i = 0; i < takes; i++)
                {
                    float[] buffer = new float[buffercount];

                    //read additional 2 floats then scroll back a bit
                    provider.Read(buffer, 0, countp2);                    
                    reader.Seek(-2 * reader.BlockAlign, SeekOrigin.Current);

                    Fourier.ForwardReal(buffer, count);
                    buffers.Add(buffer);
                }
            }

            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(buffers.ToArray());
            listBox1.EndUpdate();
        }

        private void SetGraph(float[] numbers)
        {            
            chart1.Series.Clear();
            chart1.Series.Add(numbers.GetHashCode().ToString());
            Series series = chart1.Series.FindByName(numbers.GetHashCode().ToString());

            //series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline; // to get connected line            

            int count = Math.Min(numbers.Length, (int)numericUpDown2.Value);            
            for (int i = 0; i < count; i++)
            {
                //math.max is to prevent 0. sideeffect is 0 and 1 have same distance but who cares                
                double distance = GetNoteDistance(440, Math.Max(i, 1));

                double number = 0;
                if (seconds > 1)
                {
                    for (int k = 0; k < (int)seconds; k++)
                    {
                        number += Math.Abs(numbers[(int)(i * seconds + k)]);
                    }
                    number = number / seconds;
                }
                else
                {
                    number = Math.Abs(numbers[(int)(i * seconds)]);
                }
                
                //AddXy(i) shows frequency. AddXy(distance) shows notes. higher notes have more data points cause how it works lol
                int index = series.Points.AddXY(distance, number);
                DataPoint point = series.Points.ElementAt(index);

                Color c = Color.FromArgb(0);
                
                double thingy = distance % 12 / 12;
                if (thingy < 0) thingy = 1 + thingy;
                Hsv hsv = new Hsv();
                hsv.S = 1;
                hsv.V = 1;
                hsv.H = 360 * thingy;
                IRgb rgb = hsv.ToRgb();
                point.Color = Color.FromArgb((int)rgb.R, (int)rgb.G, (int)rgb.B);

                //series.Points.ElementAt(index).Color = Color.FromArgb(i % 256, 255 - i % 256, i * i % 256);
                //series.Points.AddXY(i, men[i]); // this is pure wav file
            }
        }

        private byte[] SpectrumToAudioFrames(float[] data)
        {
            int amount = Math.Min(count + 2, data.Length);
            float[] clone = new float[amount];
            Buffer.BlockCopy(data, 0, clone, 0, amount);
            Fourier.InverseReal(clone, clone.Length - 2);
            byte[] frames = clone.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
            return frames;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            float[] data = listBox1.SelectedItem as float[];
            SetGraph(data);

            WaveFormat format = WaveFormat.CreateIeeeFloatWaveFormat(samplerate, channels);                        
            byte[] frames = SpectrumToAudioFrames(data);
            IWaveProvider provider = new RawSourceWaveStream(new MemoryStream(frames), format);
            WaveOut wo = new WaveOut();
            wo.Init(provider);
            wo.Play();
        }

        private double GetNoteDistance(int baseFrequency, int comparedFrequency)
        {
            double result;

            double divided = comparedFrequency / (double)baseFrequency;
            double logged = Math.Log(divided, 2);
            result = logged * 12;

            return result;
        }
        IEnumerable<IVideoFrame> CreateFrames(Bitmap[] bitmaps)
        {
            for (int i = 0; i < bitmaps.Length; i++)
            {
                BitmapVideoFrameWrapper wrapper = new BitmapVideoFrameWrapper(bitmaps[i]);
                yield return wrapper;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Task task = new Task(() =>
                {
                    int width = 1280;
                    int height = 720;
                    int framerate = (int)numericUpDown1.Value;
                    string filepath = saveFileDialog1.FileName;
                    string audiopath = label1.Text;

                    Bitmap[] bitmaps = new Bitmap[listBox1.Items.Count];
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        chart1.Invoke((MethodInvoker)delegate { SetGraph((float[])listBox1.Items[i]); });
                        Bitmap image = new Bitmap(width, height);
                        chart1.Invoke((MethodInvoker)delegate { DrawToBitmap(image, new Rectangle(0, 0, width, height)); });
                        bitmaps[i] = image;
                    }

                    var videoFramesSource = new RawVideoPipeSource(CreateFrames(bitmaps)) { FrameRate = framerate };
                    FFMpegArguments
                        .FromPipeInput(videoFramesSource)
                        //.AddPipeInput(new StreamPipeSource(...)
                        .AddFileInput(new FileInfo(audiopath))
                        .OutputToFile(filepath)
                        .NotifyOnProgress(new Action<TimeSpan>((TimeSpan t) => { label4.Invoke((MethodInvoker)delegate { label4.Text = t.ToString(); }); }))
                        .ProcessSynchronously();
                });
                task.Start();
            }
        }
    }
}
