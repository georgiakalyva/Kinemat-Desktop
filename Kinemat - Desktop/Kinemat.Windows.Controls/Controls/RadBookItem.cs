// Type: Telerik.Windows.Controls.RadBookItem
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Kinemat.Windows;
using Kinemat.Windows.Controls.Book;

namespace Kinemat.Windows.Controls
{
    /// <summary>
    /// Represents a page within a book.
    /// 
    /// </summary>
    [DefaultProperty("Position")]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(RadBookItem))]
    public class RadBookItem : ContentControl
    {
        private WeakReference parentReference = new WeakReference((object)null);
        private DispatcherTimer singleClickTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(300.0)
        };
        private DispatcherTimer doubleClickTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(300.0)
        };
        private Point mousePoint = new Point();
        /// <summary>
        /// Identifies the IsHardPaper dependency property.
        /// 
        /// </summary>
        public static readonly DependencyProperty IsHardPaperProperty = DependencyProperty.Register("IsHardPaper", typeof(bool), typeof(RadBookItem), new PropertyMetadata(new PropertyChangedCallback(RadBookItem.OnIsHardPaperPropertyChanged)));
        private static readonly DependencyPropertyKey IndexPropertyKey = DependencyPropertyExtensions.RegisterReadOnly("Index", typeof(int), typeof(RadBookItem), (PropertyMetadata)null);
        private static readonly DependencyPropertyKey PositionPropertyKey = DependencyPropertyExtensions.RegisterReadOnly("Position", typeof(PagePosition), typeof(RadBookItem), (PropertyMetadata)null);
        /// <summary>
        /// DependencyProperty for the Index property.
        /// 
        /// </summary>
        public static readonly DependencyProperty IndexProperty = RadBookItem.IndexPropertyKey.DependencyProperty;
        /// <summary>
        /// DependencyProperty for the Position property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = RadBookItem.PositionPropertyKey.DependencyProperty;
        private bool singleClickMode;
        private bool dragStarted;
        private bool doubleClickMode;
        private FoldPosition draggedCorner;
        private FoldPosition targetCorner;
        private FoldPosition lastEnteredCorner;
        private FrameworkElement rightUnderPageShadow;
        private FrameworkElement leftUnderPageShadow;
        private FrameworkElement rightBackPageShadow;
        private FrameworkElement rightBackPageShadowStatic;
        private FrameworkElement leftBackPageShadow;
        private EventHandler<DragEndedEventArgs> mouseDragEnd;
        private EventHandler<DragInProgressEventArgs> mouseDragMove;
        private EventHandler<DragStartedEventArgs> mouseDragStart;
        private EventHandler<MouseClickEventArgs> mouseSingleClick;
        private EventHandler<MouseClickEventArgs> mouseDoubleClick;
        private EventHandler<CornerEventArgs> foldMouseEnter;
        private EventHandler<CornerEventArgs> foldMouseLeave;
        private EventHandler<CornerEventArgs> foldMouseMove;
        private EventHandler<PageMouseEventArgs> pageMouseEnter;
        private EventHandler<PageMouseEventArgs> pageMouseLeave;
        private EventHandler<PageMouseEventArgs> pageMouseMove;

        /// <summary>
        /// Gets the index of the page.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The index.
        /// </value>
        public int Index
        {
            get
            {
                return (int)this.GetValue(RadBookItem.IndexProperty);
            }
            internal set
            {
                this.SetValue(RadBookItem.IndexPropertyKey, (object)value);
            }
        }

        /// <summary>
        /// Gets the position of the page - Left or Right.
        /// 
        /// </summary>
        public PagePosition Position
        {
            get
            {
                return (PagePosition)this.GetValue(RadBookItem.PositionProperty);
            }
            internal set
            {
                this.SetValue(RadBookItem.PositionPropertyKey, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the IsHardPaper property.
        /// 
        /// </summary>
        public bool IsHardPaper
        {
            get
            {
                return (bool)this.GetValue(RadBookItem.IsHardPaperProperty);
            }
            set
            {
                this.SetValue(RadBookItem.IsHardPaperProperty, value);
                // this.SetValue(RadBookItem.IsHardPaperProperty, (object)bool.Parse(value ? 1 : 0));
            }
        }

        internal RadBook ParentBook
        {
            get
            {
                return this.parentReference.Target as RadBook;
            }
            set
            {
                this.parentReference = new WeakReference((object)value);
            }
        }

        internal FrameworkElement RightUnderPageShadow
        {
            get
            {
                return this.rightUnderPageShadow;
            }
        }

        internal FrameworkElement LeftUnderPageShadow
        {
            get
            {
                return this.leftUnderPageShadow;
            }
        }

        internal FrameworkElement RightBackPageShadow
        {
            get
            {
                return this.rightBackPageShadow;
            }
        }

        internal FrameworkElement RightBackPageShadowStatic
        {
            get
            {
                return this.rightBackPageShadowStatic;
            }
        }

        internal FrameworkElement LeftBackPageShadow
        {
            get
            {
                return this.leftBackPageShadow;
            }
        }

        internal bool CanBeTurned { get; set; }

        internal bool IsFirstPage { get; set; }

        internal bool IsLastPage { get; set; }

        internal bool IsHardPaperInternal { get; set; }

        internal bool CancelPagesRebuild { get; set; }

        internal Size FoldSize
        {
            get
            {
                if (this.ParentBook != null)
                    return this.ParentBook.FoldSize;
                else
                    return RadBook.DefaultPageFoldSize;
            }
        }

        private PageFlipMode PageFlipMode
        {
            get
            {
                if (this.ParentBook != null)
                    return this.ParentBook.PageFlipMode;
                else
                    return PageFlipMode.None;
            }
        }

        internal event EventHandler<DragEndedEventArgs> MouseDragEnd
        {
            add
            {
                EventHandler<DragEndedEventArgs> eventHandler = this.mouseDragEnd;
                EventHandler<DragEndedEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<DragEndedEventArgs>>(ref this.mouseDragEnd, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<DragEndedEventArgs> eventHandler = this.mouseDragEnd;
                EventHandler<DragEndedEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<DragEndedEventArgs>>(ref this.mouseDragEnd, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<DragInProgressEventArgs> MouseDragMove
        {
            add
            {
                EventHandler<DragInProgressEventArgs> eventHandler = this.mouseDragMove;
                EventHandler<DragInProgressEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<DragInProgressEventArgs>>(ref this.mouseDragMove, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<DragInProgressEventArgs> eventHandler = this.mouseDragMove;
                EventHandler<DragInProgressEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<DragInProgressEventArgs>>(ref this.mouseDragMove, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<DragStartedEventArgs> MouseDragStart
        {
            add
            {
                EventHandler<DragStartedEventArgs> eventHandler = this.mouseDragStart;
                EventHandler<DragStartedEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<DragStartedEventArgs>>(ref this.mouseDragStart, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<DragStartedEventArgs> eventHandler = this.mouseDragStart;
                EventHandler<DragStartedEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<DragStartedEventArgs>>(ref this.mouseDragStart, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<MouseClickEventArgs> MouseSingleClick
        {
            add
            {
                EventHandler<MouseClickEventArgs> eventHandler = this.mouseSingleClick;
                EventHandler<MouseClickEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<MouseClickEventArgs>>(ref this.mouseSingleClick, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<MouseClickEventArgs> eventHandler = this.mouseSingleClick;
                EventHandler<MouseClickEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<MouseClickEventArgs>>(ref this.mouseSingleClick, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<MouseClickEventArgs> MouseDoubleClick
        {
            add
            {
                EventHandler<MouseClickEventArgs> eventHandler = this.mouseDoubleClick;
                EventHandler<MouseClickEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<MouseClickEventArgs>>(ref this.mouseDoubleClick, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<MouseClickEventArgs> eventHandler = this.mouseDoubleClick;
                EventHandler<MouseClickEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<MouseClickEventArgs>>(ref this.mouseDoubleClick, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<CornerEventArgs> FoldMouseEnter
        {
            add
            {
                EventHandler<CornerEventArgs> eventHandler = this.foldMouseEnter;
                EventHandler<CornerEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<CornerEventArgs>>(ref this.foldMouseEnter, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<CornerEventArgs> eventHandler = this.foldMouseEnter;
                EventHandler<CornerEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<CornerEventArgs>>(ref this.foldMouseEnter, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<CornerEventArgs> FoldMouseLeave
        {
            add
            {
                EventHandler<CornerEventArgs> eventHandler = this.foldMouseLeave;
                EventHandler<CornerEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<CornerEventArgs>>(ref this.foldMouseLeave, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<CornerEventArgs> eventHandler = this.foldMouseLeave;
                EventHandler<CornerEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<CornerEventArgs>>(ref this.foldMouseLeave, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<CornerEventArgs> FoldMouseMove
        {
            add
            {
                EventHandler<CornerEventArgs> eventHandler = this.foldMouseMove;
                EventHandler<CornerEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<CornerEventArgs>>(ref this.foldMouseMove, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<CornerEventArgs> eventHandler = this.foldMouseMove;
                EventHandler<CornerEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<CornerEventArgs>>(ref this.foldMouseMove, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<PageMouseEventArgs> PageMouseEnter
        {
            add
            {
                EventHandler<PageMouseEventArgs> eventHandler = this.pageMouseEnter;
                EventHandler<PageMouseEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageMouseEventArgs>>(ref this.pageMouseEnter, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<PageMouseEventArgs> eventHandler = this.pageMouseEnter;
                EventHandler<PageMouseEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageMouseEventArgs>>(ref this.pageMouseEnter, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<PageMouseEventArgs> PageMouseLeave
        {
            add
            {
                EventHandler<PageMouseEventArgs> eventHandler = this.pageMouseLeave;
                EventHandler<PageMouseEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageMouseEventArgs>>(ref this.pageMouseLeave, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<PageMouseEventArgs> eventHandler = this.pageMouseLeave;
                EventHandler<PageMouseEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageMouseEventArgs>>(ref this.pageMouseLeave, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        internal event EventHandler<PageMouseEventArgs> PageMouseMove
        {
            add
            {
                EventHandler<PageMouseEventArgs> eventHandler = this.pageMouseMove;
                EventHandler<PageMouseEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageMouseEventArgs>>(ref this.pageMouseMove, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler<PageMouseEventArgs> eventHandler = this.pageMouseMove;
                EventHandler<PageMouseEventArgs> comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange<EventHandler<PageMouseEventArgs>>(ref this.pageMouseMove, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }

        static RadBookItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadBookItem), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(RadBookItem)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.RadBookItem"/> class.
        /// 
        /// </summary>
        public RadBookItem()
        {
            this.AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate)new MouseButtonEventHandler(this.RadBookItem_MouseLeftButtonDown), true);
            this.MouseMove += new MouseEventHandler(this.RadBookItem_MouseMove);
            this.MouseEnter += new MouseEventHandler(this.RadBookItem_MouseEnter);
            this.MouseLeave += new MouseEventHandler(this.RadBookItem_MouseLeave);
            this.AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate)new MouseButtonEventHandler(this.RadBookItem_MouseLeftButtonUp), true);
            this.singleClickTimer.Tick += new EventHandler(this.SingleClickTimer_Tick);
            this.doubleClickTimer.Tick += new EventHandler(this.DoubleClickTimer_Tick);
            this.LostMouseCapture += new MouseEventHandler(this.RadPage_LostMouseCapture);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.rightUnderPageShadow = this.GetTemplateChild("RightUnderPageShadow") as FrameworkElement;
            this.leftUnderPageShadow = this.GetTemplateChild("LeftUnderPageShadow") as FrameworkElement;
            this.rightBackPageShadow = this.GetTemplateChild("RightBackPageShadow") as FrameworkElement;
            this.rightBackPageShadowStatic = this.GetTemplateChild("RightBackPageShadowStatic") as FrameworkElement;
            this.leftBackPageShadow = this.GetTemplateChild("LeftBackPageShadow") as FrameworkElement;
            this.UpdateShadows();
        }

        /// <summary>
        /// Raises the <see cref="E:PageMouseEnter"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.Book.PageMouseEventArgs"/> instance containing the event data.</param>
        public virtual void OnPageMouseEnter(PageMouseEventArgs e)
        {
            if (this.pageMouseEnter == null)
                return;
            this.pageMouseEnter((object)this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:PageMouseMove"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.Book.PageMouseEventArgs"/> instance containing the event data.</param>
        public virtual void OnPageMouseMove(PageMouseEventArgs e)
        {
            if (this.pageMouseMove == null)
                return;
            this.pageMouseMove((object)this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:PageMouseLeave"/> event.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:Telerik.Windows.Controls.Book.PageMouseEventArgs"/> instance containing the event data.</param>
        public virtual void OnPageMouseLeave(PageMouseEventArgs e)
        {
            if (this.pageMouseLeave == null)
                return;
            this.pageMouseLeave((object)this, e);
        }

        internal void CancelPageTurn()
        {
            this.dragStarted = false;
        }

        internal void Reset()
        {
            this.lastEnteredCorner = FoldPosition.None;
        }

        /// <summary/>
        protected virtual void OnIsHardPaperChanged(bool oldValue, bool newValue)
        {
            if (this.ParentBook != null && !this.CancelPagesRebuild)
                this.ParentBook.BuildPages();
            this.CancelPagesRebuild = false;
            this.IsHardPaperInternal = newValue;
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
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return (AutomationPeer)new FrameworkElementAutomationPeer((FrameworkElement)this);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
        /// 
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // StyleManager.SetDefaultStyleKey((Control)this, typeof(RadBookItem));
        }

        private static FoldPosition GetOppositeCorner(FoldPosition corner)
        {
            FoldPosition foldPosition;
            switch (corner)
            {
                case FoldPosition.TopLeft:
                    foldPosition = FoldPosition.TopRight;
                    break;
                case FoldPosition.TopRight:
                    foldPosition = FoldPosition.TopLeft;
                    break;
                case FoldPosition.BottomRight:
                    foldPosition = FoldPosition.BottomLeft;
                    break;
                case FoldPosition.BottomLeft:
                    foldPosition = FoldPosition.BottomRight;
                    break;
                default:
                    foldPosition = FoldPosition.None;
                    break;
            }
            return foldPosition;
        }

        /// <summary>
        /// IsHardPaperProperty property changed handler.
        /// 
        /// </summary>
        /// <param name="d">RadBookItem that changed its IsHardPaper.</param><param name="e">Event arguments.</param>
        private static void OnIsHardPaperPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadBookItem radBookItem = d as RadBookItem;
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;
            if (radBookItem == null)
                return;
            radBookItem.OnIsHardPaperChanged(oldValue, newValue);
        }

        private void UpdateShadows()
        {
            if (this.Position != PagePosition.Left || this.rightBackPageShadowStatic == null)
                return;
            if (this.IsLastPage && this.ParentBook.FirstPagePosition == PagePosition.Right)
                this.rightBackPageShadowStatic.Visibility = Visibility.Collapsed;
            else
                this.rightBackPageShadowStatic.Visibility = Visibility.Visible;
        }

        private bool DetermineIfDragEndedOutside(Point mouse)
        {
            return this.Position == PagePosition.Left && mouse.X > this.ActualWidth || this.Position == PagePosition.Right && mouse.X < 0.0;
        }

        private void SingleClickTimer_Tick(object sender, EventArgs e)
        {
            this.singleClickMode = false;
        }

        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            this.doubleClickMode = false;
        }

        private void RadBookItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.CanBeTurned)
                return;
            this.mousePoint = e.GetPosition((IInputElement)this);
            if (!FoldHelper.MouseWithinFoldAreas(this.mousePoint, this) && this.FoldSize != new Size(0.0, 0.0))
                return;
            this.draggedCorner = FoldHelper.GetQuadrant(this.mousePoint, this);
            this.targetCorner = RadBookItem.GetOppositeCorner(this.draggedCorner);
            this.UpdatePageFold();
            if (this.doubleClickMode && this.PageFlipMode == PageFlipMode.DoubleClick)
            {
                if (this.mouseDoubleClick != null)
                    this.mouseDoubleClick((object)this, new MouseClickEventArgs(this.targetCorner));
                this.doubleClickMode = false;
                this.doubleClickTimer.Stop();
            }
            else
            {
                this.singleClickMode = this.PageFlipMode == PageFlipMode.SingleClick;
                this.doubleClickMode = this.PageFlipMode == PageFlipMode.DoubleClick;
                this.dragStarted = true;
                if (this.PageFlipMode == PageFlipMode.SingleClick)
                    this.singleClickTimer.Start();
                else if (this.PageFlipMode == PageFlipMode.DoubleClick)
                {
                    this.doubleClickTimer.Start();
                }
                else
                {
                    this.singleClickTimer.Stop();
                    this.doubleClickTimer.Stop();
                }
                if (this.mouseDragStart == null)
                    return;
                this.mouseDragStart((object)this, new DragStartedEventArgs(this.draggedCorner, this.mousePoint));
            }
        }

        private void RadBookItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.dragStarted || FoldHelper.MouseWithinFoldAreas(e.GetPosition((IInputElement)this), this) || this.ParentBook != null && this.ParentBook.ShowPageFold != PageFoldVisibility.OnPageEnter)
                return;
            this.OnPageMouseEnter(new PageMouseEventArgs(this));
        }

        private void RadBookItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.CanBeTurned)
                return;
            if (this.dragStarted)
                this.CaptureMouse();
            if (this.ParentBook.ShowPageFold == PageFoldVisibility.OnPageEnter && !FoldHelper.MouseWithinFoldAreas(e.GetPosition((IInputElement)this), this))
                this.OnPageMouseMove(new PageMouseEventArgs(this));
            this.mousePoint = e.GetPosition((IInputElement)this);
            if (this.dragStarted && this.mouseDragMove != null)
                this.mouseDragMove((object)this, new DragInProgressEventArgs(this.mousePoint));
            else
                this.UpdatePageFold();
        }

        private void RadBookItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.dragStarted || !this.CanBeTurned)
                return;
            this.OnPageMouseLeave(new PageMouseEventArgs(this));
        }

        private void RadBookItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnDragEnded();
        }

        private void RadPage_LostMouseCapture(object sender, MouseEventArgs e)
        {
            this.OnDragEnded();
        }

        private void OnDragEnded()
        {
            if (!this.CanBeTurned)
                return;
            if (this.singleClickMode && this.PageFlipMode == PageFlipMode.SingleClick)
            {
                if (this.mouseSingleClick != null)
                    this.mouseSingleClick((object)this, new MouseClickEventArgs(this.targetCorner));
                this.singleClickTimer.Stop();
            }
            else if (this.dragStarted)
            {
                bool dragEndedOutsidePage = this.DetermineIfDragEndedOutside(this.mousePoint);
                if (this.mouseDragEnd != null)
                    this.mouseDragEnd((object)this, new DragEndedEventArgs(dragEndedOutsidePage, this.targetCorner));
            }
            this.singleClickMode = false;
            this.dragStarted = false;
            this.ReleaseMouseCapture();
        }

        private void RaiseCornerEnterMove(FoldPosition corner, double offsetX, double offsetY)
        {
            Point mousePoint = new Point(this.mousePoint.X + offsetX, this.mousePoint.Y + offsetY);
            if (corner != this.lastEnteredCorner)
            {
                if (this.foldMouseEnter == null)
                    return;
                this.foldMouseEnter((object)this, new CornerEventArgs(mousePoint, corner));
            }
            else
            {
                if (this.foldMouseMove == null)
                    return;
                this.foldMouseMove((object)this, new CornerEventArgs(mousePoint, corner));
            }
        }

        private void RaiseCornerLeave(FoldPosition exitedCorner)
        {
            if (exitedCorner != this.lastEnteredCorner || this.foldMouseLeave == null)
                return;
            this.foldMouseLeave((object)this, new CornerEventArgs(this.mousePoint, exitedCorner));
        }

        private void UpdatePageFold()
        {
            if (this.Position == PagePosition.Right)
            {
                if (FoldHelper.MouseWithinTopRightFoldArea(this.mousePoint, this.FoldSize, this.ActualWidth))
                {
                    this.RaiseCornerEnterMove(FoldPosition.TopRight, 0.0, 0.0);
                    this.lastEnteredCorner = FoldPosition.TopRight;
                }
                else if (FoldHelper.MouseWithinBottomRightFoldArea(this.mousePoint, this.FoldSize, this.ActualWidth, this.ActualHeight))
                {
                    this.RaiseCornerEnterMove(FoldPosition.BottomRight, 0.0, 0.0);
                    this.lastEnteredCorner = FoldPosition.BottomRight;
                }
                else
                {
                    this.RaiseCornerLeave(FoldHelper.GetQuadrant(this.mousePoint, this));
                    this.lastEnteredCorner = FoldPosition.None;
                }
            }
            else if (FoldHelper.MouseWithinTopLeftFoldArea(this.mousePoint, this.FoldSize))
            {
                this.RaiseCornerEnterMove(FoldPosition.TopLeft, 0.0, 0.0);
                this.lastEnteredCorner = FoldPosition.TopLeft;
            }
            else if (FoldHelper.MouseWithinBottomLeftFoldArea(this.mousePoint, this.FoldSize, this.ActualHeight))
            {
                this.RaiseCornerEnterMove(FoldPosition.BottomLeft, 0.0, 0.0);
                this.lastEnteredCorner = FoldPosition.BottomLeft;
            }
            else
            {
                this.RaiseCornerLeave(FoldHelper.GetQuadrant(this.mousePoint, this));
                this.lastEnteredCorner = FoldPosition.None;
            }
        }
    }
}
