using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class DrawingAids
    {
        public static Bitmap GenerateBackground(string imageFilepath, int width, int height)
        {
            Bitmap background = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(background))
            {
                using (Image image = Image.FromFile(imageFilepath))
                {
                    double scale = Math.Min((double)width / image.Width, (double)height / image.Height);
                    int scaleWidth = (int)(image.Width * scale);
                    int scaleHeight = (int)(image.Height * scale);

                    g.Clear(Color.Black);
                    g.DrawImage(image, (width - scaleWidth) / 2, (height - scaleHeight) / 2, scaleWidth, scaleHeight);
                }
            }

            return background;
        }

        public static Bitmap GenerateBackgroundWithTitle(string imageFilepath, int width, int height, string title, int titleHeightA, int titleHeightB)
        {
            Bitmap background = GenerateBackground(imageFilepath, width, height);

            using (Graphics g = Graphics.FromImage(background))
            {
                float em = width / 35;
                Font font = new Font(FontFamily.GenericSansSerif, em, FontStyle.Bold);
                SizeF size = g.MeasureString(title, font);
                Brush fore = Brushes.White;
                Brush back = Brushes.Black;
                float textX = width / 2 - size.Width / 2;
                float textY = height / titleHeightB * titleHeightA - size.Height / 2;

                g.DrawString(title, font, back, textX + em / 14f, textY + em / 14f);
                g.DrawString(title, font, fore, textX, textY);
            }

            return background;
        }

        public static PointF[] GenerateCurvePoints(string svgFilepath, int width, int height)
        {
            // maybe want to do curve fluxuations or something cool            
            SvgDocument document = SvgDocument.Open(svgFilepath);

            int w = width;
            int h = height;
            float xRatio = w / document.Width;
            float yRatio = h / document.Height;

            using (GraphicsPath path = SvgConverter.ToGraphicsPath(document))
            {
                var matrixPointA = new PointF(document.Bounds.X * xRatio, document.Bounds.Y * yRatio);
                var matrixPointB = new PointF((document.Bounds.X + document.Bounds.Width) * xRatio, document.Bounds.Y * yRatio);
                var matrixPointC = new PointF(document.Bounds.X * xRatio, (document.Bounds.Y + document.Bounds.Height) * yRatio);

                var pathBounds = path.GetBounds();

                Matrix mx = new Matrix(pathBounds, new PointF[] { matrixPointA, matrixPointB, matrixPointC });
                path.Transform(mx);

                using (Matrix flatteningMx = new Matrix(1, 0, 0, 1, 0, 0))
                {
                    path.Flatten(flatteningMx, 0.1f);
                    return path.PathPoints;
                }
            }
        }
    }
}
