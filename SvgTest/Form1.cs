using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
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

            string filename = "D:\\361274.svg"; // put something here to test, cant be bothered to make fancy tester

            SvgDocument document = SvgDocument.Open(filename);

            RectangleF rect = document.Path.GetBounds();

            List<GraphicsPath> paths = new List<GraphicsPath>();
            using (GraphicsPathIterator iterator = new GraphicsPathIterator(document.Path))
            {
                for (int i = 0; i < iterator.SubpathCount; i++)
                {
                    iterator.NextMarker(out int startIndex, out int endIndex);
                    //iterator.NextSubpath(out int startIndex, out int endIndex, out bool isClosed);
                    int takeCount = endIndex + 1;

                    PointF[] newPathPoints = document.Path.PathPoints.Skip(startIndex).Take(takeCount).ToArray();
                    byte[] newPathTypes = document.Path.PathTypes.Skip(startIndex).Take(takeCount).ToArray();

                    GraphicsPath path = new GraphicsPath(newPathPoints, newPathTypes, document.Path.FillMode);

                    paths.Add(path);
                }
            }

            /*path.Transform(new Matrix(rect, new PointF[] {
                new PointF(0, 0),
                new PointF(rect.Width, 0),
                new PointF(0, rect.Height)
            }));*/

            Bitmap bmp = new Bitmap((int)(rect.Width), (int)(rect.Height ));

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                for (int i = 0; i < paths.Count; i++)
                {
                    g.DrawLines(new Pen(PickBrush(), 2), paths[i].PathPoints);
                }
            }

            pictureBox1.Image = bmp;
        }

        static int random = 0;
        private Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            //int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            random++;

            return result;
        }
    }
}
