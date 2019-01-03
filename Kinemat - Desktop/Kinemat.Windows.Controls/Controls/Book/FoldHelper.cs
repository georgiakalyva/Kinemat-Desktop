// Type: Telerik.Windows.Controls.Book.FoldHelper
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System.Windows;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    internal class FoldHelper
    {
        public static bool MouseWithinFoldAreas(Point mousePoint, RadBook book)
        {
            double actualWidth = book.ActualWidth;
            double num = actualWidth / 2.0;
            double actualHeight = book.ActualHeight;
            Size foldSize = book.FoldSize;
            if (mousePoint.X < num)
                return FoldHelper.MouseWithinTopLeftFoldArea(mousePoint, foldSize) || FoldHelper.MouseWithinBottomLeftFoldArea(mousePoint, foldSize, actualHeight);
            else
                return FoldHelper.MouseWithinTopRightFoldArea(mousePoint, foldSize, actualWidth) || FoldHelper.MouseWithinBottomRightFoldArea(mousePoint, foldSize, actualWidth, actualHeight);
        }

        public static bool MouseWithinFoldAreas(Point mousePoint, RadBookItem page)
        {
            Size foldSize = page.FoldSize;
            if (page.Position == PagePosition.Left)
                return FoldHelper.MouseWithinTopLeftFoldArea(mousePoint, foldSize) || FoldHelper.MouseWithinBottomLeftFoldArea(mousePoint, foldSize, page.ActualHeight);
            else
                return FoldHelper.MouseWithinTopRightFoldArea(mousePoint, foldSize, page.ActualWidth) || FoldHelper.MouseWithinBottomRightFoldArea(mousePoint, foldSize, page.ActualWidth, page.ActualHeight);
        }

        public static bool MouseWithinTopLeftFoldArea(Point mousePoint, Size foldSize)
        {
            return mousePoint.X <= foldSize.Width && mousePoint.Y <= foldSize.Height;
        }

        public static bool MouseWithinBottomLeftFoldArea(Point mousePoint, Size foldSize, double totalHeight)
        {
            return mousePoint.X <= foldSize.Width && mousePoint.Y >= totalHeight - foldSize.Height;
        }

        public static bool MouseWithinTopRightFoldArea(Point mouse, Size foldSize, double totalWidth)
        {
            return mouse.X >= totalWidth - foldSize.Width && mouse.Y <= foldSize.Height;
        }

        public static bool MouseWithinBottomRightFoldArea(Point mouse, Size foldSize, double totalWidth, double totalHeight)
        {
            return mouse.X >= totalWidth - foldSize.Width && mouse.Y >= totalHeight - foldSize.Height;
        }

        public static FoldPosition GetQuadrant(Point mousePoint, RadBookItem page)
        {
            double num = page.ActualHeight / 2.0;
            if (page.Position == PagePosition.Left && mousePoint.Y < num)
                return FoldPosition.TopLeft;
            if (page.Position == PagePosition.Left && mousePoint.Y > num)
                return FoldPosition.BottomLeft;
            return page.Position == PagePosition.Right && mousePoint.Y < num ? FoldPosition.TopRight : FoldPosition.BottomRight;
        }

        public static FoldPosition GetQuadrant(Point mousePoint, RadBook book)
        {
            double actualWidth = book.ActualWidth;
            double actualHeight = book.ActualHeight;
            double num1 = actualWidth / 2.0;
            double num2 = book.ActualHeight / 2.0;
            if (mousePoint.X < num1 && mousePoint.Y < num2)
                return FoldPosition.TopLeft;
            if (mousePoint.X < num1 && mousePoint.Y >= num2)
                return FoldPosition.BottomLeft;
            return mousePoint.X >= num1 && mousePoint.Y < num2 ? FoldPosition.TopRight : FoldPosition.BottomRight;
        }
    }
}