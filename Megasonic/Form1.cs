using MathNet.Numerics.Statistics;
using Megasonic.Conditions;
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
                foregroundCustomizationConditions.SetImageSelectedTrue();
            }
        }

        string SoundFile
        {
            get { return soundButton.Text; }
            set
            {
                soundButton.Text = value;
                soundAnalyzeConditions.SetSoundSelectedTrue();
            }
        }

        string LineFile
        {
            get { return lineButton.Text; }
            set
            {
                lineButton.Text = value;
                foregroundCustomizationConditions.SetLineSelectedTrue();
            }
        }

        string VideoFile
        {
            get { return videoButton.Text; }
            set
            {
                videoButton.Text = value;
                videoRenderConditions.SetVideoSelectedTrue();
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

        int BarMaxAngle
        {
            get { return (int)barMaxAngleNumeric.Value; }
            set { barMaxAngleNumeric.Value = value; }
        }

        string Title
        {
            get { return titleTextbox.Text; }
            set { titleTextbox.Text = value; }
        }

        int TitleHeightA
        {
            get { return (int)titleHeightANumeric.Value; }
            set { titleHeightANumeric.Value = value; }
        }

        int TitleHeightB
        {
            get { return (int)titleHeightBNumeric.Value; }
            set { titleHeightBNumeric.Value = value; }
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
                        BarMaxAngle = BarMaxAngle,
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

        BackgroundCustomizationConditions backgroundCustomizationConditions;
        ForegroundCustomizationConditions foregroundCustomizationConditions;
        SoundCustomizationConditions soundCustomizationConditions;
        VideoCustomizationConditions videoCustomizationConditions;
        VideoRenderConditions videoRenderConditions;
        SoundAnalyzeConditions soundAnalyzeConditions;

        SoundAnalyzer sound;
        VideoRenderer video;

        public Form1()
        {
            InitializeComponent();

            backgroundCustomizationConditions = new BackgroundCustomizationConditions(backgroundCustomizationControl);
            foregroundCustomizationConditions = new ForegroundCustomizationConditions(foregroundCustomizationControl);            
            soundCustomizationConditions = new SoundCustomizationConditions(soundCustomizationControl);
            soundAnalyzeConditions = new SoundAnalyzeConditions(soundAnalyzeControl);
            videoCustomizationConditions = new VideoCustomizationConditions(videoCustomizationControl, foregroundCustomizationConditions, soundAnalyzeConditions);
            videoRenderConditions = new VideoRenderConditions(videoRenderControl, videoCustomizationConditions);

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
            preview2.Image?.Dispose();
            preview3.Image?.Dispose();

            Bitmap bitmap = video.Background;
            PointF[] curvePoints = video.CurvePoints;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawLines(new Pen(Brushes.Red, 5), curvePoints);
            }

            preview2.Image = bitmap;

            Bitmap bitmap2 = video.GetFrame(video.GetSourcePoints(video.MaxIndex / 2)).Source;
            preview3.Image = bitmap2;

        }

        void RefreshVideoRenderer()
        {
            video?.Dispose();

            video = new VideoRenderer(sound, VideoFile, LineFile, ImageFile, Title, FrameSize,
                BarWidth,
                BarMaxAngle,
                ColorStart,
                ColorLength,
                DecayExponent,
                DecayTime,
                NoteRangeStart,
                NoteRangeEnd,
                TitleHeightA,
                TitleHeightB);

            UpdateVideoPreview();
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
            soundCustomizationConditions.SetSoundNotAnalyzedFalse();

            sound = new SoundAnalyzer(SoundFile, FrameRate, Lookahead, Window);
            sound.OnProgress += Audio_OnProgress;
            sound.OnComplete += Audio_OnComplete;
            sound.StartProcess();
        }

        private void Audio_OnComplete(object? sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                videoCustomizationConditions.SetSoundAnalyzedTrue();
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
            }            
        }

        private void videoRenderButton_Click(object sender, EventArgs e)
        {
            RefreshVideoRenderer();

            backgroundCustomizationConditions.SetVideoNotRenderedFalse();
            videoCustomizationConditions.SetVideoNotRenderedFalse();
            foregroundCustomizationConditions.SetVideoNotRenderedFalse();

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