using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    //morphs given points to locations on a curve, retaining the horizontal distance of the points from each other.
    class CurveMorpher
    {
        private readonly double rad90deg = 1.5707963267948966192313216916398; // 90 degrees in radians

        private PointF[] morphedPoints;        

        private PointF[] curvePoints;
        private byte[] curveTypes;
        private double curveLength;
        private double[] curveLengths;
        private double maxAngleChange;
        //private Vector[] dividedVectors;
        private VectorMatrix matrix;

        public VectorMatrix Matrix { get { return matrix; } }
        public PointF[] BaseCurve { get { return curvePoints; } }

        public CurveMorpher(PointF[] sourcePoints, byte[] sourceTypes, PointF[] morphedPoints, double curveLength, double[] curveLengths, bool isCurve = true, double maxAngleChange = 1)
        {
            this.morphedPoints = morphedPoints;
            this.curveLength = curveLength;
            this.curveLengths = curveLengths;
            this.maxAngleChange = maxAngleChange;

            if (isCurve)
            {
                // calculate base curve from source points
                using (GraphicsPath path = new GraphicsPath(sourcePoints, sourceTypes))
                {
                    using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                    {
                        path.Flatten(mx, 0.1f);
                        curvePoints = path.PathPoints;
                        curveTypes = path.PathTypes;
                    }
                }
            }
            else
            {
                curvePoints = sourcePoints;
                curveTypes = sourceTypes;
            }

            // calculate equal distance points on the curve
            DivideCurve();
        }

        double Get3PointAngle(PointF left, PointF middle, PointF right)
        {
            double leftAngle = Math.Atan2(middle.Y - left.Y, middle.X - left.X);
            double rightAngle = Math.Atan2(right.Y - middle.Y, right.X - middle.X);
            double angle = (leftAngle + rightAngle) / 2 - rad90deg;
            return angle;
        }

        void DivideCurve()
        {
            int segmentPointCount = morphedPoints.Length;

            PointF[] points = new PointF[segmentPointCount];
            double[] angles = new double[segmentPointCount];

            double currentPos = 0;
            double lastLength = 0;
            int curveIndex = 0;
            int dividedIndex = 0;
            int lastCurveIndex = 0;

            while (dividedIndex < segmentPointCount)
            {
                double currentGoal = (double)morphedPoints[dividedIndex].X / morphedPoints[morphedPoints.Length - 1].X * curveLength;

                if (currentPos < currentGoal - 0.0000001)
                {
                    currentPos += curveLengths[curveIndex];
                    lastLength = curveLengths[curveIndex];
                    lastCurveIndex = curveIndex;
                    curveIndex++;
                }
                else
                {
                    double diff = currentPos - currentGoal;
                    double relation = 1 - diff / lastLength;
                    PointF a = curvePoints[lastCurveIndex];
                    PointF b = curvePoints[curveIndex];
                    PointF c = new PointF(
                        (float)(a.X + (b.X - a.X) * relation),
                        (float)(a.Y + (b.Y - a.Y) * relation)
                    );
                    points[dividedIndex] = c;

                    angles[dividedIndex] = Get3PointAngle(a, c, b);

                    dividedIndex++;
                }
            }

            // corners dont get calculated in algorithm, for points its pointless for angles cant do 3 point calculation, only 2
            // TODO: Need to think of good way to make first point relate to last point, same way like all points usually relate
            points[0] = curvePoints[0];
            // This would theoretically make the connection between two parts seamless
            angles[0] = Get3PointAngle(points[segmentPointCount - 1], points[0], points[1]);

            // very crude angle limiter. but true version would do it gracefully not a hard limit block.
            for (int i = 1 + 1; i < angles.Length; i++)
            {
                double maxDiff = rad90deg / maxAngleChange;
                if (angles[i] - angles[i - 1] > maxDiff)
                {
                    angles[i] = angles[i - 1] + maxDiff;
                }
                else if (angles[i] - angles[i - 1] < -maxDiff)
                {
                    angles[i] = angles[i - 1] - maxDiff;
                }
            }

            matrix = new VectorMatrix(points, angles);
        }

        public class VectorMatrix
        {
            private double[,] secondPointMultipliers;

            public int Length { get; private set; }
            public PointF[] Points { get; private set; }
            public double[] Angles { get; private set; }
            public VectorMatrix(PointF[] points, double[] angles)
            {
                if (points.Length != angles.Length) throw new Exception("Lengths must be the same.");

                Length = points.Length;
                Points = points;
                Angles = angles;

                secondPointMultipliers = new double[Length, 2];

                for (int i = 0; i < Length; i++)
                {
                    secondPointMultipliers[i, 0] = Math.Cos(angles[i]);
                    secondPointMultipliers[i, 1] = Math.Sin(angles[i]);
                }
            }

            public PointF GetSecondPoint(int index, double length)
            {
                return new PointF(
                    Points[index].X + (int)(secondPointMultipliers[index, 0] * length),
                    Points[index].Y + (int)(secondPointMultipliers[index, 1] * length)
                    );
            }
        }

        public class Vector
        {
            public PointF Point { get; private set; }
            public double Angle { get; private set; }

            public Vector(PointF point, double angle)
            {
                Point = point;
                Angle = angle;
            }
        }
    }
}
