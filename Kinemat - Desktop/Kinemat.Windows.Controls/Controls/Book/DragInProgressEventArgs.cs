// Type: Telerik.Windows.Controls.Book.DragInProgressEventArgs
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System;
using System.Windows;

namespace Kinemat.Windows.Controls.Book
{
    internal class DragInProgressEventArgs : EventArgs
    {
        public Point MousePoint { get; set; }

        public DragInProgressEventArgs(Point mousePosition)
        {
            this.MousePoint = mousePosition;
        }
    }
}
