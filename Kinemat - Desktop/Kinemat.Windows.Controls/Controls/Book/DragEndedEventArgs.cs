using System;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    internal class DragEndedEventArgs : EventArgs
    {
        public bool DragEndedOutsideOfPage { get; set; }

        public FoldPosition TargetCorner { get; set; }

        public DragEndedEventArgs(bool dragEndedOutsidePage, FoldPosition targetCorner)
        {
            this.DragEndedOutsideOfPage = dragEndedOutsidePage;
            this.TargetCorner = targetCorner;
        }
    }
}