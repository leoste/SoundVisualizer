﻿using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatVideoMaker
{
    class SvgConverter
    {
        public static GraphicsPath ToGraphicsPath(SvgDocument document)
        {
            GraphicsPath path;

            RectangleF rect = document.Path.GetBounds();

            using (GraphicsPathIterator iterator = new GraphicsPathIterator(document.Path))
            {
                iterator.NextSubpath(out int startIndex, out int endIndex, out bool isClosed);

                path = new GraphicsPath(
                    document.Path.PathPoints.Take(endIndex).ToArray(),
                    document.Path.PathTypes.Take(endIndex).ToArray(),
                    document.Path.FillMode);

                path.Transform(new Matrix(rect, new PointF[] {
                    new PointF(0, 0),
                    new PointF(rect.Width, 0),
                    new PointF(0, rect.Height)
                }));
            }

            return path;
        }
    }
}
