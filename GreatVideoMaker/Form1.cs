using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private string VideoPath
        {
            get { return label2.Text; }
            set { label2.Text = value; }
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
            FlyleafLib.Master.RegisterFFmpeg("ffmpeg");
            player.Control = flyleaf1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AudioPath = openFileDialog1.FileName;
                audio = new SoundAnalyzer(AudioPath, FrameRate, LookAhead);
                audio.OnProgress += Audio_OnProgress;
                audio.OnComplete += Audio_OnComplete;
            }
        }

        private void Audio_OnComplete(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                int test = 0;            
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
                video = new VideoRenderer(audio, VideoPath, FrameSize,
                    BarRelation,
                    ColorStartDegree,
                    ColorLengthDegree,
                    DecayExponent,
                    DecayTime,
                    MinNote,
                    MaxNote);
                video.OnProgress += Video_OnProgress;
                video.OnComplete += Video_OnComplete;
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
            audio.StartProcess();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            player.Stop();
            watch.Reset();
            watch.Start();
            video.StartProcess();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            RenderInfo.ProcessorCount = (int)numericUpDown12.Value;
        }
    }
}
