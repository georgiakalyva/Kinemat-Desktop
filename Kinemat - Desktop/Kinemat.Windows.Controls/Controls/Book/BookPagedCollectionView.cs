using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using Kinemat.Windows;
using Kinemat.Windows.Controls;
using Kinemat.Windows.Data;

namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// Provides paging functionality for RadBook.
    /// 
    /// </summary>
    public class BookPagedCollectionView : IPagedCollectionView, IEnumerable, INotifyPropertyChanged
    {
        private RadBook owner;
        private EventHandler<EventArgs> pageChanged;
        private PropertyChangedEventHandler propertyChanged;
        private EventHandler<PageChangingEventArgs> pageChanging;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="P:Telerik.Windows.Data.IPagedCollectionView.PageIndex"/> value is allowed to change.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// true if the <see cref="P:Telerik.Windows.Data.IPagedCollectionView.PageIndex"/> value is allowed to change; otherwise, false.
        /// </returns>
        public bool CanChangePage
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether a page index change is in process.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// true if the page index is changing; otherwise, false.
        /// </returns>
        public bool IsPageChanging
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the minimum number of items known to be in the source collection.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The minimum number of items known to be in the source collection.
        /// </returns>
        public int ItemCount
        {
            get
            {
                return this.owner.FirstPagePosition != PagePosition.Left ? this.owner.Items.Count / 2 + 1 : (this.owner.Items.Count % 2 == 0 ? this.owner.Items.Count / 2 : this.owner.Items.Count / 2 + 1);
            }
        }

        /// <summary>
        /// Gets the zero-based index of the current page.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The zero-based index of the current page.
        /// </returns>
        public int PageIndex
        {
            get
            {
                return RadBook.ConvertPageToSheetIndex(this.owner.RightPageIndex, this.owner.FirstPagePosition);
            }
        }

        /// <summary>
        /// Gets or sets the number of items to display on a page.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The number of items to display on a page.
        /// </returns>
        public int PageSize
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the total number of items in the source collection.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The total number of items in the source collection, or -1 if the total number is unknown.
        /// </returns>
        public int TotalItemCount
        {
            get
            {
                return this.ItemCount;
            }
        }

        /// <summary>
        /// Occurs when page has changed.
        /// 
        /// </summary>
        public event EventHandler<EventArgs> PageChanged
        {
            add
            {
                EventHandler<EventArgs> eventHandler = this.pageChanged;
                EventHandler<EventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.pageChanged, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler = this.pageChanged;
                EventHandler<EventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.pageChanged, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        /// <summary>
        /// Occurs when a property has changed value.
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                PropertyChangedEventHandler changedEventHandler = this.propertyChanged;
                PropertyChangedEventHandler comparand;
                do
                {
                    comparand = changedEventHandler;
                    changedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.propertyChanged, comparand + value, comparand);
                }
                while (changedEventHandler != comparand);
            }
            remove
            {
                PropertyChangedEventHandler changedEventHandler = this.propertyChanged;
                PropertyChangedEventHandler comparand;
                do
                {
                    comparand = changedEventHandler;
                    changedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.propertyChanged, comparand - value, comparand);
                }
                while (changedEventHandler != comparand);
            }
        }

        /// <summary>
        /// Occurs when a page is changing.
        /// 
        /// </summary>
        public event EventHandler<PageChangingEventArgs> PageChanging
        {
            add
            {
                EventHandler<PageChangingEventArgs> eventHandler = this.pageChanging;
                EventHandler<PageChangingEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageChangingEventArgs>>(ref this.pageChanging, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<PageChangingEventArgs> eventHandler = this.pageChanging;
                EventHandler<PageChangingEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageChangingEventArgs>>(ref this.pageChanging, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.Book.BookPagedCollectionView"/> class.
        /// 
        /// </summary>
        /// <param name="owner">The owner.</param>
        public BookPagedCollectionView(RadBook owner)
        {
            this.owner = owner;
            this.owner.PageChanged += new ExtendedRoutedEventHandler(this.Book_PageChanged);
            this.owner.ItemsCountChanged += new EventHandler<EventArgs>(this.Book_ItemsCountChanged);
        }

        /// <summary>
        /// Sets the first page as the current page.
        /// 
        /// </summary>
        public bool MoveToFirstPage()
        {
            this.owner.FirstPage();
            return true;
        }

        /// <summary>
        /// Sets the last page as the current page.
        /// 
        /// </summary>
        public bool MoveToLastPage()
        {
            this.owner.LastPage();
            return true;
        }

        /// <summary>
        /// Moves to the page after the current page.
        /// 
        /// </summary>
        public bool MoveToNextPage()
        {
            this.owner.NextPage();
            return true;
        }

        /// <summary>
        /// Requests a page move to the page at the specified index.
        /// 
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to.</param>
        public bool MoveToPage(int pageIndex)
        {
            int rightPageIndex = this.owner.FirstPagePosition != PagePosition.Left ? pageIndex * 2 : pageIndex * 2 + 1;
            this.owner.RightPageIndex = rightPageIndex;
            this.OnPageChanging(RadBook.ConvertPageToSheetIndex(rightPageIndex, this.owner.FirstPagePosition));
            return true;
        }

        /// <summary>
        /// Moves to the page before the current page.
        /// 
        /// </summary>
        public bool MoveToPreviousPage()
        {
            this.owner.PreviousPage();
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// 
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
            //return this.owner.Items.GetEnumerator();
        }

        private void Book_PageChanged(object sender, ExtendedRoutedEventArgs e)
        {
            this.OnPageChanged();
            this.OnPropertyChanged("PageIndex");
        }

        private void Book_ItemsCountChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged("ItemCount");
        }

        private void OnPageChanged()
        {
            if (this.pageChanged == null)
                return;
            this.pageChanged((object)this, EventArgs.Empty);
        }

        private void OnPageChanging(int newPageIndex)
        {
            if (this.pageChanging == null)
                return;
            this.pageChanging((object)this, new PageChangingEventArgs(newPageIndex));
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.propertyChanged == null)
                return;
            this.propertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
