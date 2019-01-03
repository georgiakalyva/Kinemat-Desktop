using System;
using System.Windows;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// Event args for the drag events in RadBook.
    /// 
    /// </summary>
    public class DragStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the dragged corner.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The dragged corner.
        /// </value>
        public FoldPosition DraggedCorner { get; set; }

        /// <summary>
        /// Gets or sets the mouse point.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The mouse point.
        /// </value>
        public Point MousePoint { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.Book.DragStartedEventArgs"/> class.
        /// 
        /// </summary>
        /// <param name="draggedCorner">The dragged corner.</param><param name="mousePosition">The mouse position.</param>
        public DragStartedEventArgs(FoldPosition draggedCorner, Point mousePosition)
        {
            this.DraggedCorner = draggedCorner;
            this.MousePoint = mousePosition;
        }
    }
}