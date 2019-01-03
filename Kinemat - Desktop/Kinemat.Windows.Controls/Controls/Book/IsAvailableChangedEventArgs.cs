using System;

namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// Event args used when the PageTurner changes its state from available to unavailable and vice versa.
    /// </summary>
    internal class IsAvailableChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the PageTurner is available.
        /// 
        /// </summary>
        public bool IsAvailable { get; set; }

        public IsAvailableChangedEventArgs(bool isAvailable)
        {
            this.IsAvailable = isAvailable;
        }
    }
}