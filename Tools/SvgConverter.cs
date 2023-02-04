using Svg;
using System.Drawing.Drawing2D;

namespace Tools
{
    public class SvgConverter
    {
        public static GraphicsPath ToGraphicsPath(SvgDocument document)
        {
            GraphicsPath path;

            RectangleF rect = document.Path.GetBounds();

            using (GraphicsPathIterator iterator = new GraphicsPathIterator(document.Path))
            {
                iterator.NextSubpath(out int startIndex, out int endIndex, out bool isClosed);
                int takeCount = endIndex + 1;

                PointF[] newPathPoints = document.Path.PathPoints.Take(takeCount).ToArray();
                byte[] newPathTypes = document.Path.PathTypes.Take(takeCount).ToArray();

                path = new GraphicsPath(newPathPoints, newPathTypes, document.Path.FillMode);

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
