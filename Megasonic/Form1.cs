using Svg;
using System.Drawing.Drawing2D;
using Tools;

namespace Megasonic
{
    public partial class Form1 : Form
    {
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

            LineFile = "J:\\heli\\FL Studio Projects\\my music\\Album 2nd\\final version\\rockable\\ccc-silhouette 720px reversed then line.svg";
            ImageFile = "J:\\heli\\FL Studio Projects\\my music\\Album 2nd\\final version\\rockable\\ccc 720px.png";
        }

        private void VideoPreviewConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            using (Image image = Image.FromFile(ImageFile))
            {
                Bitmap bitmap = new Bitmap(image);

                GraphicsPath path = Tools.SvgConverter.ToGraphicsPath(SvgDocument.Open(LineFile));

                using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0)) path.Flatten(mx, 0.1f);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawLines(new Pen(Brushes.Red, 5), path.PathPoints);
                }

                sourcePicture.Image = bitmap;
            }
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
                videoButton.Text = videoDialog.FileName;
                videoRenderConditions.VideoSelected = true;
            }            
        }

        private void videoRenderButton_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = false;
            videoRenderButton.Enabled = false;
        }
    }
}