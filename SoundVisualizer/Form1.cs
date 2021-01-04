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

namespace SoundVisualizer
{    
    public partial class Form1 : Form
    {
        private int samplerate = 0;
        private int channels = 0;

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

                int seconds = (int)numericUpDown1.Value;
                int takes = (int)(reader.TotalTime.TotalSeconds / seconds);
                int count = samplerate * seconds * channels;
                int countp2 = count + 2; // bad variable name ik

                for (int i = 0; i < takes; i++)
                {
                    float[] buffer = new float[countp2];

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
            System.Windows.Forms.DataVisualization.Charting.Series series = chart1.Series.FindByName(numbers.GetHashCode().ToString());

            //series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline; // to get connected line            

            int count = Math.Min(numbers.Length, 4000);
            for (int i = 0; i < count; i++)
            {
                int index = series.Points.AddXY(i, Math.Abs(numbers[i]));
                DataPoint point = series.Points.ElementAt(index);

                double distance = 0;
                if (i > 0) distance = GetNoteDistance(440, i);
                double thingy = distance % 12 / 12;
                if (thingy < 0) thingy = 1 + thingy;                
                int value = (int)(256 * thingy);
                point.Color = Color.FromArgb(value, 0, 0);

                //series.Points.ElementAt(index).Color = Color.FromArgb(i % 256, 255 - i % 256, i * i % 256);
                //series.Points.AddXY(i, men[i]); // this is pure wav file
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            float[] data = listBox1.SelectedItem as float[];
            SetGraph(data);

            WaveFormat format = WaveFormat.CreateIeeeFloatWaveFormat(samplerate, channels);

            float[] clone = (data.Clone() as float[]).ToList().Append(0).Append(0).ToArray();
            Fourier.InverseReal(clone, data.Length);
            byte[] frames = clone.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
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
    }
}
