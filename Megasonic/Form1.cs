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

        ProjectSettings CurrentProjectSettings
        {
            get
            {
                return new ProjectSettings()
                {
                    VideoSourceSettings = new VideoSourceSettings()
                    {
                        ImageFile = imageButton.Text,
                        LineFile = lineButton.Text,
                        FrameSize = new Size((int)frameWidthNumeric.Value, (int)frameHeightNumeric.Value)
                    },
                    SoundSourceSettings = new SoundSourceSettings()
                    {
                       SoundFile = soundButton.Text,
                       Framerate = (int)framerateNumeric.Value,
                       Lookahead = (int)lookaheadNumeric.Value,
                       Window = windowCombobox.Text
                    },
                    VideoOutputSettings = new VideoOutputSettings()
                    {
                        VideoFile = videoButton.Text,
                        NoteRange = new Range((int)noteRangeStartNumeric.Value, (int)noteRangeEndNumeric.Value),
                        DecayExponent = (int)decayExponentNumeric.Value,
                        DecayTime = (int)decayTimeNumeric.Value,
                        ColorStart = (int)colorStartNumeric.Value,
                        ColorLength = (int)colorLengthNumeric.Value,
                        BarWidth = (int)barWidthNumeric.Value,
                        Title = titleTextbox.Text
                    }
                };
            }
            set
            {
                ImageFile = value.VideoSourceSettings.ImageFile;
                LineFile = value.VideoSourceSettings.LineFile;
                frameWidthNumeric.Value = value.VideoSourceSettings.FrameSize.Width;
                frameHeightNumeric.Value = value.VideoSourceSettings.FrameSize.Height;

                SoundFile = value.SoundSourceSettings.SoundFile;
                framerateNumeric.Value = value.SoundSourceSettings.Framerate;
                lookaheadNumeric.Value = value.SoundSourceSettings.Lookahead;
                windowCombobox.Text = value.SoundSourceSettings.Window;

                VideoFile = value.VideoOutputSettings.VideoFile;
                noteRangeStartNumeric.Value = value.VideoOutputSettings.NoteRange.Start.Value;
                noteRangeEndNumeric.Value = value.VideoOutputSettings.NoteRange.End.Value;
                decayExponentNumeric.Value = value.VideoOutputSettings.DecayExponent;
                decayTimeNumeric.Value = value.VideoOutputSettings.DecayTime;
                colorStartNumeric.Value = value.VideoOutputSettings.ColorStart;
                colorLengthNumeric.Value = value.VideoOutputSettings.ColorLength;
                barWidthNumeric.Value = value.VideoOutputSettings.BarWidth;
                titleTextbox.Text = value.VideoOutputSettings.Title;
            }
        }

        SoundAnalyzeConditions soundAnalyzeConditions = new SoundAnalyzeConditions();
        VideoConditions videoConditions = new VideoConditions();
        VideoRenderConditions videoRenderConditions = new VideoRenderConditions();
        VideoPreviewConditions videoPreviewConditions = new VideoPreviewConditions();

        public Form1()
        {
            InitializeComponent();
            soundAnalyzeConditions.ConditionsMetEvent += SoundAnalyzeConditions_ConditionsMetEvent;
            videoConditions.ConditionsMetEvent += VideoConditions_ConditionsMetEvent;
            videoRenderConditions.ConditionsMetEvent += VideoRenderConditions_ConditionsMetEvent;
            videoPreviewConditions.ConditionsMetEvent += VideoPreviewConditions_ConditionsMetEvent;
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
            videoConditions.SoundAnalyzed = true;
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