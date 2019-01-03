using System;
using System.Windows;
using System.Windows.Media;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    internal class TransformCalculator
    {
        public static RotateTransform GetRotateTransform(Point hoverPoint, double pageWidth, double pageHeight, FoldPosition draggedCorner, PagePosition position)
        {
            RotateTransform rotateTransform = new RotateTransform();
            double num1 = draggedCorner == FoldPosition.TopRight || draggedCorner == FoldPosition.BottomRight ? pageWidth - hoverPoint.X : hoverPoint.X;
            double y = hoverPoint.Y;
            double curlY = CurlCalculator.GetCurlY(num1, y);
            if (position == PagePosition.Left)
            {
                double num2 = 0.0;
                double num3 = hoverPoint.Y > 0.0 ? 0.0 : pageHeight;
                double d = Math.Atan2(num1, curlY - y) * 180.0 / Math.PI;
                if (double.IsNaN(d) || double.IsInfinity(d) || d < 0.0)
                    d = 0.0;
                rotateTransform.CenterY = num3;
                rotateTransform.CenterX = num2;
                rotateTransform.Angle = hoverPoint.Y >= 0.0 ? -d : 180.0 - d;
            }
            else
            {
                double num2 = pageWidth;
                double num3 = hoverPoint.Y > 0.0 ? 0.0 : pageHeight;
                double d = Math.Atan2(num1, curlY - y) * 180.0 / Math.PI;
                if (double.IsNaN(d) || double.IsInfinity(d) || d < 0.0)
                    d = 0.0;
                rotateTransform.CenterY = num3;
                rotateTransform.CenterX = num2;
                rotateTransform.Angle = hoverPoint.Y >= 0.0 ? d : d - 180.0;
            }
            return rotateTransform;
        }

        public static TranslateTransform GetTranslateTransform(Point hoverPoint, double pageWidth, FoldPosition draggedCorner)
        {
            return new TranslateTransform()
            {
                X = draggedCorner == FoldPosition.TopLeft || draggedCorner == FoldPosition.BottomLeft ? -2.0 * pageWidth + hoverPoint.X : hoverPoint.X + pageWidth,
                Y = hoverPoint.Y
            };
        }
    }
}
