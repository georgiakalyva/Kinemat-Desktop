using System;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    internal class MouseClickEventArgs : EventArgs
    {
        public FoldPosition TargetCorner { get; set; }

        public MouseClickEventArgs(FoldPosition targetCorner)
        {
            this.TargetCorner = targetCorner;
        }
    }
}