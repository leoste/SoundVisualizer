using ColorMine.ColorSpaces;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Svg;
using System.Buffers.Text;
using System.Drawing.Drawing2D;

namespace Tools
{
    public static class DrawingAids
    {
        public static Bitmap GetBackground(string imageFilepath, int width, int height)
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

        public static Bitmap GetBackgroundWithTitle(string imageFilepath, int width, int height, string title, int titleHeightA, int titleHeightB)
        {
            Bitmap background = GetBackground(imageFilepath, width, height);

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

        public static PointF[] GetCurvePoints(string svgFilepath, int width, int height)
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

        public static Bitmap GetFrame(PointF[] sourcePoints, PointF[] curvePoints, double curveLength, double[] curveLengths, Bitmap background, Color[] colors, int barMaxAngle, double definition, float columnWidth)
        {
            PointF[] uniformPoints = CurveOperations.SpecifyHorizontally(sourcePoints, definition);

            // curvePoints - svg path
            // uniform points - audio visualization on a straight line
            // curve - visualized onto 

            CurveMorpher curve = new CurveMorpher(curvePoints, uniformPoints, curveLength, curveLengths, false, barMaxAngle);

            // i dont use "using" cause bitmap needs to stay for a while until its really used, then i dispose it
            Bitmap bitmap;
            lock (background)
            {
                bitmap = (Bitmap)background.Clone();
            }
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (int k = 0; k < uniformPoints.Length; k++)
                {
                    Color c = colors[(int)uniformPoints[k].X];
                    Brush brush = new SolidBrush(c);

                    PointF a = curve.Matrix.Points[k];
                    PointF b = curve.Matrix.GetSecondPoint(k, uniformPoints[k].Y);
                    g.DrawLine(new Pen(brush, columnWidth), a, b);
                }
            }

            return bitmap;
        }

        public static float GetColumnWidth(int width, int barRelation)
        {
            float columnRelation = 1f / barRelation;
            float columnWidth = width * columnRelation;
            return columnWidth;
        }

        public static Color[] GetColors(int width, float scaleThingy, float visibleNoteSpan, float noteMinoffset, float colorStartDegree)
        {
            Color[] colors = new Color[width]; //theres only really point in calculating color for each pixel
            float colorRelation = visibleNoteSpan / colors.Length;
            Hsv hsv = new Hsv
            {
                S = 1,
                V = 1
            };
            for (int i = 0; i < colors.Length; i++)
            {
                double thingy = (i * colorRelation + noteMinoffset) % 12;
                if (thingy < 0) thingy = 12 + thingy; // correct negative numbers
                if (thingy > 6) thingy = 12 - thingy; // make it go backwards
                thingy = thingy * scaleThingy; //divide only by 6 cause thats the max number can go to thanks to backwards
                thingy = (thingy + colorStartDegree) % 360;
                hsv.H = thingy;
                IRgb rgb = hsv.ToRgb();
                Color c = Color.FromArgb(255, (int)rgb.R, (int)rgb.G, (int)rgb.B);
                colors[i] = c;
            }
            return colors;
        }

        public static float GetScaleThingy(float colorLengthDegree)
        {
            float scaleThingy = colorLengthDegree / 6;
            return scaleThingy;
        }

        public static (double curveLength, double[] curveLengths, double definition) GetCurveProperties(PointF[] curvePoints, int width, int barRelation)
        {
            CurveOperations.CalculateLength(curvePoints, out double curveLength, out double[] curveLengths);
            double definition = width / barRelation * width / curveLength;
            return (curveLength, curveLengths, definition);
        }

        public static (PointF[] sourcePoints, float visibleNoteSpan, float noteMinoffset) GetSimulatedSourcePoints(int width, int minNoteBorder, int maxNoteBorder, int barRelation, int frameRate, int lookaround)
        {
            // Part 1: sound. save for minor changes, this is heavy copy-paste from SoundAnalyzer with the assumption it won't change much
            // + it was too hard to refactor into reusable simulatable pieces for me to bother

            // Just assume these, they'd be taken from real audio which we don't have in this function
            int channels = 2;
            int sampleRate = 44100;

            double frameLength = 1 / (double)frameRate;
            int lookFactor = 1 + lookaround * 2;
            float FrequencyFidelity = frameRate / (float)lookFactor;
            int precount = (int)(sampleRate * frameLength * channels);
            int frequencyCount = precount * lookFactor;
            int count = frequencyCount;
            int bufferLength = Math.Max(sampleRate, count) + 2;

            // all note distance stuff
            float[] distances = new float[count + 1];
            distances[0] = NoteHelper.GetNoteDistance(1); //hackfix cause cant calculate distance to 0 frequencies
            for (int i = 1; i < distances.Length; i++)
            {
                distances[i] = NoteHelper.GetNoteDistance((int)(i * FrequencyFidelity));
            }
            float maximumNote = distances[distances.Length - 1];
            float minimumNote = distances[0];
            float notesTotalLength = maximumNote - minimumNote;
            // calculating note spans
            NoteSpan[] NoteSpans = new NoteSpan[count];
            for (int i = 0; i < count; i++)
            {
                NoteSpans[i].start = distances[i];
                NoteSpans[i].end = distances[i + 1];
                NoteSpans[i].length = NoteSpans[i].end - NoteSpans[i].start;
            }



            // Part 2: video

            float columnHalfWidth = DrawingAids.GetColumnWidth(width, barRelation) / 2;

            float noteMinoffset = -(minimumNote - minNoteBorder);
            float noteMaxOffset = (maximumNote - maxNoteBorder);
            float visibleNoteSpan = notesTotalLength - noteMinoffset - noteMaxOffset;

            //calculate x, find edges for notes that are actually visible
            float[] x = new float[frequencyCount];
            int startIndex = 0; //index of first note we can start on
            int endIndex = frequencyCount; // index of last note used
            bool startIsntSet = true;
            bool endIsntSet = true;
            for (int i = 0; i < frequencyCount; i++)
            {
                x[i] = (NoteSpans[i].start - minimumNote - noteMinoffset) * (width / visibleNoteSpan) - columnHalfWidth;
                if (startIsntSet)
                {
                    if (NoteSpans[i].start >= minNoteBorder)
                    {
                        startIndex = i;
                        startIsntSet = false;
                    }
                }
                else if (endIsntSet)
                {
                    if (NoteSpans[i].start >= maxNoteBorder)
                    {
                        endIndex = i;
                        endIsntSet = false;
                    }
                }
            }

            int visibleLength = endIndex - startIndex;

            // first get the points that we do know
            PointF[] sourcePoints = new PointF[visibleLength];
            for (int k = 0; k < visibleLength; k++)
            {
                int correctedIndex = k + startIndex;
                float baseh = 1;

                float h = baseh * width / 7f;
                sourcePoints[k] = new PointF(x[correctedIndex], h);
            }

            // really bad hackfix that loses very slight definition and is inaccurate, but....
            // visually isnt that different + its easier than reworking everything to not have this issue
            sourcePoints[0].X = 0;

            return (sourcePoints, visibleNoteSpan, noteMinoffset);
        }
    }
}
