using System.Windows;
using System.Windows.Media;

namespace Kinemat.Windows.Controls.Book
{
    internal class ClipCalculator
    {
        public static PathGeometry GetLeftSideForePageClip(Point hoverPoint, double pageWidth, double pageHeight)
        {
            double x = hoverPoint.X;
            double y = hoverPoint.Y;
            double curlX = CurlCalculator.GetCurlX(x, y);
            double curlY = CurlCalculator.GetCurlY(x, y);
            if (hoverPoint.Y >= 0.0)
            {
                return GeometryBuilder.BuildPathGeometry(new Point(curlX, 0.0), new Point(pageWidth, 0.0), new Point(pageWidth, pageHeight), new Point(0.0, pageHeight), new Point(0.0, curlY));
            }
            else
            {
                double num = -curlY;
                if (num < pageHeight)
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, 0.0), new Point(pageWidth, 0.0), new Point(pageWidth, pageHeight), new Point(curlX, pageHeight), new Point(0.0, pageHeight - num));
                else
                    return GeometryBuilder.BuildPathGeometry(new Point(pageWidth, 0.0), new Point(curlX * (num - pageHeight) / num, 0.0), new Point(curlX, pageHeight), new Point(pageWidth, pageHeight));
            }
        }

        public static PathGeometry GetLeftSideBackPageClip(Point hoverPoint, double pageWidth, double pageHeight)
        {
            double x = hoverPoint.X;
            double y = hoverPoint.Y;
            double curlX = CurlCalculator.GetCurlX(x, y);
            double curlY = CurlCalculator.GetCurlY(x, y);
            if (hoverPoint.Y >= 0.0)
            {
                if (curlY < pageHeight)
                    return GeometryBuilder.BuildPathGeometry(new Point(pageWidth, 0.0), new Point(pageWidth - curlX, 0.0), new Point(pageWidth, curlY));
                else
                    return GeometryBuilder.BuildPathGeometry(new Point(pageWidth - curlX, 0.0), new Point(pageWidth, 0.0), new Point(pageWidth, pageHeight), new Point(pageWidth - curlX * (curlY - pageHeight) / curlY, pageHeight));
            }
            else
            {
                double num = -curlY;
                if (num < pageHeight)
                    return GeometryBuilder.BuildPathGeometry(new Point(pageWidth, pageHeight - num), new Point(pageWidth, pageHeight), new Point(pageWidth - curlX, pageHeight));
                else
                    return GeometryBuilder.BuildPathGeometry(new Point(pageWidth, pageHeight), new Point(pageWidth - curlX, pageHeight), new Point(pageWidth - curlX * (num - pageHeight) / num, 0.0), new Point(pageWidth, 0.0));
            }
        }

        public static PathGeometry GetRightSideForePageClip(Point hoverPoint, double pageWidth, double pageHeight)
        {
            double x = pageWidth - hoverPoint.X;
            double y = hoverPoint.Y;
            double curlX = CurlCalculator.GetCurlX(x, y);
            double curlY = CurlCalculator.GetCurlY(x, y);
            if (hoverPoint.Y >= 0.0)
            {
                return GeometryBuilder.BuildPathGeometry(new Point(0.0, 0.0), new Point(pageWidth - curlX, 0.0), new Point(pageWidth, curlY), new Point(pageWidth, pageHeight), new Point(0.0, pageHeight));
            }
            else
            {
                double num = -curlY;
                if (num < pageHeight)
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, 0.0), new Point(pageWidth, 0.0), new Point(pageWidth, pageHeight - num), new Point(pageWidth - curlX, pageHeight), new Point(0.0, pageHeight));
                else
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, 0.0), new Point(pageWidth - curlX * (num - pageHeight) / num, 0.0), new Point(pageWidth - curlX, pageHeight), new Point(0.0, pageHeight));
            }
        }

        public static PathGeometry GetRightSideBackPageClip(Point hoverPoint, double pageWidth, double pageHeight)
        {
            double x = pageWidth - hoverPoint.X;
            double y = hoverPoint.Y;
            double curlX = CurlCalculator.GetCurlX(x, y);
            double curlY = CurlCalculator.GetCurlY(x, y);
            if (hoverPoint.Y >= 0.0)
            {
                if (curlY < pageHeight)
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, 0.0), new Point(curlX, 0.0), new Point(0.0, curlY));
                else
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, 0.0), new Point(curlX, 0.0), new Point(curlX * (curlY - pageHeight) / curlY, pageHeight), new Point(0.0, pageHeight));
            }
            else
            {
                double num = -curlY;
                if (num < pageHeight)
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, pageHeight - num), new Point(0.0, pageHeight), new Point(curlX, pageHeight));
                else
                    return GeometryBuilder.BuildPathGeometry(new Point(0.0, pageHeight), new Point(curlX, pageHeight), new Point(curlX * (num - pageHeight) / num, 0.0), new Point(0.0, 0.0));
            }
        }

        public static RectangleGeometry GetUnderPageClip(double pageWidth, double pageHeight)
        {
            return new RectangleGeometry()
            {
                Rect = new Rect(0.0, 0.0, pageWidth, pageHeight)
            };
        }
    }
}