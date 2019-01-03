using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using Kinemat.Models.Games;

namespace Kinemat.Controls.Boards
{
	/// <summary>
	/// Interaction logic for GoNoGoGameBoard.xaml
	/// </summary>
	public partial class GoNoGoGameBoard : UserControl
	{
		#region Constants

		private const string MarkerStorageFileName = "markers.bin";

		#endregion

		#region Private members

		//int sec = 0;

		DispatcherTimer fakeRecognition = new DispatcherTimer();

		DispatcherTimer SimonSequence = new DispatcherTimer();

		private int CurrentRegion = 0;

		ImageSource[] src = new ImageSource[4];

		private int[] sequence = new int[500];
		private bool CanPlay = false;

		private GoNoGoGame game;
		private Action<object, RoutedEventArgs> action;

		Random rand = new Random();

		Storyboard come, go, Time1, Time2, Time3, TIMESTORY;


		// STUFF FROM EDITOR
		int Difficulty = 3;
		int ImageSet = 4;
		private int CorrectIndex = 1;

		int sec;
		int Maxsec;

		#endregion

		private int WrongCount;
		private int CorrectCount;

		DispatcherTimer Time = new DispatcherTimer();
		DispatcherTimer TotalTime = new DispatcherTimer();
		private int TotalSeconds = 60;
		MediaPlayer OST;
		private int index = -1;

		private int seconds;

		public GoNoGoGameBoard()
		{
			InitializeComponent();
		}


		#region Event handlers

		private void GameWindowLoaded(object sender, RoutedEventArgs e)
		{
			go = (Storyboard)Resources["CardAnimation"];
			come = (Storyboard)Resources["CardIncoming"];
			Time1 = (Storyboard)Resources["TimeRemaining1"];
			Time2 = (Storyboard)Resources["TimeRemaining2"];
			Time3 = (Storyboard)Resources["TimeRemaining3"];
			TIMESTORY = (Storyboard)Resources["GameTime"];



			//msg = (Storyboard)Resources["MessageBlink"];
			// msg.Begin();
			// fly1.Stop();
		}

		private void GameWindowContentRendered(object sender, EventArgs e)
		{
			// Set the hot spot regions
			KinectRegion.AddHandPointerEnterHandler(Tick, SetHandEntersHotspot1);
			KinectRegion.AddHandPointerLeaveHandler(Tick, SetHandLeavesHotspot1);

			KinectRegion.AddHandPointerEnterHandler(PLAYBTN, PLAYBTNENTER);
			KinectRegion.AddHandPointerLeaveHandler(PLAYBTN, PLAYBTNLEAVE);
		}

		private void PLAYBTNENTER(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = 5;
			fakeRecognition.Start();
		}

		private void PLAYBTNLEAVE(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = -1;
			fakeRecognition.Stop();
		}

		private void SetHandLeavesHotspot1(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = -1;
			seconds = 0;
			fakeRecognition.Stop();
		}

		private void SetHandEntersHotspot1(object sender, HandPointerEventArgs e)
		{
			if (CanPlay)
			{
				if (index == CorrectIndex)
				{
					MediaPlayer wowSound = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name wowSound
					wowSound.Open(new Uri(System.IO.Path.GetFullPath("choice2.wav"))); //Open the file for a media playback
					wowSound.Play(); //Play the media
					CorrectCount++;

					if (cardimg.Visibility == Visibility.Collapsed)
					{
						cardimg.Visibility = cardrectangle.Visibility = Visibility.Visible;
						index = rand.Next(0, src.Length);
						cardimg.Source = src[index];
						come.Begin();
					}
					else
					{
						go.Begin();
						index = rand.Next(0, src.Length);
					}
					Tick.Visibility = Visibility.Collapsed;
					CanPlay = false;
					Time.Stop();
				}
				else
				{
					MediaPlayer wowSound = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name wowSound
					wowSound.Open(new Uri(System.IO.Path.GetFullPath("failure1.wav"))); //Open the file for a media playback
					wowSound.Play(); //Play the media
					WrongCount++;
					strikes.Text = "Times FAILED: " + WrongCount;

					if (cardimg.Visibility == Visibility.Collapsed)
					{
						cardimg.Visibility = cardrectangle.Visibility = Visibility.Visible;
						index = rand.Next(0, src.Length);
						cardimg.Source = src[index];
						come.Begin();
					}
					else
					{
						go.Begin();
						index = rand.Next(0, src.Length);
					}
					Tick.Visibility = Visibility.Collapsed;
					CanPlay = false;
					Time.Stop();
				}
			}
		}

		private void AnswerCountdown(object sender, EventArgs e)
		{
			seconds++;
			if (seconds == 1)
			{
				if (CurrentRegion == 5)
				{

					MSG.Visibility = strikes.Visibility = Tick.Visibility = Visibility.Visible;
					PLAYBTN.Visibility = Visibility.Collapsed;

					if (cardimg.Visibility == Visibility.Collapsed)
					{
						TIMESTORY.Begin();
						TotalTime.Start();
						AllBoutTime.Visibility = Visibility.Visible;
						cardimg.Visibility = cardrectangle.Visibility = Visibility.Visible;
						index = rand.Next(0, src.Length);
						cardimg.Source = src[index];
						come.Begin();
					}

					return;
				}
			}
		}

		#endregion

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (cardimg.Visibility == Visibility.Collapsed)
			{
				cardimg.Visibility = cardrectangle.Visibility = Visibility.Visible;
				index = rand.Next(0, src.Length);
				cardimg.Source = src[index];
				come.Begin();
			}
			else
			{

				go.Begin();
				index = rand.Next(0, src.Length);
			}

		}

		public void Start(GoNoGoGame game, Action<object, RoutedEventArgs> action)
		{
			this.game = game;
			this.action = action;

			this.src[0] = this.game.Activities.First()[0].Option.ImageSource;

			if (!this.game.Activities.First()[0].IsWrong)
				this.CorrectIndex = 0;
			this.src[1] = this.game.Activities.First()[1].Option.ImageSource;

			if (!this.game.Activities.First()[1].IsWrong)
				this.CorrectIndex = 1;


			this.src[2] = this.game.Activities.First()[2].Option.ImageSource;
			if (!this.game.Activities.First()[2].IsWrong)
				this.CorrectIndex = 2;
			
			this.src[3] = this.game.Activities.First()[3].Option.ImageSource;
			if (!this.game.Activities.First()[3].IsWrong)
				this.CorrectIndex = 3;
		}

		private void Storyboard_Completed_1(object sender, EventArgs e)
		{

			cardimg.Source = src[index];
			come.Begin();
		}

		private void PLAYBTN_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{

			CanPlay = true;
			MSG.Visibility = strikes.Visibility = Visibility.Visible;
			PLAYBTN.Visibility = Visibility.Collapsed;

			Time.Start();
		}

		private void Storyboard_Completed(object sender, EventArgs e)
		{
			Tick.Visibility = Visibility.Visible;
			CanPlay = true;
			sec = Maxsec;
			//SEC.Text = "0" + (Maxsec / 10).ToString();
			Time.Start();
		}
	}
}
