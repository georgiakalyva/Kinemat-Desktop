using System;

namespace Kinemat.Windows.Data
{
    /// <summary>
    /// Provides paging functionality for a collection view.
    /// </summary>
    public interface IPagedCollectionView
    {
        /// <summary>
        /// Gets a value that indicates whether the <see cref="P:Telerik.Windows.Data.IPagedCollectionView.PageIndex"/> value is allowed to change.
        /// </summary>
        /// 
        /// <returns>
        /// true if the <see cref="P:Telerik.Windows.Data.IPagedCollectionView.PageIndex"/> value is allowed to change; otherwise, false.
        /// </returns>
        bool CanChangePage { get; }

        /// <summary>
        /// Gets a value that indicates whether a page index change is in process.
        /// </summary>
        /// 
        /// <returns>
        /// true if the page index is changing; otherwise, false.
        /// </returns>
        bool IsPageChanging { get; }

        /// <summary>
        /// Gets the minimum number of items known to be in the source collection.
        /// </summary>
        /// 
        /// <returns>
        /// The minimum number of items known to be in the source collection.
        /// </returns>
        int ItemCount { get; }

        /// <summary>
        /// Gets the zero-based index of the current page.
        /// </summary>
        /// 
        /// <returns>
        /// The zero-based index of the current page.
        /// </returns>
        int PageIndex { get; }

        /// <summary>
        /// Gets or sets the number of items to display on a page.
        /// </summary>
        /// 
        /// <returns>
        /// The number of items to display on a page.
        /// </returns>
        int PageSize { get; set; }

        /// <summary>
        /// Gets the total number of items in the source collection.
        /// </summary>
        /// 
        /// <returns>
        /// The total number of items in the source collection, or -1 if the total number is unknown.
        /// </returns>
        int TotalItemCount { get; }

        /// <summary>
        /// Occurs when the <see cref="P:Telerik.Windows.Data.IPagedCollectionView.PageIndex"/> has changed.
        /// </summary>
        event EventHandler<EventArgs> PageChanged;

        /// <summary>
        /// Occurs when the <see cref="P:Telerik.Windows.Data.IPagedCollectionView.PageIndex"/> is changing.
        /// </summary>
        event EventHandler<PageChangingEventArgs> PageChanging;

        /// <summary>
        /// Sets the first page as the current page.
        /// </summary>
        /// 
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        bool MoveToFirstPage();

        /// <summary>
        /// Sets the last page as the current page.
        /// </summary>
        /// 
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        bool MoveToLastPage();

        /// <summary>
        /// Moves to the page after the current page.
        /// </summary>
        /// 
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        bool MoveToNextPage();

        /// <summary>
        /// Requests a page move to the page at the specified index.
        /// </summary>
        /// 
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        /// <param name="pageIndex">The index of the page to move to.</param>
        bool MoveToPage(int pageIndex);

        /// <summary>
        /// Moves to the page before the current page.
        /// </summary>
        /// 
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        bool MoveToPreviousPage();
    }
}
