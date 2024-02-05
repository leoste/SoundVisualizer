using Svg;
using System.Drawing.Drawing2D;

namespace Tools
{
    public class SvgConverter
    {
        public static GraphicsPath ToGraphicsPath(SvgDocument document)
        {
            GraphicsPath mainPath = new GraphicsPath();

            RectangleF rect = document.Path.GetBounds();

            using (GraphicsPathIterator iterator = new GraphicsPathIterator(document.Path))
            {
                for (int i = 0; i < iterator.SubpathCount - 2; i++)
                {                    
                    iterator.NextSubpath(out int startIndex, out int endIndex, out bool isClosed);
                    int takeCount = endIndex - startIndex + 1;

                    PointF[] newPathPoints = document.Path.PathPoints.Skip(startIndex).Take(takeCount).ToArray();
                    byte[] newPathTypes = document.Path.PathTypes.Skip(startIndex).Take(takeCount).ToArray();

                    GraphicsPath path = new GraphicsPath(newPathPoints.ToArray(), newPathTypes.ToArray(), FillMode.Winding);

                    path.Transform(new Matrix(rect, new PointF[] {
                        new PointF(0, 0),
                        new PointF(rect.Width, 0),
                        new PointF(0, rect.Height)
                    }));

                    mainPath.AddPath(path, false);
                }
            }

            return mainPath;
        }
    }
}
