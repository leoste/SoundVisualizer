using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurveTest
{
    public partial class Form1 : Form
    {
        // segmentcount is how many equal length segments there should be
        PointF[] getCurve(out PointF[] baseCurve, double segmentCount = 10)
        {
            PointF[] sourcePoints = new PointF[] { new PointF(0, 300), new PointF(100, 400), new PointF(150, 200), new PointF(200, 500), new PointF(300, 350), new PointF(400, 250), new PointF(500, 400), new PointF(600, 300) };
            sourcePoints = new PointF[] { new PointF(100, 200), new PointF(300, 100), new PointF(500, 300), new PointF(300, 500), new PointF(100, 400) };

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddCurve(sourcePoints);
                using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                {
                    path.Flatten(mx, 5f);
                    baseCurve = path.PathPoints;
                }
            }

            double length = getCurveLength(baseCurve, out double[] lengths);
            PointF[] dividedPoints = divideCurve(baseCurve, lengths, length, segmentCount);

            return dividedPoints;
        }

        double getCurveLength(PointF[] curve, out double[] lengths)
        {
            double length = 0;
            lengths = new double[curve.Length - 1];

            for (int i = 0; i < lengths.Length; i++)
            {
                PointF a = curve[i];
                PointF b = curve[i + 1];

                lengths[i] = Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)); ;
                length += lengths[i];
            }

            return length;
        }

        PointF[] divideCurve(PointF[] curve, double[] curveLengths, double curveLength, double segmentCount)
        {
            PointF[] dividedPoints = new PointF[(int)Math.Ceiling(segmentCount + 1)];
            dividedPoints[0] = curve[0];
            dividedPoints[dividedPoints.Length - 1] = curve[curve.Length - 1];
            double segmentLength = curveLength / segmentCount;

            double currentGoal = segmentLength;
            double currentPos = 0;
            double lastLength = 0;
            int curveIndex = 0;
            int dividedIndex = 1;
                        
            while (dividedIndex < dividedPoints.Length)
            {
                if (currentPos < currentGoal - 0.0000001)
                {
                    currentPos += curveLengths[curveIndex];
                    lastLength = curveLengths[curveIndex];
                    curveIndex++;
                }
                else
                {
                    double diff = currentPos - currentGoal;
                    double relation = 1 - diff / lastLength;

                    PointF a = curve[curveIndex - 1];
                    PointF b = curve[curveIndex];

                    PointF c = new PointF(
                        (float)(a.X + (b.X - a.X) * relation),
                        (float)(a.Y + (b.Y - a.Y) * relation)
                    );
                    dividedPoints[dividedIndex] = c;
                    dividedIndex++;

                    currentGoal += segmentLength;
                }
            }

            return dividedPoints;
        }

        // Angle returned in radians
        double getLineAngle(PointF a, PointF b)
        {
            double xDiff = b.X - a.X;
            double yDiff = b.Y - a.Y;
            return Math.Atan2(yDiff, xDiff);
        }

        // Angle needs to be in radians
        PointF getLineSecondPoint(PointF a, double angle, double length)
        {
            return new PointF((float)(a.X + Math.Cos(angle) * length), (float)(a.Y + Math.Sin(angle) * length));
        }

        private void DrawCurve()
        {
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();

            Bitmap bitmap = new Bitmap(600, 600);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int lineWidth = 2;

                Pen black = new Pen(Brushes.Black, lineWidth);
                Pen red = new Pen(Brushes.Red, lineWidth);
                Pen green = new Pen(Brushes.LimeGreen, lineWidth);
                Pen blue = new Pen(Brushes.LightBlue, lineWidth);

                g.Clear(Color.White);

                PointF[] curve = getCurve(out PointF[] baseCurve, (double)numericUpDown1.Value);

                g.DrawLines(black, baseCurve);
                g.DrawLines(red, curve);

                double[] angles = new double[curve.Length];
                for (int i = 1; i < curve.Length - 1; i++)
                {
                    PointF leftPoint = curve[i - 1];
                    PointF middlePoint = curve[i];
                    PointF rightPoint = curve[i + 1];

                    double leftAngle = getLineAngle(leftPoint, middlePoint);
                    double rightAngle = getLineAngle(middlePoint, rightPoint);
                    angles[i] = (leftAngle + rightAngle) / 2 - 1.5707963267948966192313216916398; // 90 degrees in radians

                    g.DrawLine(green, middlePoint, getLineSecondPoint(middlePoint, angles[i], 15));
                }
            }

            pictureBox1.Image = bitmap;
        }

        public Form1()
        {
            InitializeComponent();

            DrawCurve();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DrawCurve();
        }
    }
}
