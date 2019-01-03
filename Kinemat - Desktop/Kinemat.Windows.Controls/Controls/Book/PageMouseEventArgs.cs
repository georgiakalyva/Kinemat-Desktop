// Type: Telerik.Windows.Controls.Book.PageMouseEventArgs
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// Event arguments for Page_MouseEnter and Page_MouseLeave events.
    /// 
    /// </summary>
    public class PageMouseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the page over which mouse actions are being performed.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The page.
        /// </value>
        public RadBookItem Page { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.Book.PageMouseEventArgs"/> class.
        /// 
        /// </summary>
        /// <param name="page">The page.</param>
        public PageMouseEventArgs(RadBookItem page)
        {
            this.Page = page;
        }
    }
}