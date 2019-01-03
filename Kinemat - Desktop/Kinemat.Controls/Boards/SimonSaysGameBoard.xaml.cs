using Kinemat.Models.Games;
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

namespace Kinemat.Controls.Boards
{
	/// <summary>
	/// Interaction logic for SimonSaysGameBoard.xaml
	/// </summary>
	public partial class SimonSaysGameBoard : UserControl
	{
		int sec = 0;

		DispatcherTimer fakeRecognition = new DispatcherTimer();

		DispatcherTimer SimonSequence = new DispatcherTimer();
		DispatcherTimer Timer = new DispatcherTimer();

		private int SimonIndex = 0;
		private int CurrentRegion = 0;
		private int index = 0;
		private int TimerDifficulty = 2;
		private int maxIndex = 0;
		private int TimerMax;
		private int secRemaining;
		private int[] sequence = new int[500];
		private int[] answers = new int[500];
		Random rand = new Random();
		private bool CanPlay = false;
		Storyboard tickEasy, tickMedium, tickHard, Yellow, Red, Green, Blue, msg, wrong;
		private int FailCount;
		private int score;
		private SimonSaysGame game;
		private Action<object, RoutedEventArgs> action;

		public SimonSaysGameBoard()
		{
			InitializeComponent();

			KinectRegion.AddHandPointerEnterHandler(RECT1, SetHandEntersHotspot1);
			KinectRegion.AddHandPointerLeaveHandler(RECT1, SetHandLeavesHotspot1);

			KinectRegion.AddHandPointerEnterHandler(RECT2, SetHandEntersHotspot2);
			KinectRegion.AddHandPointerLeaveHandler(RECT2, SetHandLeavesHotspot2);

			KinectRegion.AddHandPointerEnterHandler(RECT3, SetHandEntersHotspot3);
			KinectRegion.AddHandPointerLeaveHandler(RECT3, SetHandLeavesHotspot3);

			KinectRegion.AddHandPointerEnterHandler(RECT4, SetHandEntersHotspot4);
			KinectRegion.AddHandPointerLeaveHandler(RECT4, SetHandLeavesHotspot4);
		}

		void Timer_Tick(object sender, EventArgs e)
		{
			secRemaining--;
			timetxt.Text = secRemaining.ToString();

			if (secRemaining == -1)
			{
				tickHard.Stop();
				tickEasy.Stop();
				tickMedium.Stop();
				Timer.Stop();
				Time.EndAngle = 360;
				secRemaining = TimerMax;
				timetxt.Text = secRemaining.ToString();
				FailCount++;
				wrong.Begin();

				failed.Text = "Times FAILED: " + FailCount;
				ProgressSequence(false);
			}
		}

		void SimonSequence_Tick(object sender, EventArgs e)
		{
			fire7_wav.IsMuted = false;
			ice03_wav.IsMuted = false;
			bolt06_wav.IsMuted = false;
			earth04_wav.IsMuted = false;

			if (sequence[SimonIndex] == 3)
			{
				Yellow.Begin();
			}
			if (sequence[SimonIndex] == 2)
			{
				Green.Begin();
			}
			if (sequence[SimonIndex] == 1)
			{
				Red.Begin();
			}
			if (sequence[SimonIndex] == 0)
			{
				Blue.Begin();
			}

			SimonIndex++;
			if (SimonIndex == maxIndex + 1)
			{
				SimonSequence.Stop();
				CanPlay = true;
				msg.Stop();
				MSG.Text = "GIVE IT A GO!";
				secRemaining = TimerMax;
				timetxt.Text = secRemaining.ToString();
				Timer.Start();
				if(this.game.Difficulty == SimonSaysDifficulty.Hard)
				//if (TimerDifficulty == 3)
					tickHard.Begin();
				if(this.game.Difficulty == SimonSaysDifficulty.Normal)
				//if (TimerDifficulty == 2)
					tickMedium.Begin();
				if(this.game.Difficulty == SimonSaysDifficulty.Ease)
				//if (TimerDifficulty == 1)
					tickEasy.Begin();
			}
		}

		#region Event handlers

		private void GameWindowLoaded(object sender, RoutedEventArgs e)
		{
			Yellow = (Storyboard)Resources["SimonYellow"];
			Blue = (Storyboard)Resources["SimonBlue"];
			Red = (Storyboard)Resources["SimonRed"];
			Green = (Storyboard)Resources["SimonGreen"];
			msg = (Storyboard)Resources["MessageBlink"];
			wrong = (Storyboard)Resources["WRONG"];

			tickEasy = (Storyboard)Resources["Timer1"];
			tickMedium = (Storyboard)Resources["Timer2"];
			tickHard = (Storyboard)Resources["Timer3"];

			msg.Begin();
		}

		private void GameWindowContentRendered(object sender, EventArgs e)
		{
			// Set the hot spot regions
			KinectRegion.AddHandPointerEnterHandler(RECT1, SetHandEntersHotspot1);
			KinectRegion.AddHandPointerLeaveHandler(RECT1, SetHandLeavesHotspot1);

			KinectRegion.AddHandPointerEnterHandler(RECT2, SetHandEntersHotspot2);
			KinectRegion.AddHandPointerLeaveHandler(RECT2, SetHandLeavesHotspot2);

			KinectRegion.AddHandPointerEnterHandler(RECT3, SetHandEntersHotspot3);
			KinectRegion.AddHandPointerLeaveHandler(RECT3, SetHandLeavesHotspot3);

			KinectRegion.AddHandPointerEnterHandler(RECT4, SetHandEntersHotspot4);
			KinectRegion.AddHandPointerLeaveHandler(RECT4, SetHandLeavesHotspot4);
		}

		private void SetHandEntersHotspot4(object sender, HandPointerEventArgs e)
		{
			if (CanPlay)
			{
				CurrentRegion = 3;
				fakeRecognition.Start();
			}
		}

		private void SetHandLeavesHotspot4(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = -1;
			sec = 0;
			fakeRecognition.Stop();
		}

		private void SetHandLeavesHotspot3(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = -1;
			sec = 0;
			fakeRecognition.Stop();
		}

		private void SetHandEntersHotspot3(object sender, HandPointerEventArgs e)
		{
			if (CanPlay)
			{
				CurrentRegion = 2;
				fakeRecognition.Start();
			}
		}

		private void SetHandLeavesHotspot2(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = -1;
			sec = 0;
			fakeRecognition.Stop();
		}

		private void SetHandEntersHotspot2(object sender, HandPointerEventArgs e)
		{
			if (CanPlay)
			{
				CurrentRegion = 1;
				fakeRecognition.Start();
			}
		}

		private void SetHandLeavesHotspot1(object sender, HandPointerEventArgs e)
		{
			CurrentRegion = -1;
			sec = 0;
			fakeRecognition.Stop();
		}

		private void SetHandEntersHotspot1(object sender, HandPointerEventArgs e)
		{
			if (CanPlay)
			{
				CurrentRegion = 0;
				fakeRecognition.Start();
			}
		}

		private void AnswerCountdown(object sender, EventArgs e)
		{
			sec++;
			if (sec == 3)
			{
				if (CurrentRegion == sequence[index])
				{
					sec = 0;
					//Swsto
					if (CurrentRegion == 3)
					{
						Yellow.Begin();
					}
					if (CurrentRegion == 2)
					{
						Green.Begin();
					}
					if (CurrentRegion == 1)
					{
						Red.Begin();
					}
					if (CurrentRegion == 0)
					{
						Blue.Begin();
					}

					tickHard.Stop();
					tickEasy.Stop();
					tickMedium.Stop();
					Timer.Stop();
					Time.EndAngle = 360;
					secRemaining = TimerMax;
					timetxt.Text = secRemaining.ToString();


					if (index == maxIndex)
					{
						Sequence.Text = "Sequence Typed: ";
						score++;
						if (score < 10)
							ScoreText.Text = "0" + score;
						else
							ScoreText.Text = score.ToString();

						ProgressSequence(true);
					}
					else
					{
						Timer.Start();
						if (TimerDifficulty == 3)
							tickHard.Begin();
						if (TimerDifficulty == 2)
							tickMedium.Begin();
						if (TimerDifficulty == 1)
							tickEasy.Begin();
						index++;
						Sequence.Text = "Sequence Typed: ";
						for (int i = 0; i < index; i++)
						{
							Sequence.Text += sequence[i] + " ";
						}
					}
				}
				else
				{
					FailCount++;
					wrong.Begin();
					//Lathos
					failed.Text = "Times FAILED: " + FailCount;
					ProgressSequence(false);
				}
			}
		}

		private void ProgressSequence(bool right)
		{
			if (right)
			{
				maxIndex++;
				sequence[maxIndex] = rand.Next(0, 4);

				index = 0;

				sequence_Required.Text = "Sequence Required: ";
				for (int i = 0; i < maxIndex + 1; i++)
				{
					sequence_Required.Text += sequence[i] + " ";
				}
			}
			else
			{
				maxIndex = 0;
				index = 0;

				sequence[0] = rand.Next(0, 4);
				sequence_Required.Text = "Sequence Required: " + sequence[0];
			}
			MSG.Text = "CONCENTRATE...";
			msg.Begin();
			SimonIndex = 0;
			SimonSequence.Start();
			CanPlay = false;
		}
		#endregion

		public void Start(SimonSaysGame game, Action<object, RoutedEventArgs> action)
		{
			this.game = game;
			this.action = action;

			Yellow = (Storyboard)Resources["SimonYellow"];
			Blue = (Storyboard)Resources["SimonBlue"];
			Red = (Storyboard)Resources["SimonRed"];
			Green = (Storyboard)Resources["SimonGreen"];
			msg = (Storyboard)Resources["MessageBlink"];
			wrong = (Storyboard)Resources["WRONG"];

			tickEasy = (Storyboard)Resources["Timer1"];
			tickMedium = (Storyboard)Resources["Timer2"];
			tickHard = (Storyboard)Resources["Timer3"];

			msg.Begin();

			if(game.Difficulty == SimonSaysDifficulty.Hard)
			//if (TimerDifficulty == 3)
				TimerMax = 5;
			if(game.Difficulty == SimonSaysDifficulty.Normal)
			//if (TimerDifficulty == 2)
				TimerMax = 10;
			if(game.Difficulty == SimonSaysDifficulty.Ease)
			//if (TimerDifficulty == 1)
				TimerMax = 15;

			Timer.Tick += Timer_Tick;
			Timer.Interval = TimeSpan.FromSeconds(1);

			fakeRecognition.Interval = TimeSpan.FromMilliseconds(500);
			fakeRecognition.Tick += AnswerCountdown;

			SimonSequence.Tick += SimonSequence_Tick;
			SimonSequence.Interval = TimeSpan.FromMilliseconds(800);

			sequence[maxIndex] = rand.Next(0, 4);
			sequence_Required.Text = "Sequence Required: " + sequence[maxIndex];

			SimonSequence.Start();
		}
	}
}
