using Microsoft.VisualBasic.Devices;
using Svg;
using System.Drawing.Drawing2D;
using System.IO;
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
                videoPreviewConditions.ImageSelected = true;
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
                videoPreviewConditions.LineSelected = true;
            }
        }

        string VideoFile
        {
            get { return videoButton.Text; }
            set
            {
                videoButton.Text = value;
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

        Range NoteRange
        {
            get { return new Range((int)noteRangeStartNumeric.Value, (int)noteRangeEndNumeric.Value); }
            set
            {
                noteRangeStartNumeric.Value = value.Start.Value;
                noteRangeEndNumeric.Value = value.End.Value;
            }
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
                        NoteRange = NoteRange,
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
                NoteRange = value.VideoOutputSettings.NoteRange;
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
        VideoPreviewConditions videoPreviewConditions = new VideoPreviewConditions();

        SoundAnalyzer sound;
        VideoRenderer video;

        public Form1()
        {
            InitializeComponent();

            soundAnalyzeConditions.ConditionsMetEvent += SoundAnalyzeConditions_ConditionsMetEvent;
            videoConditions.ConditionsMetEvent += VideoConditions_ConditionsMetEvent;
            videoRenderConditions.ConditionsMetEvent += VideoRenderConditions_ConditionsMetEvent;
            videoPreviewConditions.ConditionsMetEvent += VideoPreviewConditions_ConditionsMetEvent;

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
            int w = (int)frameWidthNumeric.Value;
            int h = (int)frameHeightNumeric.Value;

            Bitmap bitmap = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (Image image = Image.FromFile(ImageFile))
                {
                    g.DrawImage(image, 0, 0, w, h);
                }

                SvgDocument document = SvgDocument.Open(LineFile);
                GraphicsPath path = SvgConverter.ToGraphicsPath(document);

                float xRatio = w / document.Width;
                float yRatio = h / document.Height;

                Matrix mx = new Matrix(path.GetBounds(), new PointF[] {
                    new PointF(document.Bounds.X * xRatio, document.Bounds.Y * yRatio),
                    new PointF((document.Bounds.X + document.Bounds.Width) * xRatio, document.Bounds.Y * yRatio),
                    new PointF(document.Bounds.X * xRatio, (document.Bounds.Y + document.Bounds.Height) * yRatio)
                });
                
                path.Transform(mx);

                //using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0)) path.Flatten(mx, 0.1f);
                g.DrawLines(new Pen(Brushes.Red, 5), path.PathPoints);
            }

            sourcePicture.Image?.Dispose();
            sourcePicture.Image = bitmap;
        }

        private void VideoPreviewConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            UpdateVideoPreview();
        }

        private void SoundAnalyzeConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            soundAnalyzeButton.Enabled = true;
        }

        private void VideoConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            groupBox3.Enabled = true;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
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

        private void Audio_OnComplete(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                videoConditions.SoundAnalyzed = true;
            });
        }

        private void Audio_OnProgress(object sender, ProgressEventArgs e)
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
        }

        private void frameWidthNumeric_ValueChanged(object sender, EventArgs e)
        {
            UpdateVideoPreview();
        }

        private void frameHeightNumeric_ValueChanged(object sender, EventArgs e)
        {
            UpdateVideoPreview();
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
    }
}