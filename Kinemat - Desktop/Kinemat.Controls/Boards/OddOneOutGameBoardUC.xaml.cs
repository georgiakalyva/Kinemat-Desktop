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
	/// Interaction logic for OddOneOutGameBoardUC.xaml
	/// </summary>
	public partial class OddOneOutGameBoardUC : UserControl
	{
		private ImageSource FirstOption;
		private ImageSource SecondOption;
		private ImageSource ThirdOption;
		private ImageSource FourthOption;
		private Storyboard soundtrack;
		private Storyboard bubbleMovement;
		private OddOneOutGame game;
		#region Constants
		private Action<object, RoutedEventArgs> action;

		#endregion

		#region Private members

		int sec = 0;

		DispatcherTimer fakeRecognition = new DispatcherTimer();

		DispatcherTimer SimonSequence = new DispatcherTimer();


		private int CurrentRegion = 0;
		private int[] sequence = new int[500];
		private int[] answers = new int[500];
		Random rand = new Random();
		private bool CanPlay = true;
		Storyboard wrong, right, swap;

		#endregion

		private int FailCount;
		private int score;
		private int oddIndex;

		ImageSource[] src = new ImageSource[4];
		DispatcherTimer FeedbackTimer = new DispatcherTimer();
		private int answer = 0;
		private int feedbacksec;
		// 0 = nothing , 1 = true , 2 = false

		private int currentActivity;
		private int expectedAnswer;

		public OddOneOutGameBoardUC()
		{
			InitializeComponent();

			this.soundtrack = (Storyboard)this.Resources["MEDIA"];
			this.bubbleMovement = (Storyboard)this.Resources["Glow1"];
			this.wrong = (Storyboard)this.Resources["WRONG"];
			this.right = (Storyboard)this.Resources["CORRECT"];
			this.swap = (Storyboard)this.Resources["SWAP"];
		}

		private void OptionClicked(object sender, RoutedEventArgs e)
		{
			// Calculates the time which needed to complete the activity
			//TimeSpan activityTime = DateTime.Now - activityStart;

			KinectCircleButton button = sender as KinectCircleButton;
			
			int givenAnswer = 0;

			if(button.Name == "img1")
				givenAnswer = 1;

			if (button.Name == "img2")
				givenAnswer = 2;

			if (button.Name == "img3")
				givenAnswer = 3;

			if (button.Name == "img4")
				givenAnswer = 4;

			// Calculate the score
			if (givenAnswer == this.expectedAnswer)
			{
				this.right.Begin();
				score += 1;
				this.GameMessage.Text = this.game.Activities[this.currentActivity].PositiveFeedback;
			}
			else
			{
				this.wrong.Begin();
				this.GameMessage.Text = this.game.Activities[this.currentActivity].NegativeFeedback;
				FailCount++;
				this.failed.Text = "Times FAILED: " + FailCount;
			}

			// this.gameStatistics.Add(new Tuple<OddOneOutActivity, TimeSpan, int>(this.Game.Activities[this.currentActivity], activityTime, givenAnswer));

			this.currentActivity++;

			RefreshGame();
		}

		private void RefreshGame()
		{
			OddOneOutActivity activity = this.currentActivity <= this.game.Activities.Length - 1 ? this.game.Activities[this.currentActivity] : null;

			// We have completed the game.
			if (activity == null)
			{
				this.soundtrack.Stop();
				this.bubbleMovement.Stop();
				action(null, null);
				return;
			}

			// 
			for (int i = 0; i < 4; i++)
			{
				if (activity[i].IsWrong)
				{
					this.expectedAnswer = i;
					break;
				}
			}

			int[] randomOrder = GenerateRandomSequence();

			this.FirstOption = activity[randomOrder[0]].Option.ImageSource;
			this.FirstOptionImg.Source = activity[randomOrder[0]].Option.ImageSource;
			this.SecondOption = activity[randomOrder[1]].Option.ImageSource;
			this.SecondOptionImg.Source = activity[randomOrder[1]].Option.ImageSource;
			this.ThirdOption = activity[randomOrder[2]].Option.ImageSource;
			this.ThirdOptionImg.Source = activity[randomOrder[2]].Option.ImageSource;
			this.FourthOption = activity[randomOrder[3]].Option.ImageSource;
			this.FourthOptionImg.Source = activity[randomOrder[3]].Option.ImageSource;
			// this.activityStart = DateTime.Now;
		}

		/// <summary>
		/// Generates random positions for the activity images.
		/// </summary>
		/// <returns>An array which contains the random order of the images.</returns>
		private int[] GenerateRandomSequence()
		{
			int[] randomSequence = new int[4];
			Random random = new Random();
			List<int> randomNumberList = new List<int>();
			randomNumberList.AddRange(new int[] { 0, 1, 2, 3 });

			for (int i = 0; i < 4; i++)
			{
				int index = random.Next(0, randomNumberList.Count - 1);
				randomSequence[i] = randomNumberList[index];
				randomNumberList.RemoveAt(index);
			}

			return randomSequence;
		}

		public void Start(OddOneOutGame game, Action<object, RoutedEventArgs> action)
		{
			this.game = game;
			this.action = action;
			this.soundtrack.Begin();
			this.bubbleMovement.Begin();

			this.RefreshGame();
		}

		private void UserControl_KeyUp(object sender, KeyEventArgs e)
		{

		}
	}
}