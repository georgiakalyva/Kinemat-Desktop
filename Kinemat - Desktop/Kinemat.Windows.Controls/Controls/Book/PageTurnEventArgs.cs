// Type: Telerik.Windows.Controls.Book.PageTurnEventArgs
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System;
using System.Windows;

namespace Kinemat.Windows.Controls.Book
{
    internal class PageTurnEventArgs : EventArgs
    {
        public Point HoverPoint { get; set; }

        public PageTurnEventArgs(Point hoverPoint)
        {
            this.HoverPoint = hoverPoint;
        }
    }
}