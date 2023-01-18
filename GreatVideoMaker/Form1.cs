using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlyleafLib.MediaPlayer;

namespace GreatVideoMaker
{
    public partial class Form1 : Form
    {
        private SoundAnalyzer audio;
        private VideoRenderer video;
        private Player player;
        private System.Diagnostics.Stopwatch watch;
                
        private string AudioPath
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        private string SvgPath
        {
            get { return label17.Text; }
            set { label17.Text = value; }
        }

        private string ImagePath
        {
            get { return label18.Text; }
            set { label18.Text = value; }
        }

        private string VideoPath
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }
        private string Title
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        private int ProcessorCount
        {
            get { return (int)numericUpDown12.Value; }
            set { numericUpDown12.Value = value; }
        }
        private int FrameRate { get { return (int)numericUpDown1.Value; } }
        private int LookAhead { get { return (int)numericUpDown2.Value; } }
        private int BarRelation { get { return (int)numericUpDown4.Value; } }
        private float ColorStartDegree { get { return (float)numericUpDown3.Value; } }
        private float ColorLengthDegree { get { return (float)numericUpDown5.Value; } }
        private int DecayExponent { get { return (int)numericUpDown6.Value; } }
        private int DecayTime { get { return (int)numericUpDown7.Value; } }
        private Size FrameSize { get { return new Size((int)numericUpDown9.Value, (int)numericUpDown8.Value); } }
        private int MinNote { get { return (int)numericUpDown11.Value; } }
        private int MaxNote { get { return (int)numericUpDown10.Value; } }

        public Form1()
        {
            InitializeComponent();
            watch = new System.Diagnostics.Stopwatch();

            ProcessorCount = Environment.ProcessorCount;

            player = new Player();
            FlyleafLib.Master.RegisterFFmpeg("C:\\programs");
            player.Control = flyleaf1;

            MethodInfo[] windows = typeof(MathNet.Numerics.Window).GetMethods();
            comboBox1.BeginUpdate();
            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i].ReturnType == typeof(double[])) comboBox1.Items.Add(windows[i].Name);
            }
            comboBox1.SelectedIndex = 3;
            comboBox1.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AudioPath = openFileDialog1.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                SvgPath = openFileDialog2.FileName;
            }
        }

        private void Audio_OnComplete(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                
            });
        }

        private void Audio_OnProgress(object sender, ProgressEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                progressBar1.Maximum = e.Total;
                progressBar1.Value = e.Value;
                label3.Text = $"{e.Value}/{e.Total}";
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                VideoPath = saveFileDialog1.FileName;
            }
            else
            {
                VideoPath = "";
            }
        }

        private void Video_OnComplete(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                watch.Stop();
                label15.Text = watch.Elapsed.ToString();
                player.Open(VideoPath);
                player.OpenCompleted += (o, x) => { if (x.success && x.type == FlyleafLib.MediaType.Video) player.Play(); };
            });
        }

        private void Video_OnProgress(object sender, ProgressEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                progressBar2.Maximum = Math.Max(e.Total, e.Value);
                progressBar2.Value = e.Value;
                label4.Text = $"{e.Value}/{e.Total}";
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            audio = new SoundAnalyzer(AudioPath, FrameRate, LookAhead, comboBox1.SelectedItem as string);
            audio.OnProgress += Audio_OnProgress;
            audio.OnComplete += Audio_OnComplete;
            audio.StartProcess();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            video = new VideoRenderer(audio, VideoPath, SvgPath, ImagePath, Title, FrameSize,
                BarRelation,
                ColorStartDegree,
                ColorLengthDegree,
                DecayExponent,
                DecayTime,
                MinNote,
                MaxNote);
            video.OnProgress += Video_OnProgress;
            video.OnComplete += Video_OnComplete;
            player.Stop();
            watch.Reset();
            watch.Start();
            video.StartProcess();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            RenderInfo.ProcessorCount = (int)numericUpDown12.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                ImagePath = openFileDialog3.FileName;
            }
        }

        private void savePresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                List<string> lines = new List<string>();

                lines.Add(numericUpDown12.Value.ToString());
                lines.Add(label1.Text);
                lines.Add(label17.Text);
                lines.Add(label18.Text);
                lines.Add(numericUpDown1.Value.ToString());
                lines.Add(numericUpDown2.Value.ToString());
                lines.Add(comboBox1.Text);
                lines.Add(label2.Text);
                lines.Add(numericUpDown9.Value.ToString());
                lines.Add(numericUpDown8.Value.ToString());
                lines.Add(numericUpDown4.Value.ToString());
                lines.Add(numericUpDown3.Value.ToString());
                lines.Add(numericUpDown5.Value.ToString());
                lines.Add(numericUpDown6.Value.ToString());
                lines.Add(numericUpDown7.Value.ToString());
                lines.Add(numericUpDown11.Value.ToString());
                lines.Add(numericUpDown10.Value.ToString());
                lines.Add(textBox1.Text);

                File.WriteAllLines(saveFileDialog2.FileName, lines);
            }
        }

        private void loadPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog4.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(openFileDialog4.FileName);

                numericUpDown12.Value = decimal.Parse(lines[0]);
                label1.Text = lines[1];
                label17.Text = lines[2];
                label18.Text = lines[3];
                numericUpDown1.Value = decimal.Parse(lines[4]);
                numericUpDown2.Value = decimal.Parse(lines[5]);
                comboBox1.Text = lines[6];
                label12.Text = lines[7];
                numericUpDown9.Value = decimal.Parse(lines[8]);
                numericUpDown8.Value = decimal.Parse(lines[9]);
                numericUpDown4.Value = decimal.Parse(lines[10]);
                numericUpDown3.Value = decimal.Parse(lines[11]);
                numericUpDown5.Value = decimal.Parse(lines[12]);
                numericUpDown6.Value = decimal.Parse(lines[13]);
                numericUpDown7.Value = decimal.Parse(lines[14]);
                numericUpDown11.Value = decimal.Parse(lines[15]);
                numericUpDown10.Value = decimal.Parse(lines[16]);
                textBox1.Text = lines[17];
            }
        }
    }
}
