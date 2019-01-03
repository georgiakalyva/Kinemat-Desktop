using Kinemat.Viewer.Utilities;
using Kinemat.Viewer.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kinemat.Viewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Private members

		/// <summary>
		/// Mouse movement detector.
		/// </summary>
		private readonly MouseMovementDetector movementDetector;

		/// <summary>
		/// Controller used as ViewModel for this window.
		/// </summary>
		private readonly KinectController controller;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		/// <param name="controller"></param>
		public MainWindow(KinectController controller) : 
			this()
		{
			if (controller == null)
				throw new ArgumentNullException("controller", Properties.Resources.KinectControllerInvalid);
			
			this.controller = controller;

			controller.EngagedUserColor = (Color)this.Resources["EngagedUserColor"];
			controller.TrackedUserColor = (Color)this.Resources["TrackedUserColor"];
			controller.EngagedUserMessageBrush = (Brush)this.Resources["EngagedUserMessageBrush"];
			controller.TrackedUserMessageBrush = (Brush)this.Resources["TrackedUserMessageBrush"];

			this.kinectRegion.HandPointersUpdated += (sender, args) => controller.OnHandPointersUpdated(this.kinectRegion.HandPointers);

			this.DataContext = controller;

			this.movementDetector = new MouseMovementDetector(this);
			this.movementDetector.IsMovingChanged += this.OnIsMouseMovingChanged;
			this.movementDetector.Start();
		}
		#endregion

		/// <summary>
		/// Handles Window.Loaded event, and prompts user if screen resolution does not meet
		/// minimal requirements.
		/// </summary>
		/// <param name="sender">
		/// Object that sent the event.
		/// </param>
		/// <param name="e">
		/// Event arguments.
		/// </param>
		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			// get the main screen size
			double height = SystemParameters.PrimaryScreenHeight;
			double width = SystemParameters.PrimaryScreenWidth;
		}

		/// <summary>
		/// Handles MouseMovementDetector.IsMovingChanged event and shows/hides the window bezel,
		/// as appropriate.
		/// </summary>
		/// <param name="sender">
		/// Object that sent the event.
		/// </param>
		/// <param name="e">
		/// Event arguments.
		/// </param>
		private void OnIsMouseMovingChanged(object sender, EventArgs e)
		{
			WindowBezelHelper.UpdateBezel(this, this.movementDetector.IsMoving);
			this.controller.IsInEngagementOverrideMode = this.movementDetector.IsMoving;
		}

		private void KeyRelease(object sender, KeyEventArgs e) 
		{
			// Gets the data context.
			KinectController model = DataContext as KinectController;

			IPausable viewModel = model.NavigationManager.CurrentNavigationContext as IPausable;

			// The current view model is not IPausable
			if (viewModel == null)
				return;

			if (e.Key == Key.Space)
			{
				if (viewModel.IsPaused)
					viewModel.Resume();
				else
					viewModel.Pause();
			}

			if (e.Key == Key.Escape)
			{
				model.NavigationManager.GoBack();
			}
		}

	}
}
