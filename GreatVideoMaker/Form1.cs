using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreatVideoMaker
{
    public partial class Form1 : Form
    {
        private SoundAnalyzer audio;
        private VideoRenderer video;

        private int FrameRate
        {
            get { return (int)numericUpDown1.Value; }
        }

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

        private int LookAhead
        {
            get { return (int)numericUpDown2.Value; }
            set { numericUpDown2.Value = value; }
        }

        private int BarRelation
        {
            get { return (int)numericUpDown4.Value; }
            set { numericUpDown4.Value = value; }
        }

        public Form1()
        {
            InitializeComponent();
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
                video = new VideoRenderer(VideoPath, audio, BarRelation);
                video.OnProgress += Video_OnProgress;
                video.OnComplete += Video_OnComplete;
            }
        }

        private void Video_OnComplete(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {

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
            video.StartProcess();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Serializer.WriteToXmlFile(saveFileDialog2.FileName, audio.Data);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                SoundAnalyzerData data = Serializer.ReadFromXmlFile<SoundAnalyzerData>(openFileDialog2.FileName);
                audio = new SoundAnalyzer(data);
            }
        }
    }
}
