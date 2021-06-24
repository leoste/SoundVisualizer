using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatVideoMaker
{
    //another name idea was CurveElaborator
    class CurveSpecifier
    {
        public static PointF[] SpecifyHorizontally(PointF[] sourcePoints, double definition)
        {
            // pre-work curve to not lose definition on sharp vertical climbs
            PointF[] curve;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddCurve(sourcePoints);
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
    }
}
