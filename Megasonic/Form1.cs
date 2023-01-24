using MathNet.Numerics.Statistics;
using Microsoft.VisualBasic.Devices;
using Svg;
using System.Drawing.Drawing2D;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tools;

namespace Megasonic
{
    public partial class Form1 : Form
    {
        // TODO
        // instead of just setting true, set false if new text is blank?

        string ImageFile
        {
            get { return imageButton.Text; }
            set
            {
                imageButton.Text = value;
                videoConditions.ImageSelected = true;
            }
        }

        string SoundFile
        {
            get { return soundButton.Text; }
            set
            {
                soundButton.Text = value;
                soundAnalyzeConditions.SoundSelected = true;
            }
        }

        string LineFile
        {
            get { return lineButton.Text; }
            set
            {
                lineButton.Text = value;
                videoConditions.LineSelected = true;
            }
        }

        string VideoFile
        {
            get { return videoButton.Text; }
            set
            {
                videoButton.Text = value;
                videoRenderConditions.VideoSelected = true;
            }
        }

        int FrameRate
        {
            get { return (int)framerateNumeric.Value; }
            set { framerateNumeric.Value = value; }
        }

        Size FrameSize
        {
            get { return new Size((int)frameWidthNumeric.Value, (int)frameHeightNumeric.Value); }
            set { 
                frameWidthNumeric.Value = value.Width;
                frameHeightNumeric.Value = value.Height;
            }
        }

        int Lookahead
        {
            get { return (int)lookaheadNumeric.Value; }
            set { lookaheadNumeric.Value = value; }
        }

        string Window
        {
            get { return windowCombobox.Text; }
            set { windowCombobox.Text = value; }
        }

        int NoteRangeStart
        {
            get { return (int)noteRangeStartNumeric.Value; }
            set { noteRangeStartNumeric.Value = value; }
        }
        int NoteRangeEnd
        {
            get { return (int)noteRangeEndNumeric.Value; }
            set { noteRangeEndNumeric.Value = value; }
        }

        int DecayExponent
        {
            get { return (int)decayExponentNumeric.Value; }
            set { decayExponentNumeric.Value = value; }
        }

        int DecayTime
        {
            get { return (int)decayTimeNumeric.Value; }
            set { decayTimeNumeric.Value = value; }
        }

        int ColorStart
        {
            get { return (int)colorStartNumeric.Value; }
            set { colorStartNumeric.Value = value; }
        }

        int ColorLength
        {
            get { return (int)colorLengthNumeric.Value; }
            set { colorLengthNumeric.Value = value; }
        }

        int BarWidth
        {
            get { return (int)barWidthNumeric.Value; }
            set { barWidthNumeric.Value = value; }
        }

        string Title
        {
            get { return titleTextbox.Text; }
            set { titleTextbox.Text = value; }
        }

        ProjectSettings CurrentProjectSettings
        {
            get
            {
                return new ProjectSettings()
                {
                    VideoSourceSettings = new VideoSourceSettings()
                    {
                        ImageFile = ImageFile,
                        LineFile = LineFile,
                        FrameSize = FrameSize
                    },
                    SoundSourceSettings = new SoundSourceSettings()
                    {
                       SoundFile = SoundFile,
                       FrameRate = FrameRate,
                       Lookahead = Lookahead,
                       Window = Window
                    },
                    VideoOutputSettings = new VideoOutputSettings()
                    {
                        VideoFile = videoButton.Text,
                        NoteRangeStart = NoteRangeStart,
                        NoteRangeEnd = NoteRangeEnd,
                        DecayExponent = DecayExponent,
                        DecayTime = DecayTime,
                        ColorStart = ColorStart,
                        ColorLength = ColorLength,
                        BarWidth = BarWidth,
                        Title = Title
                    }
                };
            }
            set
            {
                ImageFile = value.VideoSourceSettings.ImageFile;
                LineFile = value.VideoSourceSettings.LineFile;
                FrameSize = value.VideoSourceSettings.FrameSize;

                SoundFile = value.SoundSourceSettings.SoundFile;
                FrameRate = value.SoundSourceSettings.FrameRate;
                Lookahead = value.SoundSourceSettings.Lookahead;
                Window = value.SoundSourceSettings.Window;

                VideoFile = value.VideoOutputSettings.VideoFile;
                NoteRangeStart = value.VideoOutputSettings.NoteRangeStart;
                NoteRangeEnd = value.VideoOutputSettings.NoteRangeEnd;
                DecayExponent = value.VideoOutputSettings.DecayExponent;
                DecayTime = value.VideoOutputSettings.DecayTime;
                ColorStart = value.VideoOutputSettings.ColorStart;
                ColorLength = value.VideoOutputSettings.ColorLength;
                BarWidth = value.VideoOutputSettings.BarWidth;
                Title = value.VideoOutputSettings.Title;
            }
        }

        SoundAnalyzeConditions soundAnalyzeConditions = new SoundAnalyzeConditions();
        VideoConditions videoConditions = new VideoConditions();
        VideoRenderConditions videoRenderConditions = new VideoRenderConditions();

        SoundAnalyzer sound;
        VideoRenderer video;

        public Form1()
        {
            InitializeComponent();

            soundAnalyzeConditions.ConditionsMetEvent += SoundAnalyzeConditions_ConditionsMetEvent;
            videoConditions.ConditionsMetEvent += VideoConditions_ConditionsMetEvent;
            videoRenderConditions.ConditionsMetEvent += VideoRenderConditions_ConditionsMetEvent;

            windowCombobox.BeginUpdate();
            foreach (string window in SoundAnalyzer.GetWindows())
            {
                windowCombobox.Items.Add(window);
            }
            windowCombobox.SelectedIndex = 3;
            windowCombobox.EndUpdate();
        }

        private void UpdateVideoPreview()
        {
            Bitmap bitmap = video.Background;
            PointF[] curvePoints = video.CurvePoints;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawLines(new Pen(Brushes.Red, 5), curvePoints);
            }

            sourcePicture.Image?.Dispose();
            sourcePicture.Image = bitmap;
        }

        void RefreshVideoRenderer()
        {
            video?.Dispose();

            video = new VideoRenderer(sound, VideoFile, LineFile, ImageFile, Title, FrameSize,
                BarWidth,
                ColorStart,
                ColorLength,
                DecayExponent,
                DecayTime,
                NoteRangeStart,
                NoteRangeEnd);

            UpdateVideoPreview();
        }

        private void SoundAnalyzeConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            soundAnalyzeButton.Enabled = true;
        }

        private void VideoConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            groupBox4.Enabled = true;
            groupBox3.Enabled = true;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;

            foreach (Control control in groupBox4.Controls)
            {
                if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.ValueChanged += ParametersChanged;
                }
                else if (control is TextBox textBox)
                {
                    textBox.TextChanged += ParametersChanged;
                }
            }

            RefreshVideoRenderer();
        }

        private void VideoRenderConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            videoRenderButton.Enabled = true;
        }

        private void audioButton_Click(object sender, EventArgs e)
        {
            if (soundDialog.ShowDialog(this) == DialogResult.OK)
            {
                SoundFile = soundDialog.FileName;
            }
        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            if (imageDialog.ShowDialog(this) == DialogResult.OK)
            {
                ImageFile = imageDialog.FileName;
            }
        }

        private void lineButton_Click(object sender, EventArgs e)
        {
            if (lineDialog.ShowDialog(this) == DialogResult.OK)
            {
                LineFile = lineDialog.FileName;
            }
        }

        private void soundAnalyze_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            soundAnalyzeButton.Enabled = false;

            sound = new SoundAnalyzer(SoundFile, FrameRate, Lookahead, Window);
            sound.OnProgress += Audio_OnProgress;
            sound.OnComplete += Audio_OnComplete;
            sound.StartProcess();
        }

        private void Audio_OnComplete(object? sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                videoConditions.SoundAnalyzed = true;
            });
        }

        private void Audio_OnProgress(object? sender, ProgressEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                progressBar1.Maximum = e.Total;
                progressBar1.Value = e.Value;
                //label3.Text = $"{e.Value}/{e.Total}"; // TODO figure out where to display this
            });
        }

        private void videoButton_Click(object sender, EventArgs e)
        {
            if (videoDialog.ShowDialog() == DialogResult.OK)
            {
                VideoFile = videoDialog.FileName;
                videoRenderConditions.VideoSelected = true;
            }            
        }

        private void videoRenderButton_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = false;
            videoRenderButton.Enabled = false;

            video.OnProgress += Video_OnProgress;
            video.OnComplete += Video_OnComplete;
            video.StartProcess();
        }

        private void Video_OnComplete(object? sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                MessageBox.Show("Video rendered");
            });
        }

        private void Video_OnProgress(object? sender, ProgressEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                progressBar2.Maximum = Math.Max(e.Total, e.Value);
                progressBar2.Value = e.Value;
                //label4.Text = $"{e.Value}/{e.Total}"; // TODO figure out where to show this
            });
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonString = File.ReadAllText(openProjectDialog.FileName);
                ProjectSettings settings;

                try
                {
                    settings = JsonSerializer.Deserialize<ProjectSettings>(jsonString);                    
                }
                catch
                {
                    return;
                }

                if (settings != null)
                {
                    CurrentProjectSettings = settings;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveProjectDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonString = JsonSerializer.Serialize(CurrentProjectSettings);
                File.WriteAllText(saveProjectDialog.FileName, jsonString);
            }
        }

        void ParametersChanged(object? sender, EventArgs e)
        {
            applyPropertiesButton.Enabled = true;
        }

        private void applyPropertiesButton_Click(object sender, EventArgs e)
        {
            applyPropertiesButton.Enabled = false;

            RefreshVideoRenderer();
        }
    }
}