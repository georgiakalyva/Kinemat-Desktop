using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Kinemat.Controls.Boards
{
	/// <summary>
	/// Interaction logic for OddOneOutGameBoard.xaml
	/// </summary>
	public partial class OddOneOutGameBoard : UserControl
	{
		#region Constants

		private const string DefaultNegativeFeedback = "Times FAILED: 0";
		private const string DefaultMessage = "PICK THE ODD ONE OUT!!";
		private const string DefaultScoreFeedback = "00";

		#endregion

		#region Dependency properties

		public readonly static DependencyProperty FeedbackProperty = DependencyProperty.Register(
			"Feedback", typeof(string), typeof(OddOneOutGameBoard), new PropertyMetadata(DefaultMessage));

		public readonly static DependencyProperty ScoreProperty = DependencyProperty.Register(
			"Score", typeof(string), typeof(OddOneOutGameBoard), new PropertyMetadata(DefaultScoreFeedback));

		public readonly static DependencyProperty FirstOptionProperty = DependencyProperty.Register(
			"FirstOption", typeof(ImageSource), typeof(OddOneOutGameBoard), new PropertyMetadata(null));

		public readonly static DependencyProperty SecondOptionProperty = DependencyProperty.Register(
			"SecondOption", typeof(ImageSource), typeof(OddOneOutGameBoard), new PropertyMetadata(null));

		public readonly static DependencyProperty ThirdOptionProperty = DependencyProperty.Register(
			"ThirdOption", typeof(ImageSource), typeof(OddOneOutGameBoard), new PropertyMetadata(null));

		public readonly static DependencyProperty FourthOptionProperty = DependencyProperty.Register(
			"FourthOption", typeof(ImageSource), typeof(OddOneOutGameBoard), new PropertyMetadata(null));

		public readonly static DependencyProperty IsPausedProperty = DependencyProperty.Register(
			"IsPaused", typeof(bool), typeof(OddOneOutGameBoard), new PropertyMetadata(OnIsPausedChanged));

		public readonly static RoutedEvent OddItemSelectedEvent = EventManager.RegisterRoutedEvent(
			"OddItemSelected", RoutingStrategy.Bubble, typeof(EventHandler<OddItemSelectedEventArgs>), typeof(OddOneOutGameBoard));

		#endregion

		#region Callbacks

		private static void OnIsPausedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Cast the source object
			OddOneOutGameBoard sender = (OddOneOutGameBoard)d;

			// Cast dependency property values
			bool oldValue = (bool)e.OldValue;
			bool newValue = (bool)e.NewValue;

			// If the object notified but the value didnt changed return.
			if (oldValue == newValue)
				return;

			if (newValue)
			{
				// Stop all storyboards
				sender.soundtrack.Pause();
				sender.bubbleMovement.Pause();
			}
			else
			{
				sender.soundtrack.Resume();
				sender.bubbleMovement.Resume();
			}
		}

		#endregion

		#region Private members

		private Storyboard soundtrack;
		private Storyboard bubbleMovement;

		#endregion

		#region Public events

		public event EventHandler<OddItemSelectedEventArgs> OddItemSelected
		{
			add { this.AddHandler(OddItemSelectedEvent, value); }
			remove { this.RemoveHandler(OddItemSelectedEvent, value); }
		}

		#endregion

		#region Property wrappers

		public string Feedback
		{
			get { return (string)GetValue(FeedbackProperty); }
			set { SetValue(FeedbackProperty, value); }
		}

		public string Score
		{
			get { return (string)GetValue(ScoreProperty); }
			set { SetValue(ScoreProperty, value); }
		}

		public ImageSource FirstOption
		{
			get { return (ImageSource)GetValue(FirstOptionProperty); }
			set { SetValue(FirstOptionProperty, value); }
		}

		public ImageSource SecondOption
		{
			get { return (ImageSource)GetValue(SecondOptionProperty); }
			set { SetValue(SecondOptionProperty, value); }
		}

		public ImageSource ThirdOption
		{
			get { return (ImageSource)GetValue(ThirdOptionProperty); }
			set { SetValue(ThirdOptionProperty, value); }
		}

		public ImageSource FourthOption
		{
			get { return (ImageSource)GetValue(FourthOptionProperty); }
			set { SetValue(FourthOptionProperty, value); }
		}

		public bool IsPaused
		{
			get { return (bool)GetValue(IsPausedProperty); }
			set { SetValue(IsPausedProperty, value); }
		}

		#endregion

		public OddOneOutGameBoard()
		{
			InitializeComponent();

			this.soundtrack = (Storyboard)this.Resources["MEDIA"];
			this.bubbleMovement = (Storyboard)this.Resources["Glow1"];
			this.soundtrack.Begin();
			this.bubbleMovement.Begin();
		}

		protected virtual void OnOddItemSelected(OddItemSelectedEventArgs e)
		{
			this.RaiseEvent(e);
		}

		private void OptionClicked(object sender, RoutedEventArgs e)
		{
			KinectCircleButton circleButton = sender as KinectCircleButton;
			string name = circleButton.Name;
			int selectedIndex;

			switch (name)
			{
				case "img1":
					selectedIndex = 0;
					break;
				case "img2":
					selectedIndex = 1;
					break;
				case "img3":
					selectedIndex = 2;
					break;
				case "img4":
					selectedIndex = 3;
					break;
				default:
					selectedIndex = 0;
					break;
			}

			OddItemSelectedEventArgs eventArgs = new OddItemSelectedEventArgs(sender, selectedIndex, OddItemSelectedEvent);
			this.RaiseEvent(eventArgs);
		}
	}
}
