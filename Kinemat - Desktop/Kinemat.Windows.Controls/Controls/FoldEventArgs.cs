using System.Windows;
using Kinemat.Windows;

namespace Kinemat.Windows.Controls
{
    /// <summary>
    /// EventArgs used by the FoldActivated and FoldDeactivated events.
    /// 
    /// </summary>
    public class FoldEventArgs : ExtendedRoutedEventArgs
    {
        /// <summary>
        /// Gets the position of the fold.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The position.
        /// </value>
        public FoldPosition Position { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.FoldEventArgs"/> class.
        /// 
        /// </summary>
        public FoldEventArgs(object source, FoldPosition position, RoutedEvent routedEvent)
            : base(routedEvent, source)
        {
            this.Position = position;
        }
    }
}