// Type: Telerik.Windows.Controls.RadBook
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Kinemat.Windows;
//using Kinemat.Windows.Automation.Peers;
using Kinemat.Windows.Controls.Book;
using Kinemat.Windows.Data;

namespace Kinemat.Windows.Controls
{
    /// <summary>
    /// RadBook is an ItemsControl that displays its items in the fashion of a regular book.
    /// </summary>
    [DefaultProperty("Items")]
    [DefaultEvent("PageChanged")]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(RadBookItem))]
    public class RadBook : ItemsControl
    {
        /// <summary>
        /// Identifies the IsVirtualizing dependency property. This property tells RadBook whether to reuse its item containers or not.
        /// 
        /// </summary>
        public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.Register("IsVirtualizing", typeof(bool), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the DefaultActiveFoldPosition property.
        /// 
        /// </summary>
        public static readonly DependencyProperty FoldHintPositionProperty = DependencyProperty.Register("FoldHintPosition", typeof(FoldHintPosition), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the RightPageIndex property.
        /// 
        /// </summary>
        public static readonly DependencyProperty RightPageIndexProperty = DependencyProperty.Register("RightPageIndex", typeof(int), typeof(RadBook), new PropertyMetadata(new PropertyChangedCallback(RadBook.OnRightPageIndexPropertyChanged)));
        /// <summary>
        /// DependencyProperty for the FlipDuration property.
        /// 
        /// </summary>
        public static readonly DependencyProperty FlipDurationProperty = DependencyProperty.Register("FlipDuration", typeof(TimeSpan), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the IsKeyboardNavigationEnabled property.
        /// 
        /// </summary>
        public static readonly DependencyProperty IsKeyboardNavigationEnabledProperty = DependencyProperty.Register("IsKeyboardNavigationEnabled", typeof(bool), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the LeftPageTemplate property.
        /// 
        /// </summary>
        public static readonly DependencyProperty LeftPageTemplateProperty = DependencyProperty.Register("LeftPageTemplate", typeof(DataTemplate), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the LeftPageTemplateSelector property.
        /// 
        /// </summary>
        public static readonly DependencyProperty LeftPageTemplateSelectorProperty = DependencyProperty.Register("LeftPageTemplateSelector", typeof(DataTemplateSelector), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for theRightPageTemplateSelector property.
        /// 
        /// </summary>
        public static readonly DependencyProperty RightPageTemplateSelectorProperty = DependencyProperty.Register("RightPageTemplateSelector", typeof(DataTemplateSelector), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the RightPageTemplate property.
        /// 
        /// </summary>
        public static readonly DependencyProperty RightPageTemplateProperty = DependencyProperty.Register("RightPageTemplate", typeof(DataTemplate), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the FirstPagePosition property.
        /// 
        /// </summary>
        public static readonly DependencyProperty FirstPagePositionProperty = DependencyProperty.Register("FirstPagePosition", typeof(PagePosition), typeof(RadBook), new PropertyMetadata(new PropertyChangedCallback(RadBook.OnFirstPagePositionPropertyChanged)));
        /// <summary>
        /// DependencyProperty for the FoldSize property.
        /// 
        /// </summary>
        public static readonly DependencyProperty FoldSizeProperty = DependencyProperty.Register("FoldSize", typeof(Size), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the PageFlipMode property.
        /// 
        /// </summary>
        public static readonly DependencyProperty PageFlipModeProperty = DependencyProperty.Register("PageFlipMode", typeof(PageFlipMode), typeof(RadBook), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the ShowPageFold property.
        /// 
        /// </summary>
        public static readonly DependencyProperty ShowPageFoldProperty = DependencyProperty.Register("ShowPageFold", typeof(PageFoldVisibility), typeof(RadBook), new PropertyMetadata(new PropertyChangedCallback(RadBook.OnShowPageFoldPropertyChanged)));
        /// <summary>
        /// Identifies the HardPages dependency property.
        /// 
        /// </summary>
        public static readonly DependencyProperty HardPagesProperty = DependencyProperty.Register("HardPages", typeof(HardPages), typeof(RadBook), new PropertyMetadata(new PropertyChangedCallback(RadBook.OnHardPagesPropertyChanged)));
        /// <summary>
        /// RoutedEvent for the PreviewPageFlipStarted event.
        /// 
        /// </summary>
        public static readonly RoutedEvent PreviewPageFlipStartedEvent = EventManager.RegisterRoutedEvent("PreviewPageFlipStarted", RoutingStrategy.Bubble, typeof(EventHandler<PageFlipEventArgs>), typeof(RadBook));
        /// <summary>
        /// RoutedEvent for the PreviewPageFlipStarted event.
        /// 
        /// </summary>
        public static readonly RoutedEvent PageFlipStartedEvent = EventManager.RegisterRoutedEvent("PageFlipStarted", RoutingStrategy.Bubble, typeof(EventHandler<PageFlipEventArgs>), typeof(RadBook));
        /// <summary>
        /// RoutedEvent for the PageFlipEnded event.
        /// 
        /// </summary>
        public static readonly RoutedEvent PageFlipEndedEvent = EventManager.RegisterRoutedEvent("PageFlipEnded", RoutingStrategy.Bubble, typeof(EventHandler<PageFlipEventArgs>), typeof(RadBook));
        /// <summary>
        /// RoutedEvent for the FoldActivated event.
        /// 
        /// </summary>
        public static readonly RoutedEvent FoldActivatedEvent = EventManager.RegisterRoutedEvent("FoldActivated", RoutingStrategy.Bubble, typeof(EventHandler<FoldEventArgs>), typeof(RadBook));
        /// <summary>
        /// RoutedEvent for the FoldDeactivated event.
        /// 
        /// </summary>
        public static readonly RoutedEvent FoldDeactivatedEvent = EventManager.RegisterRoutedEvent("FoldDeactivated", RoutingStrategy.Bubble, typeof(EventHandler<FoldEventArgs>), typeof(RadBook));
        /// <summary>
        /// RoutedEvent for the PageChanged event.
        /// 
        /// </summary>
        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent("PageChanged", RoutingStrategy.Bubble, typeof(ExtendedRoutedEventHandler), typeof(RadBook));
        private PageTurner softPageTurner;
        private PageTurner sheetPageTurner;
        private PageTurner hardPageTurner;
        private Panel itemsPanel;
        private FoldPosition currentFold;
        private bool mouseTurn;
        private bool updateRightPageIndex;
        private int newSheetIndex;
        private int oldSheetIndex;
        private int buildSheetIndex;
        private bool rebuildPages;
        private int currentSheetIndex;
        private bool raisePageChanged;
        private bool raiseFoldDeactivated;
        private bool raisePageFlipEnded;
        private bool isPreviewPageFlipStartedHandled;
        private RadBookItem pageForPageFlipEndedEvent;
        private bool isDraggingSoftPaper;
        private bool buildPagesWithoutAnimation;
        private IPagedCollectionView pagedItems;
        private bool isAutoCornerVisible;
        private bool rebuildAfterMouseLeavesPage;
        private bool executePageMouseMove;
        private RadBookItem forePage;
        private RadBookItem backPage;
        private RadBookItem underPage;
        private RadBookItem otherPage;
        private bool isDraggingHardPaper;
        private bool bookClicked;
        private SkewTransform skewTransform;
        private ScaleTransform forePageScaleTransform;
        private ScaleTransform backPageScaleTransform;
        private bool hardPaperAnimationInProgress;
        private Point mouseDownPoint;
        private EventHandler<EventArgs> itemsCountChanged;

        /// <summary>
        /// Gets or sets the starting fold when turning the page programmatically or when ShowPageFold is set to OnPageEnter.
        /// 
        /// </summary>
        public FoldHintPosition FoldHintPosition
        {
            get
            {
                return (FoldHintPosition)this.GetValue(RadBook.FoldHintPositionProperty);
            }
            set
            {
                this.SetValue(RadBook.FoldHintPositionProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the IsVirtualizing property. This property tells RadBook whether to reuse its item containers or not.
        /// 
        /// </summary>
        public bool IsVirtualizing
        {
            get
            {
                return (bool)this.GetValue(RadBook.IsVirtualizingProperty);
            }
            set
            {
                this.SetValue(RadBook.IsVirtualizingProperty, (object)value);
                // this.SetValue(RadBook.IsVirtualizingProperty, (object)(bool)(value ? 1 : 0));
            }
        }

        /// <summary>
        /// Gets or sets the index of the right page in the book.
        /// 
        /// </summary>
        public int RightPageIndex
        {
            get
            {
                return (int)this.GetValue(RadBook.RightPageIndexProperty);
            }
            set
            {
                this.SetValue(RadBook.RightPageIndexProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the amount of time necessary to flip a page.
        /// 
        /// </summary>
        public TimeSpan FlipDuration
        {
            get
            {
                return (TimeSpan)this.GetValue(RadBook.FlipDurationProperty);
            }
            set
            {
                this.SetValue(RadBook.FlipDurationProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets whether the keyboard can be used to navigate through the pages of the book.
        /// 
        /// </summary>
        public bool IsKeyboardNavigationEnabled
        {
            get
            {
                return (bool)this.GetValue(RadBook.IsKeyboardNavigationEnabledProperty);
            }
            set
            {
                this.SetValue(RadBook.IsKeyboardNavigationEnabledProperty, (object)value);
                // this.SetValue(RadBook.IsKeyboardNavigationEnabledProperty, (object)(bool)(value ? 1 : 0));
            }
        }

        /// <summary>
        /// Gets or sets the template for a left page.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The left page template.
        /// </value>
        public DataTemplate LeftPageTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(RadBook.LeftPageTemplateProperty);
            }
            set
            {
                this.SetValue(RadBook.LeftPageTemplateProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the left page template selector.
        /// 
        /// </summary>
        public DataTemplateSelector LeftPageTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)this.GetValue(RadBook.LeftPageTemplateSelectorProperty);
            }
            set
            {
                this.SetValue(RadBook.LeftPageTemplateSelectorProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the template for a right page.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The right page template.
        /// </value>
        public DataTemplate RightPageTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(RadBook.RightPageTemplateProperty);
            }
            set
            {
                this.SetValue(RadBook.RightPageTemplateProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the right page template selector.
        /// 
        /// </summary>
        public DataTemplateSelector RightPageTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)this.GetValue(RadBook.RightPageTemplateSelectorProperty);
            }
            set
            {
                this.SetValue(RadBook.RightPageTemplateSelectorProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the position the first page - left or right, with respect to the book.
        /// 
        /// </summary>
        public PagePosition FirstPagePosition
        {
            get
            {
                return (PagePosition)this.GetValue(RadBook.FirstPagePositionProperty);
            }
            set
            {
                this.SetValue(RadBook.FirstPagePositionProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the page fold.
        /// 
        /// </summary>
        public Size FoldSize
        {
            get
            {
                return (Size)this.GetValue(RadBook.FoldSizeProperty);
            }
            set
            {
                this.SetValue(RadBook.FoldSizeProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the fashion (None, SingleClick or DoubleClick) in which the pages turn.
        /// 
        /// </summary>
        public PageFlipMode PageFlipMode
        {
            get
            {
                return (PageFlipMode)this.GetValue(RadBook.PageFlipModeProperty);
            }
            set
            {
                this.SetValue(RadBook.PageFlipModeProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets whether and when a fold will be displayed.
        /// 
        /// </summary>
        public PageFoldVisibility ShowPageFold
        {
            get
            {
                return (PageFoldVisibility)this.GetValue(RadBook.ShowPageFoldProperty);
            }
            set
            {
                this.SetValue(RadBook.ShowPageFoldProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the paged items.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The paged items.
        /// </value>
        public IPagedCollectionView PagedItems
        {
            get
            {
                if (this.pagedItems == null)
                    this.pagedItems = (IPagedCollectionView)new BookPagedCollectionView(this);
                return this.pagedItems;
            }
            set
            {
                this.pagedItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the HardPages property.
        /// 
        /// </summary>
        public HardPages HardPages
        {
            get
            {
                return (HardPages)this.GetValue(RadBook.HardPagesProperty);
            }
            set
            {
                this.SetValue(RadBook.HardPagesProperty, (object)value);
            }
        }

        internal static Size DefaultPageFoldSize
        {
            get
            {
                return new Size(50.0, 50.0);
            }
        }

        internal BookPanel BookPanel
        {
            get
            {
                return this.itemsPanel as BookPanel;
            }
        }

        internal int CurrentSheetIndex
        {
            get
            {
                return this.currentSheetIndex;
            }
            set
            {
                int oldValue = this.currentSheetIndex;
                if (this.currentSheetIndex == value)
                    return;
                this.currentSheetIndex = value;
                this.OnCurrentSheetIndexChanged(this.currentSheetIndex, oldValue);
            }
        }

        private Size PageDimensions
        {
            get
            {
                if (this.itemsPanel != null)
                    return new Size(this.itemsPanel.ActualWidth / 2.0, this.itemsPanel.ActualHeight);
                else
                    return new Size();
            }
        }

        private bool XamlInitialized { get; set; }

        private bool IsAutoCornerVisible
        {
            get
            {
                return this.isAutoCornerVisible;
            }
            set
            {
                this.isAutoCornerVisible = this.ShowPageFold == PageFoldVisibility.OnPageEnter && value;
            }
        }

        /// <summary>
        /// Occurs when mouse enters any of the four corners of the book.
        /// 
        /// </summary>
        public event EventHandler<FoldEventArgs> FoldActivated
        {
            add
            {
                this.AddHandler(RadBook.FoldActivatedEvent, (Delegate)value);
            }
            remove
            {
                this.RemoveHandler(RadBook.FoldActivatedEvent, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when mouse leaves any of the four corners of the book.
        /// 
        /// </summary>
        public event EventHandler<FoldEventArgs> FoldDeactivated
        {
            add
            {
                this.AddHandler(RadBook.FoldDeactivatedEvent, (Delegate)value);
            }
            remove
            {
                this.RemoveHandler(RadBook.FoldDeactivatedEvent, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when page is about to get dragged.
        /// 
        /// </summary>
        public event EventHandler<PageFlipEventArgs> PreviewPageFlipStarted
        {
            add
            {
                this.AddHandler(RadBook.PreviewPageFlipStartedEvent, (Delegate)value);
            }
            remove
            {
                this.RemoveHandler(RadBook.PreviewPageFlipStartedEvent, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when page is dragged.
        /// 
        /// </summary>
        public event EventHandler<PageFlipEventArgs> PageFlipStarted
        {
            add
            {
                this.AddHandler(RadBook.PageFlipStartedEvent, (Delegate)value);
            }
            remove
            {
                this.RemoveHandler(RadBook.PageFlipStartedEvent, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when page has finished dragging.
        /// 
        /// </summary>
        public event EventHandler<PageFlipEventArgs> PageFlipEnded
        {
            add
            {
                this.AddHandler(RadBook.PageFlipEndedEvent, (Delegate)value);
            }
            remove
            {
                this.RemoveHandler(RadBook.PageFlipEndedEvent, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when a full page flip occurs.
        /// 
        /// </summary>
        public event ExtendedRoutedEventHandler PageChanged
        {
            add
            {
                this.AddHandler(RadBook.PageChangedEvent, (Delegate)value);
            }
            remove
            {
                this.RemoveHandler(RadBook.PageChangedEvent, (Delegate)value);
            }
        }

        internal event EventHandler<EventArgs> ItemsCountChanged
        {
            add
            {
                EventHandler<EventArgs> eventHandler = this.itemsCountChanged;
                EventHandler<EventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.itemsCountChanged, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler = this.itemsCountChanged;
                EventHandler<EventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.itemsCountChanged, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        /// <summary>
        /// Initializes static members of the RadBook class.
        /// 
        /// </summary>
        static RadBook()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadBook), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(RadBook)));
            Control.IsTabStopProperty.OverrideMetadata(typeof(RadBook), (PropertyMetadata)new FrameworkPropertyMetadata((object)false));
            BookCommands.EnsureCommands();
            CommandManager.RegisterClassCommandBinding(typeof(RadBook), new CommandBinding((ICommand)BookCommands.FirstPage, new ExecutedRoutedEventHandler(RadBook.OnFirstPageExecuted), new CanExecuteRoutedEventHandler(RadBook.OnFirstPageCanExecute)));
            CommandManager.RegisterClassCommandBinding(typeof(RadBook), new CommandBinding((ICommand)BookCommands.PreviousPage, new ExecutedRoutedEventHandler(RadBook.OnPreviousPageExecuted), new CanExecuteRoutedEventHandler(RadBook.OnPreviousPageCanExecute)));
            CommandManager.RegisterClassCommandBinding(typeof(RadBook), new CommandBinding((ICommand)BookCommands.NextPage, new ExecutedRoutedEventHandler(RadBook.OnNextPageExecuted), new CanExecuteRoutedEventHandler(RadBook.OnNextPageCanExecute)));
            CommandManager.RegisterClassCommandBinding(typeof(RadBook), new CommandBinding((ICommand)BookCommands.LastPage, new ExecutedRoutedEventHandler(RadBook.OnLastPageExecuted), new CanExecuteRoutedEventHandler(RadBook.OnLastPageCanExecute)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.RadBook"/> class.
        /// 
        /// </summary>
        public RadBook()
        {
            this.softPageTurner = new PageTurner(this);
            RadBook radBook = this;
            RadBook book = this;
            CircleEase circleEase1 = new CircleEase();
            circleEase1.EasingMode = EasingMode.EaseInOut;
            CircleEase circleEase2 = circleEase1;
            PageTurner pageTurner = new PageTurner(book, (IEasingFunction)circleEase2);
            radBook.sheetPageTurner = pageTurner;
            this.softPageTurner.PageTurning += new EventHandler<PageTurnEventArgs>(this.PageTurner_Turning);
            this.softPageTurner.PageTurned += new EventHandler<PageTurnEventArgs>(this.PageTurner_Completed);
            this.sheetPageTurner.PageTurning += new EventHandler<PageTurnEventArgs>(this.PageTurner_Turning);
            this.sheetPageTurner.PageTurned += new EventHandler<PageTurnEventArgs>(this.PageTurner_Completed);
            this.hardPageTurner = new PageTurner(this);
            this.hardPageTurner.PageTurning += new EventHandler<PageTurnEventArgs>(this.HardPaperTurner_Turning);
            this.hardPageTurner.PageTurned += new EventHandler<PageTurnEventArgs>(this.HardPaperTurner_Completed);
            this.MouseLeave += new MouseEventHandler(this.RadBook_MouseLeave);
            this.SizeChanged += new SizeChangedEventHandler(this.RadBook_SizeChanged);
            this.MouseMove += new MouseEventHandler(this.RadBook_MouseMove);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(this.RadBook_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.RadBook_MouseLeftButtonUp);
        }

        /// <summary>
        /// Converts a page index to a sheet index.
        /// 
        /// </summary>
        /// <param name="rightPageIndex">The RightPageIndex of the book.</param><param name="firstPagePosition">The position of the first page in the book.</param>
        /// <returns/>
        public static int ConvertPageToSheetIndex(int rightPageIndex, PagePosition firstPagePosition)
        {
            if (firstPagePosition == PagePosition.Left || rightPageIndex % 2 == 0)
                return rightPageIndex / 2;
            else
                return Math.Max(0, rightPageIndex / 2 + 1);
        }

        /// <summary>
        /// Converts the index of the sheet to page.
        /// 
        /// </summary>
        /// <param name="sheetIndex">Index of the sheet.</param><param name="fistPagePosition">The fist page position of the book.</param><param name="totalItemsCount">The total number of items in the book.</param>
        /// <returns/>
        public static int ConvertSheetToPageIndex(int sheetIndex, PagePosition fistPagePosition, int totalItemsCount)
        {
            if (fistPagePosition == PagePosition.Left)
                return Math.Min(sheetIndex * 2 + 1, totalItemsCount - 1);
            else
                return Math.Min(sheetIndex * 2, totalItemsCount - 1);
        }

        /// <summary>
        /// Navigates to the first page.
        /// 
        /// </summary>
        public void FirstPage()
        {
            if (!this.CanGoToFirstPage())
                return;
            this.BuildPages();
            this.NavigateToPage(PageToNavigateTo.First);
        }

        /// <summary>
        /// Navigates to the previous page.
        /// 
        /// </summary>
        public void PreviousPage()
        {
            if (!this.CanGoToPreviousPage())
                return;
            this.BuildPages();
            this.NavigateToPage(PageToNavigateTo.Previous);
        }

        /// <summary>
        /// Navigates to the next page.
        /// 
        /// </summary>
        public void NextPage()
        {
            if (!this.CanGoToNextPage())
                return;
            this.BuildPages();
            this.NavigateToPage(PageToNavigateTo.Next);
        }

        /// <summary>
        /// Navigates to the last page.
        /// 
        /// </summary>
        public void LastPage()
        {
            if (!this.CanGoToLastPage())
                return;
            this.BuildPages();
            this.NavigateToPage(PageToNavigateTo.Last);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application
        ///             code or internal processes (such as a rebuilding layout pass) call
        ///             <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.XamlInitialized = true;
            this.itemsPanel = (Panel)null;
            base.OnApplyTemplate();
            int rightPageIndex = this.CoerceRightPageIndex(this.RightPageIndex);
            this.buildPagesWithoutAnimation = true;
            this.CurrentSheetIndex = RadBook.ConvertPageToSheetIndex(rightPageIndex, this.FirstPagePosition);
            this.buildPagesWithoutAnimation = false;
        }

        /// <summary>
        /// Resets the theme.
        /// 
        /// </summary>
        //public void ResetTheme()
        //{
        //    ThemingExtensions.SetDefaultStyleKey<RadBook>(this);
        //}

        internal void BuildPages()
        {
            this.softPageTurner.Stop();
            this.sheetPageTurner.Stop();
            this.hardPageTurner.Stop();
            if (this.itemsPanel != null)
            {
                for (int index = 0; index < this.itemsPanel.Children.Count; ++index)
                {
                    RadBookItem radBookItem = this.itemsPanel.Children[index] as RadBookItem;
                    if (!radBookItem.IsHardPaper)
                        radBookItem.IsHardPaperInternal = false;
                }
            }
            if (this.itemsPanel == null)
                return;
            for (int index = 0; index < this.itemsPanel.Children.Count; ++index)
            {
                RadBookItem page = this.itemsPanel.Children[index] as RadBookItem;
                int num = this.ItemContainerGenerator.IndexFromContainer((DependencyObject)page);
                page.RenderTransformOrigin = new Point();
                page.RenderTransform = (Transform)null;
                page.Clip = (Geometry)null;
                page.Index = num;
                page.IsFirstPage = num == 0;
                page.IsLastPage = num == this.Items.Count - 1;
                if ((this.FirstPagePosition == PagePosition.Right ? num + 1 : num) % 2 == 0)
                {
                    page.SetValue(Grid.ColumnProperty, (object)0);
                    page.Position = PagePosition.Left;
                    page.ContentTemplate = this.GetLeftPageTemplate(page);
                }
                else
                {
                    page.SetValue(Grid.ColumnProperty, (object)1);
                    page.Position = PagePosition.Right;
                    page.ContentTemplate = this.GetRightPageTemplate(page);
                }
                this.PreLoadPage(page);
                this.SetHardPages(page);
                if (page.RightBackPageShadow != null)
                    page.RightBackPageShadow.Visibility = Visibility.Collapsed;
                if (page.RightBackPageShadowStatic != null)
                {
                    if (page.Position == PagePosition.Left)
                        page.RightBackPageShadowStatic.Visibility = Visibility.Visible;
                    else
                        page.RightBackPageShadowStatic.Visibility = Visibility.Collapsed;
                }
                if (page.LeftBackPageShadow != null)
                    page.LeftBackPageShadow.Visibility = Visibility.Collapsed;
                if (page.RightUnderPageShadow != null)
                    page.RightUnderPageShadow.Visibility = Visibility.Collapsed;
                if (page.LeftUnderPageShadow != null)
                    page.LeftUnderPageShadow.Visibility = Visibility.Collapsed;
                page.CanBeTurned = (!page.IsFirstPage || page.Position != PagePosition.Left) && (!page.IsLastPage || page.Position != PagePosition.Right);
                page.IsHitTestVisible = true;
                this.currentFold = FoldPosition.None;
                page.Reset();
            }
        }

        internal bool OnKeyDownInternal(Key key)
        {
            if (!this.IsKeyboardNavigationEnabled || this.raisePageChanged || key != Key.Right && key != Key.Left && (key != Key.Home && key != Key.End))
                return false;
            if (key == Key.Right)
            {
                if (!this.CanGoToNextPage())
                    return false;
                this.NavigateToPage(PageToNavigateTo.Next);
                return true;
            }
            else if (key == Key.Left)
            {
                if (!this.CanGoToPreviousPage())
                    return false;
                this.NavigateToPage(PageToNavigateTo.Previous);
                return true;
            }
            else if (key == Key.Home)
            {
                if (!this.CanGoToFirstPage())
                    return false;
                this.NavigateToPage(PageToNavigateTo.First);
                return true;
            }
            else
            {
                if (key != Key.End || !this.CanGoToLastPage())
                    return false;
                this.NavigateToPage(PageToNavigateTo.Last);
                return true;
            }
        }

        internal void SetHardPagesInternal(RadBookItem page)
        {
            this.SetHardPages(page);
        }

        /// <summary>
        /// Called when a page is changed. Raises PageChanged event.
        /// 
        /// </summary>
        /// <param name="e"/>
        protected virtual void OnPageChanged(ExtendedRoutedEventArgs e)
        {
            this.RaiseEvent((RoutedEventArgs)e);
            this.UpdateVirtualizingPanelIndex(this.RightPageIndex);
        }

        /// <summary>
        /// Called when a page flips. Raises PageFlipped event.
        /// 
        /// </summary>
        /// <param name="e"/>
        protected virtual void OnPageFlipEnded(PageFlipEventArgs e)
        {
            this.RaiseEvent((RoutedEventArgs)e);
        }

        /// <summary>
        /// Raises the <see cref="E:PreviewPageFlipStarted"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.PageFlipEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPreviewPageFlipStarted(PageFlipEventArgs e)
        {
            this.RaiseEvent((RoutedEventArgs)e);
        }

        /// <summary>
        /// Raises the <see cref="E:PageFlipStarted"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.PageFlipEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPageFlipStarted(PageFlipEventArgs e)
        {
            this.RaiseEvent((RoutedEventArgs)e);
        }

        /// <summary>
        /// Raises the <see cref="E:FoldActivated"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.FoldEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFoldActivated(FoldEventArgs e)
        {
            if (this.ShowPageFold == PageFoldVisibility.Never)
                return;
            this.RaiseEvent((RoutedEventArgs)e);
        }

        /// <summary>
        /// Raises the <see cref="E:FoldDeactivated"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.FoldEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFoldDeactivated(FoldEventArgs e)
        {
            if (this.ShowPageFold == PageFoldVisibility.Never)
                return;
            this.RaiseEvent((RoutedEventArgs)e);
        }

        /// <summary>
        /// Override for ClearContainerForItem.
        /// 
        /// </summary>
        /// <param name="element"/><param name="item"/>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            RadBookItem radBookItem = element as RadBookItem;
            radBookItem.CancelPagesRebuild = true;
            radBookItem.IsHardPaper = false;
            radBookItem.IsHardPaperInternal = false;
            radBookItem.MouseDragStart -= new EventHandler<DragStartedEventArgs>(this.Page_MouseDragStart);
            radBookItem.MouseDragMove -= new EventHandler<DragInProgressEventArgs>(this.Page_MouseDragMove);
            radBookItem.MouseDragEnd -= new EventHandler<DragEndedEventArgs>(this.Page_MouseDragEnd);
            radBookItem.MouseSingleClick -= new EventHandler<MouseClickEventArgs>(this.Page_MouseSingleClick);
            radBookItem.MouseDoubleClick -= new EventHandler<MouseClickEventArgs>(this.Page_MouseDoubleClick);
            radBookItem.FoldMouseEnter -= new EventHandler<CornerEventArgs>(this.Fold_MouseEnter);
            radBookItem.FoldMouseMove -= new EventHandler<CornerEventArgs>(this.Fold_MouseMove);
            radBookItem.FoldMouseLeave -= new EventHandler<CornerEventArgs>(this.Fold_MouseLeave);
            radBookItem.PageMouseEnter -= new EventHandler<PageMouseEventArgs>(this.Page_PageMouseEnter);
            radBookItem.PageMouseMove -= new EventHandler<PageMouseEventArgs>(this.Page_PageMouseMove);
            radBookItem.PageMouseLeave -= new EventHandler<PageMouseEventArgs>(this.Page_PageMouseLeave);
            if (item == element)
                return;
            radBookItem.Content = (object)null;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // StyleManager.SetDefaultStyleKey((Control)this, typeof(RadBook));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.KeyDown"/> event occurs.
        /// 
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            e.Handled = this.OnKeyDownInternal(e.Key);
        }

        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> property changes.
        /// 
        /// </summary>
        /// <param name="e">A <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> that contains the event data.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            this.RaiseItemsCountChanged();
            if (this.ItemsSource == null && this.Items.Count == 0)
                return;
            if (e.Action == NotifyCollectionChangedAction.Reset && this.ItemsSource != null)
            {
                this.buildPagesWithoutAnimation = true;
                this.RightPageIndex = this.FirstPagePosition == PagePosition.Left ? 1 : 0;
                this.buildPagesWithoutAnimation = false;
            }
            if (e.Action != NotifyCollectionChangedAction.Remove)
                return;
            this.buildPagesWithoutAnimation = true;
            this.RightPageIndex = this.Items.Count <= 0 ? -1 : (this.FirstPagePosition == PagePosition.Left ? 1 : 0);
            this.buildPagesWithoutAnimation = false;
            this.BuildPages();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container.
        /// 
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is RadBookItem;
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The element that is used to display the given item.
        /// 
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return (DependencyObject)new RadBookItem();
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// 
        /// </summary>
        /// <param name="element">Element used to display the specified item.</param><param name="item">Specified item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            RadBookItem radBookItem = element as RadBookItem;
            if (this.itemsPanel == null)
            {
                this.itemsPanel = (Panel)VisualTreeHelper.GetParent((DependencyObject)radBookItem);
                this.UpdateVirtualization(this.RightPageIndex);
            }
            radBookItem.ParentBook = this;
            radBookItem.MouseDragStart += new EventHandler<DragStartedEventArgs>(this.Page_MouseDragStart);
            radBookItem.MouseDragMove += new EventHandler<DragInProgressEventArgs>(this.Page_MouseDragMove);
            radBookItem.MouseDragEnd += new EventHandler<DragEndedEventArgs>(this.Page_MouseDragEnd);
            radBookItem.MouseSingleClick += new EventHandler<MouseClickEventArgs>(this.Page_MouseSingleClick);
            radBookItem.MouseDoubleClick += new EventHandler<MouseClickEventArgs>(this.Page_MouseDoubleClick);
            radBookItem.FoldMouseEnter += new EventHandler<CornerEventArgs>(this.Fold_MouseEnter);
            radBookItem.FoldMouseMove += new EventHandler<CornerEventArgs>(this.Fold_MouseMove);
            radBookItem.FoldMouseLeave += new EventHandler<CornerEventArgs>(this.Fold_MouseLeave);
            radBookItem.PageMouseEnter += new EventHandler<PageMouseEventArgs>(this.Page_PageMouseEnter);
            radBookItem.PageMouseMove += new EventHandler<PageMouseEventArgs>(this.Page_PageMouseMove);
            radBookItem.PageMouseLeave += new EventHandler<PageMouseEventArgs>(this.Page_PageMouseLeave);
            this.BuildPages();
        }

        /// <summary>
        /// When implemented in a derived class, returns class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/>
        ///             implementations for the Silverlight automation infrastructure.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/>
        ///             subclass to return.
        /// 
        /// </returns>
        //protected override AutomationPeer OnCreateAutomationPeer()
        //{
        //    return (AutomationPeer)new Kinemat.Windows.Automation.Peers.RadBookAutomationPeer(this);
        //}

        private static void OnFirstPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            radBook.FirstPage();
        }

        private static void OnFirstPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            e.CanExecute = radBook.CanGoToFirstPage();
        }

        private static void OnPreviousPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            radBook.PreviousPage();
        }

        private static void OnPreviousPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            e.CanExecute = radBook.CanGoToPreviousPage();
        }

        private static void OnNextPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            radBook.NextPage();
        }

        private static void OnNextPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            e.CanExecute = radBook.CanGoToNextPage();
        }

        private static void OnLastPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            radBook.LastPage();
        }

        private static void OnLastPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            RadBook radBook = sender as RadBook;
            if (radBook == null)
                return;
            e.CanExecute = radBook.CanGoToLastPage();
        }

        private static void OnCurrentSheetIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadBook radBook = d as RadBook;
            if (radBook == null)
                return;
            int newValue = (int)e.NewValue;
            int oldValue = (int)e.OldValue;
            int num = radBook.Items.Count / 2 + (radBook.FirstPagePosition == PagePosition.Right ? 1 : 0);
            if (newValue < 0 || newValue >= num)
                return;
            radBook.OnCurrentSheetIndexChanged(newValue, oldValue);
        }

        private static void OnRightPageIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadBook radBook = d as RadBook;
            if (!radBook.XamlInitialized)
                return;
            int rightPageIndex = radBook.CoerceRightPageIndex((int)e.NewValue);
            radBook.OnRightPageIndexChanged(rightPageIndex);
        }

        private static void OnFirstPagePositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RadBook).BuildPages();
        }

        private static void OnShowPageFoldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadBook radBook = d as RadBook;
            if (radBook == null)
                return;
            radBook.OnShowPageFoldChanged();
        }

        private static void OnHardPagesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadBook radBook = d as RadBook;
            if (radBook == null)
                return;
            radBook.OnHardPagesChanged();
        }

        private static TransformGroup GetShadowTransform(double pageWidth, double pageHeight, PagePosition translateTransformPagePosition, PagePosition rotateTransformPagePosition, RotateTransform backPageRotateTransform)
        {
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform();
            TranslateTransform translateTransform = new TranslateTransform();
            RotateTransform rotateTransform = new RotateTransform();
            transformGroup.Children.Add((Transform)scaleTransform);
            transformGroup.Children.Add((Transform)rotateTransform);
            transformGroup.Children.Add((Transform)translateTransform);
            scaleTransform.ScaleY = 20.0;
            scaleTransform.CenterY = pageHeight / 2.0;
            rotateTransform.CenterY = backPageRotateTransform.CenterY;
            translateTransform.X = translateTransformPagePosition != PagePosition.Left ? pageWidth - CurlCalculator.CurlX : CurlCalculator.CurlX - pageWidth;
            rotateTransform.Angle = rotateTransformPagePosition != PagePosition.Left ? backPageRotateTransform.Angle / 2.0 : -backPageRotateTransform.Angle / 2.0;
            return transformGroup;
        }

        private static double GetShadowOpacity(Point hoverPoint, double pageWidth, PagePosition pagePosition)
        {
            if (pagePosition == PagePosition.Left)
                return (2.0 * pageWidth - hoverPoint.X) / (pageWidth / 4.0);
            else
                return (hoverPoint.X + pageWidth) / (pageWidth / 4.0);
        }

        private static void NullifyRenderTransform(params UIElement[] elements)
        {
            foreach (UIElement uiElement in elements)
            {
                if (uiElement != null)
                    uiElement.RenderTransform = (Transform)null;
            }
        }

        private void RadBook_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.BuildPages();
        }

        private void OnCurrentSheetIndexChanged(int newValue, int oldValue)
        {
            if (!this.mouseTurn && this.XamlInitialized && (!this.buildPagesWithoutAnimation && !this.raisePageChanged))
            {
                int index = RadBook.ConvertSheetToPageIndex(oldValue, this.FirstPagePosition, this.Items.Count);
                RadBookItem page1 = newValue <= oldValue ? this.GetPage(Math.Max(0, index - 1)) : this.GetPage(index);
                RadBookItem page2 = this.GetPage(RadBook.ConvertSheetToPageIndex(newValue, this.FirstPagePosition, this.Items.Count));
                bool flag1 = page2 != null && Math.Abs(newValue - oldValue) > 1 && page2.IsHardPaperInternal;
                bool flag2 = page1.IsHardPaper || page1.IsHardPaperInternal || flag1;
                PageFlipEventArgs e = new PageFlipEventArgs((object)this, page1, RadBook.PreviewPageFlipStartedEvent);
                this.OnPreviewPageFlipStarted(e);
                if (e.Handled)
                    return;
                this.pageForPageFlipEndedEvent = page1;
                this.OnPageFlipStarted(new PageFlipEventArgs((object)this, page1, RadBook.PageFlipStartedEvent));
                FoldPosition corner;
                FoldPosition to1;
                if (newValue > oldValue)
                {
                    corner = this.FoldHintPosition == FoldHintPosition.Top ? FoldPosition.TopRight : FoldPosition.BottomRight;
                    to1 = this.FoldHintPosition == FoldHintPosition.Top ? FoldPosition.TopLeft : FoldPosition.BottomLeft;
                }
                else
                {
                    corner = this.FoldHintPosition == FoldHintPosition.Top ? FoldPosition.TopLeft : FoldPosition.BottomLeft;
                    to1 = this.FoldHintPosition == FoldHintPosition.Top ? FoldPosition.TopRight : FoldPosition.BottomRight;
                }
                this.rebuildPages = true;
                this.raisePageFlipEnded = true;
                this.raisePageChanged = true;
                this.currentFold = corner;
                this.newSheetIndex = newValue;
                this.oldSheetIndex = oldValue;
                this.buildSheetIndex = newValue;
                this.sheetPageTurner.Stop();
                this.softPageTurner.Stop();
                this.hardPageTurner.Stop();
                if (flag2)
                {
                    this.forePage = this.GetForePage();
                    this.backPage = this.GetBackPage();
                    Point point1 = new Point(0.0, this.ActualHeight);
                    Point point2 = new Point(this.ActualWidth, this.ActualHeight);
                    Point to2 = newValue > oldValue ? point1 : point2;
                    this.hardPageTurner.CurrentMousePosition = newValue > oldValue ? point2 : point1;
                    this.currentFold = FoldHelper.GetQuadrant(this.hardPageTurner.CurrentMousePosition, this);
                    if (!this.PrepareForeAndBackPageForHardPaperTurn())
                        return;
                    this.hardPaperAnimationInProgress = true;
                    this.hardPageTurner.TurnPage(to2);
                }
                else
                {
                    Point cornerPoint = this.sheetPageTurner.GetCornerPoint(corner, RelativeTo.Page);
                    switch (corner)
                    {
                        case FoldPosition.TopLeft:
                            cornerPoint.X += this.FoldSize.Width;
                            cornerPoint.Y += this.FoldSize.Height;
                            break;
                        case FoldPosition.TopRight:
                            cornerPoint.X -= this.FoldSize.Width;
                            cornerPoint.Y += this.FoldSize.Height;
                            break;
                        case FoldPosition.BottomRight:
                            cornerPoint.X -= this.FoldSize.Width;
                            cornerPoint.Y -= this.FoldSize.Height;
                            break;
                        case FoldPosition.BottomLeft:
                            cornerPoint.X += this.FoldSize.Width;
                            cornerPoint.Y -= this.FoldSize.Height;
                            break;
                    }
                    this.sheetPageTurner.CurrentMousePosition = cornerPoint;
                    this.sheetPageTurner.TurnPage(to1, RelativeTo.Book);
                }
            }
            else if (this.mouseTurn && this.ShowPageFold == PageFoldVisibility.OnPageEnter)
            {
                this.IsAutoCornerVisible = false;
                this.BuildPages();
            }
            else
            {
                this.newSheetIndex = newValue;
                this.buildSheetIndex = newValue;
                this.BuildPages();
                this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
                this.OnPageChanged(new ExtendedRoutedEventArgs(RadBook.PageChangedEvent, (object)this));
                this.updateRightPageIndex = false;
                this.raisePageChanged = false;
                this.raiseFoldDeactivated = false;
            }
        }

        private void RadBook_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.forePage != null && this.forePage.IsHardPaperInternal || (this.isDraggingSoftPaper || this.currentFold == FoldPosition.None) || (this.rebuildPages || this.hardPaperAnimationInProgress))
                return;
            this.buildSheetIndex = this.oldSheetIndex;
            if (this.ShowPageFold == PageFoldVisibility.OnFoldEnter)
            {
                this.rebuildPages = true;
                this.raiseFoldDeactivated = true;
                this.softPageTurner.TurnPage(this.currentFold, RelativeTo.Page);
            }
            if (this.ShowPageFold != PageFoldVisibility.OnPageEnter)
                return;
            this.BuildPages();
            this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
        }

        private DataTemplate GetLeftPageTemplate(RadBookItem page)
        {
            if (this.LeftPageTemplate != null)
                return this.LeftPageTemplate;
            if (this.LeftPageTemplateSelector != null)
                return this.LeftPageTemplateSelector.SelectTemplate(page.Content, (DependencyObject)page);
            if (this.ItemTemplate != null)
                return this.ItemTemplate;
            if (this.ItemTemplateSelector != null)
                return this.ItemTemplateSelector.SelectTemplate(page.Content, (DependencyObject)page);
            else
                return page.ContentTemplate;
        }

        private DataTemplate GetRightPageTemplate(RadBookItem page)
        {
            if (this.RightPageTemplate != null)
                return this.RightPageTemplate;
            if (this.RightPageTemplateSelector != null)
                return this.RightPageTemplateSelector.SelectTemplate(page.Content, (DependencyObject)page);
            if (this.ItemTemplate != null)
                return this.ItemTemplate;
            if (this.ItemTemplateSelector != null)
                return this.ItemTemplateSelector.SelectTemplate(page.Content, (DependencyObject)page);
            else
                return page.ContentTemplate;
        }

        private RadBookItem GetUnderPage()
        {
            int num = this.newSheetIndex * 2;
            int index = this.FirstPagePosition == PagePosition.Right ? num - 1 : num;
            if (this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft)
                return this.GetPage(index);
            else
                return this.GetPage(index + 1);
        }

        private RadBookItem GetBackPage()
        {
            int num = this.newSheetIndex * 2;
            int index = this.FirstPagePosition == PagePosition.Right ? num - 1 : num;
            if (this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft)
                return this.GetPage(index + 1);
            else
                return this.GetPage(index);
        }

        private RadBookItem GetForePage()
        {
            int num = this.oldSheetIndex * 2;
            int index = this.FirstPagePosition == PagePosition.Right ? num - 1 : num;
            if (this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft)
                return this.GetPage(index);
            else
                return this.GetPage(index + 1);
        }

        private RadBookItem GetOtherPage()
        {
            int num = this.oldSheetIndex * 2;
            int index = this.FirstPagePosition == PagePosition.Right ? num - 1 : num;
            if (this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft)
                return this.GetPage(index + 1);
            else
                return this.GetPage(index);
        }

        private void CheckIfPageCanBeDraggedFurther(ref Point hoverPoint)
        {
            double width = this.PageDimensions.Width;
            double num1 = this.currentFold == FoldPosition.TopRight || this.currentFold == FoldPosition.BottomRight ? hoverPoint.X : hoverPoint.X - width;
            double y = hoverPoint.Y;
            double num2 = Math.Sqrt(num1 * num1 + y * y);
            if (num2 <= width)
                return;
            double num3 = num1 * (width / num2);
            double num4 = y * (width / num2);
            hoverPoint.X = this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft ? num3 + width : num3;
            hoverPoint.Y = num4;
        }

        private void Page_MouseDragStart(object sender, DragStartedEventArgs e)
        {
            RadBookItem page = sender as RadBookItem;
            if (page.IsHardPaper || page.IsHardPaperInternal || (this.raisePageChanged || this.raiseFoldDeactivated) || this.hardPaperAnimationInProgress)
                return;
            PageFlipEventArgs e1 = new PageFlipEventArgs((object)this, page, RadBook.PreviewPageFlipStartedEvent);
            this.OnPreviewPageFlipStarted(e1);
            this.isPreviewPageFlipStartedHandled = e1.Handled;
            this.isDraggingSoftPaper = true;
            if (e1.Handled)
            {
                page.CancelPageTurn();
                this.isDraggingSoftPaper = false;
            }
            else
            {
                this.rebuildPages = false;
                if (this.FoldSize == new Size(0.0, 0.0))
                {
                    this.currentFold = e.DraggedCorner;
                    this.oldSheetIndex = this.CurrentSheetIndex;
                    this.newSheetIndex = this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft ? this.oldSheetIndex - 1 : this.oldSheetIndex + 1;
                    this.softPageTurner.CurrentMousePosition = this.softPageTurner.GetCornerPoint(e.DraggedCorner, RelativeTo.Page);
                    this.softPageTurner.TurnPage(e.DraggedCorner, e.MousePoint, RelativeTo.Page);
                }
                else
                    this.softPageTurner.TurnPage(e.MousePoint);
                this.OnPageFlipStarted(new PageFlipEventArgs((object)this, page, RadBook.PageFlipStartedEvent));
            }
        }

        private void Page_MouseDragMove(object sender, DragInProgressEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (!this.isDraggingSoftPaper || this.hardPaperAnimationInProgress))
                return;
            this.rebuildPages = false;
            this.softPageTurner.TurnPage(e.MousePoint);
        }

        private void Page_MouseDragEnd(object sender, DragEndedEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (!this.isDraggingSoftPaper || this.hardPaperAnimationInProgress))
                return;
            this.rebuildPages = true;
            this.isDraggingSoftPaper = false;
            this.raisePageFlipEnded = true;
            this.pageForPageFlipEndedEvent = radBookItem;
            this.raiseFoldDeactivated = true;
            if (e.DragEndedOutsideOfPage)
            {
                this.buildSheetIndex = this.newSheetIndex;
                this.updateRightPageIndex = true;
                this.raisePageChanged = true;
                this.softPageTurner.TurnPage(e.TargetCorner, RelativeTo.Book);
            }
            else
            {
                this.buildSheetIndex = this.oldSheetIndex;
                this.softPageTurner.TurnPage(this.currentFold, RelativeTo.Page);
            }
        }

        private void Page_MouseSingleClick(object sender, MouseClickEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (this.isPreviewPageFlipStartedHandled || this.raisePageChanged) || (this.hardPaperAnimationInProgress || this.currentFold == FoldPosition.None))
                return;
            this.isDraggingSoftPaper = false;
            this.buildSheetIndex = this.newSheetIndex;
            this.rebuildPages = true;
            this.updateRightPageIndex = true;
            this.raisePageChanged = true;
            this.raiseFoldDeactivated = true;
            this.raisePageFlipEnded = true;
            this.pageForPageFlipEndedEvent = sender as RadBookItem;
            this.softPageTurner.Stop();
            this.softPageTurner.TurnPage(e.TargetCorner, RelativeTo.Book);
        }

        private void Page_MouseDoubleClick(object sender, MouseClickEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (this.isPreviewPageFlipStartedHandled || this.hardPaperAnimationInProgress))
                return;
            this.rebuildPages = true;
            this.isDraggingSoftPaper = false;
            this.updateRightPageIndex = true;
            this.buildSheetIndex = this.newSheetIndex;
            this.raisePageChanged = true;
            this.raiseFoldDeactivated = true;
            this.raisePageFlipEnded = true;
            this.pageForPageFlipEndedEvent = sender as RadBookItem;
            this.softPageTurner.TurnPage(e.TargetCorner, RelativeTo.Book);
        }

        private void Page_PageMouseEnter(object sender, PageMouseEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || this.hardPaperAnimationInProgress)
                return;
            this.executePageMouseMove = false;
            if (this.raisePageChanged)
                return;
            if (this.rebuildAfterMouseLeavesPage)
            {
                this.IsAutoCornerVisible = false;
                this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
                this.BuildPages();
                this.rebuildAfterMouseLeavesPage = false;
            }
            if (!e.Page.CanBeTurned)
                return;
            FoldPosition startingFold = this.GetStartingFold(e.Page.Position);
            this.currentFold = startingFold;
            this.rebuildPages = false;
            this.oldSheetIndex = this.CurrentSheetIndex;
            this.newSheetIndex = this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft ? this.oldSheetIndex - 1 : this.oldSheetIndex + 1;
            Point pointOfStartingFold = this.GetTargetPointOfStartingFold(startingFold);
            Point cornerPoint = this.softPageTurner.GetCornerPoint(startingFold, RelativeTo.Page);
            this.IsAutoCornerVisible = true;
            this.softPageTurner.CurrentMousePosition = cornerPoint;
            this.softPageTurner.Stop();
            this.softPageTurner.TurnPage(startingFold, pointOfStartingFold, RelativeTo.Page);
            this.OnFoldActivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldActivatedEvent));
        }

        private void Page_PageMouseMove(object sender, PageMouseEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (this.hardPaperAnimationInProgress || !this.executePageMouseMove))
                return;
            FoldPosition startingFold = this.GetStartingFold((sender as RadBookItem).Position);
            this.currentFold = startingFold;
            this.rebuildPages = false;
            Point pointOfStartingFold = this.GetTargetPointOfStartingFold(startingFold);
            this.softPageTurner.GetCornerPoint(startingFold, RelativeTo.Page);
            this.IsAutoCornerVisible = true;
            this.executePageMouseMove = false;
            this.softPageTurner.Stop();
            this.softPageTurner.TurnPage(startingFold, pointOfStartingFold, RelativeTo.Page);
            this.OnFoldActivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldActivatedEvent));
        }

        private void Page_PageMouseLeave(object sender, PageMouseEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (this.raisePageChanged || this.hardPaperAnimationInProgress))
                return;
            this.executePageMouseMove = false;
            this.rebuildAfterMouseLeavesPage = true;
        }

        private void Fold_MouseEnter(object sender, CornerEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (this.raisePageChanged || this.hardPaperAnimationInProgress))
                return;
            if (this.raiseFoldDeactivated)
            {
                this.BuildPages();
                this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
                this.raiseFoldDeactivated = false;
            }
            if (this.currentFold != e.Corner)
            {
                if (this.ShowPageFold == PageFoldVisibility.OnPageEnter && this.currentFold != FoldPosition.None)
                    this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
                this.currentFold = e.Corner;
                this.OnFoldActivated(new FoldEventArgs((object)this, e.Corner, RadBook.FoldActivatedEvent));
            }
            this.rebuildPages = false;
            this.oldSheetIndex = this.CurrentSheetIndex;
            this.newSheetIndex = this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft ? this.oldSheetIndex - 1 : this.oldSheetIndex + 1;
            this.softPageTurner.CurrentMousePosition = !this.IsAutoCornerVisible || e.Corner != this.GetStartingFold(radBookItem.Position) ? this.softPageTurner.GetCornerPoint(e.Corner, RelativeTo.Page) : this.softPageTurner.CurrentMousePosition;
            if (this.ShowPageFold == PageFoldVisibility.Never)
                return;
            if (this.IsAutoCornerVisible)
                this.softPageTurner.TurnPage(e.MousePoint);
            else
                this.softPageTurner.TurnPage(e.Corner, e.MousePoint, RelativeTo.Page);
        }

        private void Fold_MouseMove(object sender, CornerEventArgs e)
        {
            RadBookItem page = sender as RadBookItem;
            if (page.IsHardPaper || page.IsHardPaperInternal || (this.raisePageChanged || this.raiseFoldDeactivated) || this.hardPaperAnimationInProgress)
                return;
            this.currentFold = e.Corner;
            if (FoldHelper.GetQuadrant(this.softPageTurner.CurrentMousePosition, page) != this.currentFold)
                this.softPageTurner.CurrentMousePosition = this.softPageTurner.GetCornerPoint(this.currentFold, RelativeTo.Page);
            this.rebuildPages = false;
            this.oldSheetIndex = this.CurrentSheetIndex;
            this.newSheetIndex = this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft ? this.oldSheetIndex - 1 : this.oldSheetIndex + 1;
            if (this.ShowPageFold == PageFoldVisibility.Never)
                return;
            this.softPageTurner.TurnPage(e.MousePoint);
        }

        private void Fold_MouseLeave(object sender, CornerEventArgs e)
        {
            RadBookItem radBookItem = sender as RadBookItem;
            if (radBookItem.IsHardPaper || radBookItem.IsHardPaperInternal || (this.raisePageChanged || this.hardPaperAnimationInProgress))
                return;
            FoldPosition startingFold = this.GetStartingFold((sender as RadBookItem).Position);
            if (this.ShowPageFold == PageFoldVisibility.OnPageEnter && e.Corner == startingFold)
            {
                this.softPageTurner.TurnPage(this.GetTargetPointOfStartingFold(startingFold));
            }
            else
            {
                if (this.ShowPageFold == PageFoldVisibility.Never)
                    return;
                this.executePageMouseMove = true;
                this.rebuildPages = true;
                this.buildSheetIndex = this.oldSheetIndex;
                this.IsAutoCornerVisible = false;
                this.softPageTurner.TurnPage(e.Corner, RelativeTo.Page);
                this.raiseFoldDeactivated = this.ShowPageFold == PageFoldVisibility.OnFoldEnter;
                this.rebuildAfterMouseLeavesPage = false;
            }
        }

        private void PageTurner_Turning(object sender, PageTurnEventArgs e)
        {
            double num = 1.0;
            this.UpdatePageTurn(new Point()
            {
                X = e.HoverPoint.X,
                Y = e.HoverPoint.Y > num || e.HoverPoint.Y < 0.0 ? (e.HoverPoint.Y < -num || e.HoverPoint.Y > 0.0 ? e.HoverPoint.Y : -num) : num
            });
        }

        private void PageTurner_Completed(object sender, PageTurnEventArgs e)
        {
            this.mouseTurn = true;
            if (this.updateRightPageIndex)
                this.RightPageIndex = RadBook.ConvertSheetToPageIndex(this.newSheetIndex, this.FirstPagePosition, this.Items.Count);
            this.mouseTurn = false;
            if (this.raiseFoldDeactivated)
            {
                this.raiseFoldDeactivated = false;
                this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
            }
            if (this.raisePageFlipEnded)
            {
                this.raisePageFlipEnded = false;
                this.OnPageFlipEnded(new PageFlipEventArgs((object)this, this.pageForPageFlipEndedEvent, RadBook.PageFlipEndedEvent));
                this.pageForPageFlipEndedEvent = (RadBookItem)null;
            }
            if (this.raisePageChanged)
            {
                this.raisePageChanged = false;
                this.OnPageChanged(new ExtendedRoutedEventArgs(RadBook.PageChangedEvent, (object)this));
            }
            this.updateRightPageIndex = false;
            if (!this.rebuildPages)
                return;
            this.BuildPages();
        }

        private void UpdatePageTurn(Point hoverPoint)
        {
            double width = this.PageDimensions.Width;
            double height = this.PageDimensions.Height;
            if (width == 0.0 || height == 0.0)
                return;
            if (this.currentFold == FoldPosition.BottomLeft || this.currentFold == FoldPosition.BottomRight)
                hoverPoint.Y -= height;
            if (hoverPoint.Y == 0.0)
                hoverPoint.Y = 1.0;
            this.CheckIfPageCanBeDraggedFurther(ref hoverPoint);
            this.otherPage = this.GetOtherPage();
            this.forePage = this.GetForePage();
            this.backPage = this.GetBackPage();
            this.underPage = this.GetUnderPage();
            RadBook.NullifyRenderTransform((UIElement)this.otherPage, (UIElement)this.forePage, (UIElement)this.backPage, (UIElement)this.underPage);
            this.SetZIndex();
            this.VisualizePages();
            this.UpdateIsHitTestVisible();
            if (this.currentFold == FoldPosition.TopRight || this.currentFold == FoldPosition.BottomRight)
            {
                if (this.backPage != null)
                    this.backPage.Clip = (Geometry)ClipCalculator.GetRightSideBackPageClip(hoverPoint, width, height);
                if (this.forePage != null)
                    this.forePage.Clip = (Geometry)ClipCalculator.GetRightSideForePageClip(hoverPoint, width, height);
            }
            if (this.currentFold == FoldPosition.TopLeft || this.currentFold == FoldPosition.BottomLeft)
            {
                if (this.backPage != null)
                    this.backPage.Clip = (Geometry)ClipCalculator.GetLeftSideBackPageClip(hoverPoint, width, height);
                if (this.forePage != null)
                    this.forePage.Clip = (Geometry)ClipCalculator.GetLeftSideForePageClip(hoverPoint, width, height);
            }
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform translateTransform1 = new TranslateTransform();
            RotateTransform backPageRotateTransform = new RotateTransform();
            if (this.backPage != null)
            {
                TranslateTransform translateTransform2 = TransformCalculator.GetTranslateTransform(hoverPoint, width, this.currentFold);
                backPageRotateTransform = TransformCalculator.GetRotateTransform(hoverPoint, width, height, this.currentFold, this.backPage.Position);
                transformGroup.Children.Add((Transform)backPageRotateTransform);
                transformGroup.Children.Add((Transform)translateTransform2);
                this.backPage.RenderTransform = (Transform)transformGroup;
                if (this.backPage.Position == PagePosition.Left)
                {
                    if (this.backPage.RightBackPageShadow != null)
                    {
                        this.backPage.RightBackPageShadow.Visibility = Visibility.Visible;
                        this.backPage.RightBackPageShadow.RenderTransform = (Transform)RadBook.GetShadowTransform(width, height, PagePosition.Left, PagePosition.Left, backPageRotateTransform);
                    }
                    if (this.backPage.RightBackPageShadowStatic != null)
                        this.backPage.RightBackPageShadowStatic.Visibility = Visibility.Collapsed;
                }
                else if (this.backPage.LeftBackPageShadow != null)
                {
                    this.backPage.LeftBackPageShadow.Visibility = Visibility.Visible;
                    this.backPage.LeftBackPageShadow.RenderTransform = (Transform)RadBook.GetShadowTransform(width, height, PagePosition.Right, PagePosition.Left, backPageRotateTransform);
                    this.backPage.LeftBackPageShadow.Opacity = RadBook.GetShadowOpacity(hoverPoint, width, PagePosition.Left);
                }
            }
            if (this.underPage == null)
                return;
            this.underPage.Clip = (Geometry)ClipCalculator.GetUnderPageClip(width, height);
            if (this.underPage.Position == PagePosition.Right)
            {
                if (this.underPage.RightUnderPageShadow == null)
                    return;
                this.underPage.RightUnderPageShadow.Visibility = Visibility.Visible;
                this.underPage.RightUnderPageShadow.RenderTransform = (Transform)RadBook.GetShadowTransform(width, height, PagePosition.Right, PagePosition.Right, backPageRotateTransform);
                this.underPage.RightUnderPageShadow.Opacity = RadBook.GetShadowOpacity(hoverPoint, width, PagePosition.Right);
            }
            else
            {
                if (this.underPage.LeftUnderPageShadow == null)
                    return;
                this.underPage.LeftUnderPageShadow.Visibility = Visibility.Visible;
                this.underPage.LeftUnderPageShadow.RenderTransform = (Transform)RadBook.GetShadowTransform(width, height, PagePosition.Left, PagePosition.Right, backPageRotateTransform);
                this.underPage.LeftUnderPageShadow.Opacity = RadBook.GetShadowOpacity(hoverPoint, width, PagePosition.Left);
            }
        }

        private void UpdateVirtualizingPanelIndex(int index)
        {
            BookPanel bookPanel = this.itemsPanel as BookPanel;
            if (bookPanel == null)
                return;
            bookPanel.StartingIndex = Math.Max(0, index - bookPanel.GenerateSpan / 2);
        }

        /// <summary>
        /// Gets a page based on the index of the page.
        /// 
        /// </summary>
        private RadBookItem GetPage(int index)
        {
            if (index >= 0)
                return this.ItemContainerGenerator.ContainerFromIndex(index) as RadBookItem;
            else
                return (RadBookItem)null;
        }

        private RadBookItem GetPage(int sheetIndex, FoldPosition corner)
        {
            int num = sheetIndex * 2;
            int index = this.FirstPagePosition == PagePosition.Right ? num - 1 : num;
            if (corner == FoldPosition.TopLeft || corner == FoldPosition.BottomLeft)
                return this.GetPage(index);
            else
                return this.GetPage(index + 1);
        }

        private int CoerceRightPageIndex(int rightPageIndex)
        {
            if (rightPageIndex < 0)
                return 0;
            else
                return Math.Min(rightPageIndex, this.Items.Count - 1);
        }

        private void RaiseItemsCountChanged()
        {
            if (this.itemsCountChanged == null)
                return;
            this.itemsCountChanged((object)this, EventArgs.Empty);
        }

        private bool CanGoToFirstPage()
        {
            return RadBook.ConvertPageToSheetIndex(this.GetFirstPageIndex(), this.FirstPagePosition) < this.CurrentSheetIndex;
        }

        private int GetFirstPageIndex()
        {
            return Math.Min(0, this.Items.Count - 1);
        }

        private bool CanGoToPreviousPage()
        {
            return RadBook.ConvertPageToSheetIndex(this.GetPreviousPageIndex(), this.FirstPagePosition) < this.CurrentSheetIndex;
        }

        private int GetPreviousPageIndex()
        {
            return Math.Max(0, this.RightPageIndex - 2);
        }

        private bool CanGoToNextPage()
        {
            return this.CurrentSheetIndex < RadBook.ConvertPageToSheetIndex(this.GetNextPageIndex(), this.FirstPagePosition);
        }

        private int GetNextPageIndex()
        {
            return Math.Min(this.Items.Count - 1, this.RightPageIndex + 2);
        }

        private bool CanGoToLastPage()
        {
            return this.CurrentSheetIndex < RadBook.ConvertPageToSheetIndex(this.GetLastPageIndex(), this.FirstPagePosition);
        }

        private int GetLastPageIndex()
        {
            return this.Items.Count - 1;
        }

        private void NavigateToPage(PageToNavigateTo pageToNavigateTo)
        {
            switch (pageToNavigateTo)
            {
                case PageToNavigateTo.First:
                    this.RightPageIndex = this.GetFirstPageIndex();
                    break;
                case PageToNavigateTo.Previous:
                    this.RightPageIndex = this.GetPreviousPageIndex();
                    break;
                case PageToNavigateTo.Next:
                    this.RightPageIndex = this.GetNextPageIndex();
                    break;
                case PageToNavigateTo.Last:
                    this.RightPageIndex = this.GetLastPageIndex();
                    break;
            }
        }

        private void UpdateVirtualization(int rightPageIndex)
        {
            if (this.BookPanel == null || !this.IsVirtualizing)
                return;
            this.BookPanel.IndexCollection.Clear();
            this.BookPanel.IndexCollection.Add(Math.Max(rightPageIndex - 3, 0));
            this.BookPanel.IndexCollection.Add(Math.Max(rightPageIndex - 2, 0));
            this.BookPanel.IndexCollection.Add(Math.Max(rightPageIndex - 1, 0));
            this.BookPanel.IndexCollection.Add(rightPageIndex);
            this.BookPanel.IndexCollection.Add(Math.Min(rightPageIndex + 1, this.Items.Count - 1));
            this.BookPanel.IndexCollection.Add(Math.Min(rightPageIndex + 2, this.Items.Count - 1));
            this.BookPanel.IndexCollection.Add(Math.Min(rightPageIndex + 3, this.Items.Count - 1));
            this.BookPanel.InvalidateMeasure();
            this.BookPanel.UpdateLayout();
        }

        private void OnRightPageIndexChanged(int rightPageIndex)
        {
            this.UpdateVirtualization(rightPageIndex);
            this.CurrentSheetIndex = RadBook.ConvertPageToSheetIndex(rightPageIndex, this.FirstPagePosition);
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnShowPageFoldChanged()
        {
            this.IsAutoCornerVisible = false;
        }

        private void OnHardPagesChanged()
        {
            if (this.itemsPanel != null)
            {
                for (int index = 0; index < this.itemsPanel.Children.Count; ++index)
                {
                    RadBookItem radBookItem = this.itemsPanel.Children[index] as RadBookItem;
                    radBookItem.CancelPagesRebuild = true;
                    radBookItem.IsHardPaper = false;
                    radBookItem.CancelPagesRebuild = false;
                }
            }
            this.BuildPages();
        }

        private FoldPosition GetStartingFold(PagePosition pagePosition)
        {
            if (pagePosition == PagePosition.Left)
                return this.FoldHintPosition != FoldHintPosition.Top ? FoldPosition.BottomLeft : FoldPosition.TopLeft;
            else
                return this.FoldHintPosition != FoldHintPosition.Top ? FoldPosition.BottomRight : FoldPosition.TopRight;
        }

        private Point GetTargetPointOfStartingFold(FoldPosition startingFold)
        {
            switch (startingFold)
            {
                case FoldPosition.TopLeft:
                    return new Point(this.FoldSize.Width, this.FoldSize.Height);
                case FoldPosition.TopRight:
                    return new Point(this.ActualWidth / 2.0 - this.FoldSize.Width, this.FoldSize.Height);
                case FoldPosition.BottomRight:
                    return new Point(this.ActualWidth / 2.0 - this.FoldSize.Width, this.ActualHeight - this.FoldSize.Height);
                case FoldPosition.BottomLeft:
                    return new Point(this.FoldSize.Width, this.ActualHeight - this.FoldSize.Height);
                default:
                    return new Point(0.0, 0.0);
            }
        }

        private void SetZIndex()
        {
            short num = short.MaxValue;
            if (this.underPage != null)
                this.underPage.SetValue(Panel.ZIndexProperty, (object)((int)num - 3));
            if (this.otherPage != null)
                this.otherPage.SetValue(Panel.ZIndexProperty, (object)((int)num - 3));
            if (this.forePage != null)
                this.forePage.SetValue(Panel.ZIndexProperty, (object)((int)num - 2));
            if (this.backPage == null)
                return;
            this.backPage.SetValue(Panel.ZIndexProperty, (object)((int)num - 1));
        }

        private void VisualizePages()
        {
            if (this.underPage != null)
                this.underPage.Visibility = Visibility.Visible;
            if (this.otherPage != null)
                this.otherPage.Visibility = Visibility.Visible;
            if (this.forePage != null)
                this.forePage.Visibility = Visibility.Visible;
            if (this.backPage == null)
                return;
            this.backPage.Visibility = Visibility.Visible;
        }

        private void UpdateIsHitTestVisible()
        {
            if (this.otherPage != null)
                this.otherPage.IsHitTestVisible = !this.raisePageChanged;
            if (this.forePage != null)
                this.forePage.IsHitTestVisible = true;
            if (this.backPage != null)
                this.backPage.IsHitTestVisible = false;
            if (this.underPage == null)
                return;
            this.underPage.IsHitTestVisible = false;
        }

        /// <summary>
        /// Determines which pages to set hard with regard to the HardPages property.
        /// 
        /// </summary>
        /// <param name="page">The page.</param>
        private void SetHardPages(RadBookItem page)
        {
            if (this.HardPages == HardPages.None)
                page.IsHardPaper = false;
            if (this.HardPages == HardPages.First)
                this.SetFirstPageHard(page);
            if (this.HardPages == HardPages.Last)
                this.SetLastPageHard(page);
            if (this.HardPages == HardPages.FirstAndLast)
            {
                this.SetFirstPageHard(page);
                this.SetLastPageHard(page);
            }
            if (this.HardPages == HardPages.Custom && page.IsHardPaper)
            {
                if (page.Position == PagePosition.Left)
                    this.SetPreviousPageHard(page, true);
                else
                    this.SetNextPageHard(page, true);
            }
            if (this.HardPages != HardPages.All)
                return;
            page.IsHardPaper = true;
        }

        /// <summary>
        /// Sets the last page hard.
        /// 
        /// </summary>
        /// <param name="page">The page.</param>
        private void SetLastPageHard(RadBookItem page)
        {
            if (!page.IsLastPage || page.Position != PagePosition.Left)
                return;
            page.IsHardPaper = true;
            this.SetPreviousPageHard(page, true);
        }

        /// <summary>
        /// Sets the previous page hard.
        /// 
        /// </summary>
        private void SetPreviousPageHard(RadBookItem currentPage, bool isHardPaperInternal)
        {
            RadBookItem radBookItem = this.ItemContainerGenerator.ContainerFromIndex(currentPage.Index - 1) as RadBookItem;
            if (radBookItem == null)
                return;
            radBookItem.IsHardPaperInternal = isHardPaperInternal;
        }

        /// <summary>
        /// Sets the first page hard.
        /// 
        /// </summary>
        /// <param name="page">The page.</param>
        private void SetFirstPageHard(RadBookItem page)
        {
            if (!page.IsFirstPage || page.Position != PagePosition.Right)
                return;
            page.IsHardPaper = true;
            this.SetNextPageHard(page, true);
        }

        /// <summary>
        /// Sets the next page hard.
        /// 
        /// </summary>
        /// <param name="page">The page.</param><param name="isHardPaperInternal">If set to <c>true</c> [is hard paper internal].</param>
        private void SetNextPageHard(RadBookItem page, bool isHardPaperInternal)
        {
            RadBookItem radBookItem = this.ItemContainerGenerator.ContainerFromIndex(page.Index + 1) as RadBookItem;
            if (radBookItem == null)
                return;
            radBookItem.IsHardPaperInternal = isHardPaperInternal;
        }

        private void HardPaperTurner_Turning(object sender, PageTurnEventArgs e)
        {
            if (this.forePage == null && this.backPage == null)
                return;
            double actualWidth = this.ActualWidth;
            double actualHeight = this.ActualHeight;
            double num1 = this.ActualWidth / 2.0;
            Point hoverPoint = e.HoverPoint;
            double num2 = Math.Max(-1.0, Math.Min(this.currentFold != FoldPosition.BottomLeft ? (hoverPoint.X - num1) / num1 : (num1 - hoverPoint.X) / num1, 1.0));
            this.forePageScaleTransform.ScaleX = num2;
            this.backPageScaleTransform.ScaleX = -num2;
            if (num2 >= 0.0)
            {
                this.forePage.Visibility = Visibility.Visible;
                this.backPage.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.forePage.Visibility = Visibility.Collapsed;
                this.backPage.Visibility = Visibility.Visible;
            }
            double num3 = this.Clamp(e.HoverPoint.X, 0.0, actualWidth);
            double num4 = this.FoldSize.Height * Math.Sqrt(1.0 - this.Sqr((num3 - num1) / num1));
            double x;
            double y;
            if (num2 >= 0.0)
            {
                x = num3 - num1;
                y = num4;
            }
            else
            {
                x = num1 - num3;
                y = num4;
            }
            double num5 = Math.Atan2(y, x) * 57.2957795130823;
            this.skewTransform.AngleY = num2 >= 0.0 ? -num5 : num5;
        }

        private void HardPaperTurner_Completed(object sender, PageTurnEventArgs e)
        {
            if (this.raiseFoldDeactivated)
            {
                this.OnFoldDeactivated(new FoldEventArgs((object)this, this.currentFold, RadBook.FoldDeactivatedEvent));
                this.raiseFoldDeactivated = true;
            }
            if (!this.isDraggingHardPaper)
            {
                this.OnPageFlipEnded(new PageFlipEventArgs((object)this, this.forePage, RadBook.PageFlipEndedEvent));
                if (this.updateRightPageIndex)
                {
                    this.buildPagesWithoutAnimation = true;
                    this.RightPageIndex = RadBook.ConvertSheetToPageIndex(this.newSheetIndex, this.FirstPagePosition, this.Items.Count);
                    this.buildPagesWithoutAnimation = false;
                }
                else if (this.raisePageChanged)
                    this.OnPageChanged(new ExtendedRoutedEventArgs(RadBook.PageChangedEvent));
            }
            this.raisePageChanged = false;
            this.hardPaperAnimationInProgress = false;
        }

        private void RadBook_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            if (this.hardPaperAnimationInProgress)
                return;
            Point position = e.GetPosition((IInputElement)this);
            this.mouseDownPoint = position;
            bool flag1 = FoldHelper.MouseWithinBottomRightFoldArea(position, this.FoldSize, this.ActualWidth, this.ActualHeight);
            bool flag2 = FoldHelper.MouseWithinBottomLeftFoldArea(position, this.FoldSize, this.ActualHeight);
            bool flag3 = FoldHelper.MouseWithinTopRightFoldArea(position, this.FoldSize, this.ActualWidth);
            bool flag4 = FoldHelper.MouseWithinTopLeftFoldArea(position, this.FoldSize);
            if (!flag2 && !flag1 && (!flag3 && !flag4))
                return;
            if (flag3 || flag4)
                position.Y = this.ActualHeight - position.Y;
            FoldPosition quadrant = FoldHelper.GetQuadrant(position, this);
            RadBookItem page = this.GetPage(this.currentSheetIndex, quadrant);
            if (page != null && !page.IsHardPaper && !page.IsHardPaperInternal)
                return;
            this.oldSheetIndex = this.CurrentSheetIndex;
            this.currentFold = quadrant;
            if (quadrant == FoldPosition.BottomLeft)
            {
                this.newSheetIndex = this.oldSheetIndex - 1;
                this.hardPageTurner.CurrentMousePosition = new Point(0.0, this.ActualHeight);
            }
            else
            {
                this.newSheetIndex = this.oldSheetIndex + 1;
                this.hardPageTurner.CurrentMousePosition = new Point(this.ActualWidth, this.ActualHeight);
            }
            this.currentFold = quadrant;
            this.forePage = this.GetForePage();
            this.backPage = this.GetBackPage();
            if (this.forePage == null || this.backPage == null)
                return;
            this.OnFoldActivated(new FoldEventArgs((object)this.forePage, this.currentFold, RadBook.FoldActivatedEvent));
            PageFlipEventArgs e1 = new PageFlipEventArgs((object)this, this.forePage, RadBook.PreviewPageFlipStartedEvent);
            this.OnPreviewPageFlipStarted(e1);
            if (e1.Handled)
                return;
            this.OnPageFlipStarted(new PageFlipEventArgs((object)this, this.forePage, RadBook.PageFlipStartedEvent));
            this.isDraggingHardPaper = true;
            this.bookClicked = true;
            if (!this.PrepareForeAndBackPageForHardPaperTurn())
                return;
            this.hardPaperAnimationInProgress = true;
            this.hardPageTurner.TurnPage(position);
        }

        private void RadBook_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isDraggingHardPaper)
                return;
            Point position = e.GetPosition((IInputElement)this);
            this.bookClicked = position == this.mouseDownPoint;
            this.hardPageTurner.TurnPage(position);
        }

        private void RadBook_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.bookClicked && !this.isDraggingHardPaper)
                return;
            Point position = e.GetPosition((IInputElement)this);
            if (this.bookClicked)
            {
                bool flag1 = FoldHelper.MouseWithinBottomRightFoldArea(position, this.FoldSize, this.ActualWidth, this.ActualHeight);
                bool flag2 = FoldHelper.MouseWithinBottomLeftFoldArea(position, this.FoldSize, this.ActualHeight);
                bool flag3 = FoldHelper.MouseWithinTopRightFoldArea(position, this.FoldSize, this.ActualWidth);
                bool flag4 = FoldHelper.MouseWithinTopLeftFoldArea(position, this.FoldSize);
                this.updateRightPageIndex = true;
                if (flag1 || flag3)
                    this.hardPageTurner.TurnPage(new Point(0.0, this.ActualHeight));
                if (flag2 || flag4)
                    this.hardPageTurner.TurnPage(new Point(this.ActualWidth, this.ActualHeight));
                this.raisePageChanged = true;
            }
            else
            {
                double num = this.ActualWidth / 2.0;
                if (this.isDraggingHardPaper)
                {
                    if (position.X < num)
                    {
                        this.updateRightPageIndex = this.currentFold != FoldPosition.BottomLeft;
                        this.hardPageTurner.TurnPage(new Point(0.0, this.ActualHeight));
                    }
                    else
                    {
                        this.updateRightPageIndex = this.currentFold == FoldPosition.BottomLeft;
                        this.hardPageTurner.TurnPage(new Point(this.ActualWidth, this.ActualHeight));
                    }
                    this.raisePageChanged = this.updateRightPageIndex;
                }
            }
            this.isDraggingHardPaper = false;
            this.raiseFoldDeactivated = true;
            if (this.forePage != null)
                this.forePage.ReleaseMouseCapture();
            this.bookClicked = false;
        }

        private bool PrepareForeAndBackPageForHardPaperTurn()
        {
            for (int index = 0; index < this.itemsPanel.Children.Count; ++index)
            {
                RadBookItem radBookItem = this.ItemContainerGenerator.ContainerFromIndex(index) as RadBookItem;
                if (radBookItem != null)
                    Panel.SetZIndex((UIElement)radBookItem, -1);
            }
            if (this.forePage == null || this.backPage == null)
                return false;
            TransformGroup transformGroup1 = new TransformGroup();
            TransformGroup transformGroup2 = new TransformGroup();
            this.skewTransform = new SkewTransform();
            this.forePageScaleTransform = new ScaleTransform();
            this.backPageScaleTransform = new ScaleTransform();
            transformGroup1.Children.Add((Transform)this.forePageScaleTransform);
            transformGroup1.Children.Add((Transform)this.skewTransform);
            transformGroup2.Children.Add((Transform)this.backPageScaleTransform);
            transformGroup2.Children.Add((Transform)this.skewTransform);
            this.forePage.RenderTransform = (Transform)transformGroup1;
            this.backPage.RenderTransform = (Transform)transformGroup2;
            this.backPage.RenderTransformOrigin = this.currentFold == FoldPosition.BottomLeft ? new Point(0.0, 0.5) : new Point(1.0, 0.5);
            this.forePage.RenderTransformOrigin = this.currentFold == FoldPosition.BottomLeft ? new Point(1.0, 0.5) : new Point(0.0, 0.5);
            if (this.isDraggingHardPaper)
            {
                try
                {
                    this.forePage.CaptureMouse();
                }
                catch
                {
                }
            }
            this.forePage.SetValue(Panel.ZIndexProperty, (object)2);
            this.backPage.Visibility = Visibility.Collapsed;
            this.backPage.SetValue(Panel.ZIndexProperty, (object)1);
            this.underPage = this.GetUnderPage();
            if (this.underPage != null)
            {
                this.underPage.Visibility = Visibility.Visible;
                this.underPage.SetValue(Panel.ZIndexProperty, (object)0);
                this.underPage.RenderTransform = (Transform)null;
            }
            this.otherPage = this.GetOtherPage();
            if (this.otherPage != null)
            {
                this.otherPage.Visibility = Visibility.Visible;
                this.otherPage.SetValue(Panel.ZIndexProperty, (object)0);
                this.otherPage.RenderTransform = (Transform)null;
            }
            return true;
        }

        private double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        private double Sqr(double x)
        {
            return x * x;
        }

        private void PreLoadPage(RadBookItem page)
        {
            if (this.PageIndexBelongsToSheetIndex(page.Index, this.buildSheetIndex))
            {
                Panel.SetZIndex((UIElement)page, 2);
                page.Visibility = Visibility.Visible;
            }
            else if (this.PageIndexBelongsToSheetIndex(page.Index, this.buildSheetIndex - 1))
            {
                Panel.SetZIndex((UIElement)page, 0);
                page.RenderTransform = (Transform)new TranslateTransform()
                {
                    X = (double)int.MaxValue
                };
                page.Visibility = Visibility.Visible;
            }
            else if (this.PageIndexBelongsToSheetIndex(page.Index, this.buildSheetIndex + 1))
            {
                Panel.SetZIndex((UIElement)page, 1);
                page.RenderTransform = (Transform)new TranslateTransform()
                {
                    X = (double)int.MaxValue
                };
                page.Visibility = Visibility.Visible;
            }
            else
            {
                Panel.SetZIndex((UIElement)page, -1);
                page.Visibility = Visibility.Collapsed;
            }
        }

        private bool PageIndexBelongsToSheetIndex(int pageIndex, int sheetIndex)
        {
            int num1;
            int num2;
            if (this.FirstPagePosition == PagePosition.Left)
            {
                num1 = sheetIndex * 2;
                num2 = sheetIndex * 2 + 1;
            }
            else
            {
                num1 = sheetIndex * 2 - 1;
                num2 = sheetIndex * 2;
            }
            if (pageIndex != num1)
                return pageIndex == num2;
            else
                return true;
        }
    }
}
