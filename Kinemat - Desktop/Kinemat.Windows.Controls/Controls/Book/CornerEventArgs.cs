using System;
using System.Windows;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    internal class CornerEventArgs : EventArgs
    {
        public Point MousePoint { get; set; }

        public FoldPosition Corner { get; set; }

        public CornerEventArgs(Point mousePoint, FoldPosition corner)
        {
            this.MousePoint = mousePoint;
            this.Corner = corner;
        }
    }
}
