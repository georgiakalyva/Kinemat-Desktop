using Microsoft.Kinect.Toolkit.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using System.Linq;
using System;
using System.Windows.Threading;
using Telerik.Windows.Controls.Book;

namespace Kinemat.Controls
{
	/// <summary>
	/// Wrapper class that adds kinect interaction capabilities to the Telerik RadBook control.
	/// </summary>
	public class KinectRadBook : RadBook
	{
		#region Private members

		private static readonly bool IsInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
		private bool isCoverPageVisible = true;
		private DispatcherTimer turnPageHindTimer = new DispatcherTimer();
		private NavigationDirection currentNavigationDirection;
		private DispatcherTimer unblockPageNagivationTimer = new DispatcherTimer();
		private bool blockPageNavigation = false;

		#endregion

		#region Dependency properties

		/// <summary>
		/// Specifies if we can go to the next book page.
		/// </summary>
		public readonly static DependencyProperty CanNavigateToNextPageProperty = DependencyProperty.Register(
			"CanNavigateToNextPage", typeof(bool), typeof(KinectRadBook), new PropertyMetadata(true));

		/// <summary>
		/// Specifies if we can go to the previous book page.
		/// </summary>
		public readonly static DependencyProperty CanNavigateToPreviousPageProperty = DependencyProperty.Register(
			"CanNavigateToPreviousPage", typeof(bool), typeof(KinectRadBook), new PropertyMetadata(true));


		public readonly static RoutedEvent PageTurnedEvent = EventManager.RegisterRoutedEvent(
				"PageTurned", RoutingStrategy.Bubble, typeof(EventHandler<PageTurnEventArgs>), typeof(KinectRadBook));

		#endregion

		#region Public events

		public event EventHandler<PageTurnEventArgs> PageTurned
		{
			add { this.AddHandler(PageTurnedEvent, value); }
			remove { this.RemoveHandler(PageTurnedEvent, value); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Kinemat.Controls.KinectRadBook"/> class.
		/// </summary>
		public KinectRadBook()
		{
			if (!IsInDesignMode)
			{
				InitializeKinectRadBook();

				// Register the event handlers
				Loaded += KinectRadBookLoaded;
				Unloaded += KinectRadBookUnloaded;

				unblockPageNagivationTimer.Interval = TimeSpan.FromSeconds(3);
				unblockPageNagivationTimer.Tick += UnblockPageTurn;
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Initializes the core navigation event handlers.
		/// </summary>
		private void InitializeKinectRadBook()
		{
			KinectRegion.AddHandPointerSwipeHandler(this, HandPointerSwipeHandler);
			this.RightPageIndex = 5;
		}

		private void UnblockPageTurn(object sender, EventArgs e)
		{
			this.blockPageNavigation = false;
			this.unblockPageNagivationTimer.Stop();
		}

		/// <summary>
		/// Navigates to the next book page.
		/// </summary>
		private void NavigateToNextPage()
		{
			if (this.blockPageNavigation)
				return;

			if (this.isCoverPageVisible)
			{
				this.isCoverPageVisible = !this.isCoverPageVisible;
			}
			if (CanNavigateToNextPage)
			{
				this.currentNavigationDirection = NavigationDirection.Forward;
				this.NextPage();

				// Abort page navigation for three seconds.
				this.BlockPageNavigation();
			}
		}

		/// <summary>
		/// Navigates to the previous book page.
		/// </summary>
		private void NavigateToPreviousPage()
		{
			if (this.blockPageNavigation)
				return;

			if (CanNavigateToPreviousPage)
			{
				this.currentNavigationDirection = NavigationDirection.Backward;
				this.PreviousPage();

				// Abort page navigation
				this.BlockPageNavigation();
			}
		}

		private void RegisterRadBookItemHandlers(RadBookItem[] radBookItems)
		{
			if (radBookItems == null)
				throw new ArgumentNullException("radBookItems");

			foreach (RadBookItem item in radBookItems)
			{
				KinectRegion.AddHandPointerEnterHandler(item, OnHandPointerEnterPage);
				KinectRegion.AddHandPointerLeaveHandler(item, OnHandPointLeavePageRegion);
			}
		}

		private void UnregisterRadBookItemHandlers(RadBookItem[] radBookItems)
		{
			if (radBookItems == null)
				throw new ArgumentException("radBookItems");

			foreach (RadBookItem item in radBookItems)
			{
				KinectRegion.RemoveHandPointerEnterHandler(item, OnHandPointerEnterPage);
				KinectRegion.RemoveHandPointerLeaveHandler(item, OnHandPointLeavePageRegion);
			}
		}

		private void BlockPageNavigation()
		{
			this.blockPageNavigation = true;
			this.unblockPageNagivationTimer.Start();
		}

		#endregion

		#region Event handlers

		private void HandPointerSwipeHandler(object sender, HandPointerSwipeEventArgs e)
		{
			if (e.SwipeGesture.SwipeDirection == SwipeDirection.Left)
				NavigateToNextPage();
			else
				NavigateToPreviousPage();
		}

		private void OnHandPointerEnterPage(object sender, HandPointerEventArgs e)
		{
			UIElement uiElement = sender as UIElement;
			MouseEventArgs mouseEnterEventArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);
			mouseEnterEventArgs.RoutedEvent = Mouse.MouseEnterEvent;
			uiElement.RaiseEvent(mouseEnterEventArgs);
		}

		private void OnHandPointLeavePageRegion(object sender, HandPointerEventArgs e)
		{
			UIElement uiElement = sender as UIElement;
			MouseEventArgs mouseLeaveEventArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);
			mouseLeaveEventArgs.RoutedEvent = Mouse.MouseLeaveEvent;
			uiElement.RaiseEvent(mouseLeaveEventArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void KinectRadBookLoaded(object sender, RoutedEventArgs e)
		{
			RadBookItem[] radBookItems = this.ChildrenOfType<RadBookItem>().ToArray();

			// RegisterRadBookItemHandlers(radBookItems);
		}

		private void KinectRadBookUnloaded(object sender, RoutedEventArgs e)
		{
			RadBookItem[] radBookItems = this.ChildrenOfType<RadBookItem>().ToArray();

			UnregisterRadBookItemHandlers(radBookItems);
		}

		protected override void OnPageFlipEnded(PageFlipEventArgs e)
		{
			base.OnPageFlipEnded(e);

			PageTurnEventArgs kinectRadBookPageTurnEventArgs =
				new PageTurnEventArgs(this, e.Page, this.currentNavigationDirection, KinectRadBook.PageTurnedEvent);

			RaiseEvent(kinectRadBookPageTurnEventArgs);
		}

		protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets or set if we can go to the next book page.
		/// </summary>
		public bool CanNavigateToNextPage
		{
			get { return (bool)GetValue(CanNavigateToNextPageProperty); }
			set { SetValue(CanNavigateToNextPageProperty, value); }
		}

		/// <summary>
		/// Gets or sets if we can go to the previous book page.
		/// </summary>
		public bool CanNavigateToPreviousPage
		{
			get { return (bool)GetValue(CanNavigateToPreviousPageProperty); }
			set { SetValue(CanNavigateToPreviousPageProperty, value); }
		}

		#endregion
	}
}
