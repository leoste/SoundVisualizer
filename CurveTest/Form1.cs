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
        void AddPanel()
        {
            Panel panel = new Panel() { Width = 180, Height = 30 };

            flowLayoutPanel1.Controls.Add(panel);
            NumericUpDown numx = new NumericUpDown() { Width = 80, Height = 25, Maximum = 1000 };
            NumericUpDown numy = new NumericUpDown() { Width = 80, Height = 25, Left = 90, Maximum = 1000 };
            panel.Controls.Add(numx);
            panel.Controls.Add(numy);

            void ValueChanged(object sender, EventArgs e)
            {
                CalculateCurve();
            }

            numx.ValueChanged += ValueChanged;
            numy.ValueChanged += ValueChanged;
        }

        void CalculateCurve()
        {
            List<PointF> points = new List<PointF>();
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                PointF point = new PointF(
                    (float)(control.Controls[0] as NumericUpDown).Value,
                    (float)(control.Controls[1] as NumericUpDown).Value
                    );
                points.Add(point);
            }

            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
            Bitmap bitmap = new Bitmap(1000, 1000);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int lineWidth = 2;

                Pen black = new Pen(Brushes.Black, lineWidth);
                Pen red = new Pen(Brushes.Red, lineWidth);
                Pen green = new Pen(Brushes.LimeGreen, lineWidth);
                Pen blue = new Pen(Brushes.LightBlue, lineWidth);

                g.Clear(Color.White);
                g.DrawCurve(black, points.ToArray());
            }

            pictureBox1.Image = bitmap;
        }

        public Form1()
        {
            InitializeComponent();
            AddPanel();
            AddPanel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPanel();
        }
    }
}
