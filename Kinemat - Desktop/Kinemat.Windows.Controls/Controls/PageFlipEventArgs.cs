// Type: Telerik.Windows.Controls.PageFlipEventArgs
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System.Windows;
using Kinemat.Windows;

namespace Kinemat.Windows.Controls
{
    /// <summary>
    /// EventArgs used by the PreviewPageFlipStarted, PageFlipStarted and PageFlipEnded events.
    /// 
    /// </summary>
    public class PageFlipEventArgs : ExtendedRoutedEventArgs
    {
        /// <summary>
        /// Gets the page being flipped.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The page.
        /// </value>
        public RadBookItem Page { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.PageFlipEventArgs"/> class.
        /// 
        /// </summary>
        /// <param name="source">The source.</param><param name="page">The page.</param><param name="routedEvent">The routed event.</param>
        public PageFlipEventArgs(object source, RadBookItem page, RoutedEvent routedEvent)
            : base(routedEvent, source)
        {
            this.Page = page;
        }
    }
}