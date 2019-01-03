// Type: Telerik.Windows.Controls.Book.GeometryBuilder
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System.Windows;
using System.Windows.Media;

namespace Kinemat.Windows.Controls.Book
{
    internal class GeometryBuilder
    {
        public static PathGeometry BuildPathGeometry(params Point[] geometryPoints)
        {
            PolyLineSegment polyLineSegment = new PolyLineSegment();
            foreach (Point point in geometryPoints)
                polyLineSegment.Points.Add(point);
            return new PathGeometry()
            {
                Figures = {
          new PathFigure()
          {
            IsClosed = true,
            StartPoint = geometryPoints[0],
            Segments = {
              (PathSegment) polyLineSegment
            }
          }
        }
            };
        }
    }
}