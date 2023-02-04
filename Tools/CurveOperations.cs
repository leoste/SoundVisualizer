using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    //another name idea was CurveElaborator
    class CurveOperations
    {
        public static PointF[] SpecifyHorizontally(PointF[] curvePoints, double definition)
        {
            // pre-work curve to not lose definition on sharp vertical climbs
            PointF[] curve;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddCurve(curvePoints);
                using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                {
                    path.Flatten(mx, 0.1f);
                    curve = path.PathPoints;
                }
            };

            List<PointF> newCurve = new List<PointF>();

            int lastCurveIndex = 0;
            int curveIndex = 1;
            double currentPos = curve[0].X;
            double finalPos = curve[curve.Length - 1].X;
            newCurve.Add(curve[0]);

            while ((currentPos += definition) < finalPos - 0.0000001)
            {
                if (currentPos > curve[curveIndex].X + 0.0000001)
                {
                    currentPos = curve[curveIndex].X;

                    lastCurveIndex = curveIndex;
                    curveIndex++;
                }

                double diff = currentPos - curve[lastCurveIndex].X;
                double relation = diff / (curve[curveIndex].X - curve[lastCurveIndex].X);
                // hackfix but meh happens only in edgecases
                if (double.IsNaN(relation)) relation = 0;

                PointF a = curve[lastCurveIndex];
                PointF b = curve[curveIndex];
                PointF c = new PointF(
                    (float)(a.X + (b.X - a.X) * relation),
                    (float)(a.Y + (b.Y - a.Y) * relation)
                );

                newCurve.Add(c);
            };

            return newCurve.ToArray();
        }

        // the reverse loop with --i integrated into calculations saves really tiny bit of efficiency.
        // if it was the right way, would have to subtract again when writing into curveLengths
        public static void CalculateLength(PointF[] curvePoints, out double curveLength, out double[] curveLengths)
        {
            curveLength = 0;
            curveLengths = new double[curvePoints.Length - 1];

            for (int i = curveLengths.Length; i > 0;)
            {
                PointF b = curvePoints[i];
                PointF a = curvePoints[--i];

                curveLengths[i] = Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
                curveLength += curveLengths[i];
            }
        }
    }
}
