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

        public Form1()
        {
            InitializeComponent();

            Bitmap bitmap = new Bitmap(600, 600);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int lineWidth = 2;

                Pen black = new Pen(Brushes.Black, lineWidth);
                Pen red = new Pen(Brushes.Red, lineWidth);
                Pen green = new Pen(Brushes.LimeGreen, lineWidth);
                Pen blue = new Pen(Brushes.LightBlue, lineWidth);

                g.Clear(Color.White);

                PointF[] sourcePoints = new PointF[] { new PointF(0, 300), new PointF(100, 400), new PointF(150, 200), new PointF(200, 500), new PointF(300, 350), new PointF(400, 250), new PointF(500, 400), new PointF(600, 300) };

                PointF[] uniformPoints;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddCurve(sourcePoints);
                    using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                    {
                        path.Flatten(mx, 5f);
                        uniformPoints = path.PathPoints;
                    }
                }

                // draw base curve
                g.DrawCurve(black, uniformPoints);
                g.DrawLines(red, uniformPoints);

                double[] angles = new double[uniformPoints.Length];
                for (int i = 1; i < uniformPoints.Length - 1; i++)
                {
                    PointF leftPoint = uniformPoints[i - 1];
                    PointF middlePoint = uniformPoints[i];
                    PointF rightPoint = uniformPoints[i + 1];

                    double leftAngle = getLineAngle(leftPoint, middlePoint);
                    double rightAngle = getLineAngle(middlePoint, rightPoint);
                    angles[i] = (leftAngle + rightAngle) / 2 - 1.5707963267948966192313216916398; // 90 degrees in radians

                    double length = 15; // arbitrary length
                    g.DrawLine(green, middlePoint, getLineSecondPoint(middlePoint, angles[i], length));
                }
            }

            pictureBox1.Image = bitmap;
        }
    }
}
