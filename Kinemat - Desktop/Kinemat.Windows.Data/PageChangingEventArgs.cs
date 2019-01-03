using System.ComponentModel;

namespace Kinemat.Windows.Data
{
    /// <summary>
    /// Provides data for notifications when the page index is changing.
    /// </summary>
    public sealed class PageChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the index of the requested page.
        /// </summary>
        /// <returns>
        /// The index of the requested page.
        /// </returns>
        public int NewPageIndex { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kinemat.Windows.Data.PageChangingEventArgs"/> class.
        /// </summary>
        /// <param name="newPageIndex">The index of the requested page.</param>
        public PageChangingEventArgs(int newPageIndex)
        {
            this.NewPageIndex = newPageIndex;
        }
    }
}
