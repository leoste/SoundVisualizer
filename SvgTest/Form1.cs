using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Svg;

namespace SvgTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string filename = "D:\\randomsound\\bbb-real-background.svg"; // put something here to test, cant be bothered to make fancy tester

            SvgDocument document = SvgDocument.Open(filename);
            RectangleF rect = document.Path.GetBounds();
            Bitmap bmp = new Bitmap((int)(rect.Width), (int)(rect.Height ));

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                using (GraphicsPathIterator iterator = new GraphicsPathIterator(document.Path))
                {
                    iterator.NextSubpath(out int startIndex, out int endIndex, out bool isClosed);

                    using (GraphicsPath path = new GraphicsPath(
                        document.Path.PathPoints.Take(endIndex).ToArray(),
                        document.Path.PathTypes.Take(endIndex).ToArray(),
                        document.Path.FillMode))
                    {
                        path.Transform(new Matrix(rect, new PointF[] {
                            new PointF(0, 0),
                            new PointF(rect.Width, 0),
                            new PointF(0, rect.Height)
                        }));

                        using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0)) path.Flatten(mx, 0.1f);

                        //g.DrawPath(new Pen(Brushes.Black, 10), path);
                        g.DrawLines(new Pen(Brushes.Black, 10), path.PathPoints);
                    }
                }
            }

            pictureBox1.Image = bmp;
        }
    }
}
