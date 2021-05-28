﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatVideoMaker
{
    class CurveMorpher
    {
        private PointF[] sourcePoints;
        private PointF[] morphingPoints;

        private PointF[] curve;
        private double curveLength;
        private double[] curveLengths;
        //private Vector[] dividedVectors;
        private VectorMatrix matrix;

        public VectorMatrix Matrix { get { return matrix; } }
        public PointF[] BaseCurve { get { return curve; } }

        public CurveMorpher(PointF[] sourcePoints, PointF[] morphedPoints)
        {
            this.sourcePoints = sourcePoints;
            this.morphingPoints = morphedPoints;

            // calculate base curve from source points
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddCurve(this.sourcePoints);
                using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                {
                    path.Flatten(mx, 0.1f);
                    curve = path.PathPoints;
                }
            }

            // calculate length of the curve
            curveLength = 0;
            curveLengths = new double[curve.Length - 1];
            for (int i = 0; i < curveLengths.Length; i++)
            {
                PointF a = curve[i];
                PointF b = curve[i + 1];

                curveLengths[i] = Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
                curveLength += curveLengths[i];
            }

            // calculate equal distance points on the curve
            DivideCurve();
        }

        void DivideCurve()
        {
            int segmentPointCount = morphingPoints.Length + 1;

            PointF[] points = new PointF[segmentPointCount + 1];
            double[] angles = new double[segmentPointCount + 1];
            points[0] = curve[0];
            points[points.Length - 1] = curve[curve.Length - 1];

            double currentPos = 0;
            double lastLength = 0;
            int curveIndex = 0;
            int dividedIndex = 1;

            while (dividedIndex < segmentPointCount)
            {
                double currentGoal = (double)morphingPoints[dividedIndex - 1].X / morphingPoints[morphingPoints.Length - 1].X * curveLength;

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
                    points[dividedIndex] = c;

                    double leftAngle = Math.Atan2(c.Y - a.Y, c.X - a.X);
                    double rightAngle = Math.Atan2(b.Y - c.Y, b.X - c.X);
                    double angle = (leftAngle + rightAngle) / 2 - 1.5707963267948966192313216916398; // 90 degrees in radians
                    angles[dividedIndex] = angle;

                    dividedIndex++;
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
