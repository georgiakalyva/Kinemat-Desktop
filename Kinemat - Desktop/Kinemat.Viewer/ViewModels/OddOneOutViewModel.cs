using Kinemat.Controls.Boards;
using Kinemat.Core;
using Kinemat.Models;
using Kinemat.Models.Games;
using Kinemat.Viewer.Navigation;
using Microsoft.Kinect.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kinemat.Viewer.ViewModels
{
	[ExportNavigable(NavigableContextName = NavigableContexts.OddOneOutActivity)]
	public class OddOneOutViewModel : ViewModelBase, IPausable
	{
		#region Constants

		private const string DefaultGameMessage = "PICK THE ODD ONE OUT!";
		private const string DefaultFailureMessage = "Times FAILED: {0}";

		#endregion

		private OddOneOutGame game;
		private string feedback;

		private string failureMessage;
		private int score;
		private ImageSource firstOption;
		private ImageSource secondOption;
		private ImageSource thirdOption;
		private ImageSource fourthOption;
		private ImageSource background;
		private TimeSpan[] timeStatistics;
		private int expectedAnswer;
		private DateTime activityStart;
		private int currentActivity;
		private int failures = 0;
		private List<Tuple<OddOneOutActivity,TimeSpan,int>> gameStatistics;

		/// <summary>
		/// Indicates if the game is paused.
		/// </summary>
		private bool isPaused;

		// private readonly RelayCommand validateAnswerCommand;
		private readonly RelayCommand<OddItemSelectedEventArgs> validateAnswerCommand;

		#region Public properties

		/// <summary>
		/// Gets the active interactive game.
		/// </summary>
		public OddOneOutGame Game
		{
			get { return game; }
			private set
			{
				if (game != value)
				{
					game = value;
					OnPropertyChanged("Game");
				}
			}
		}

		/// <summary>
		/// Gets the description of the game.
		/// </summary>
		public string Feedback
		{
			get { return feedback; }
			private set
			{
				if (feedback != value)
				{
					feedback = value;
					OnPropertyChanged("Feedback");
				}
			}
		}

		public string FailureMessage
		{
			get { return failureMessage; }
			private set
			{
				if (failureMessage != value)
				{
					failureMessage = value;
					OnPropertyChanged("FailureMessage");
				}
			}
		}

		/// <summary>
		/// Gets the game score.
		/// </summary>
		public int Score
		{
			get { return score; }
			private set
			{
				if (score != value)
				{
					score = value;
					OnPropertyChanged("Score");
				}
			}
		}

		/// <summary>
		/// Gets the first game option.
		/// </summary>
		public ImageSource FirstOption
		{
			get { return firstOption; }
			private set
			{
				if (firstOption != value)
				{
					firstOption = value;
					OnPropertyChanged("FirstOption");
				}
			}
		}

		/// <summary>
		/// Gets the second game option.
		/// </summary>
		public ImageSource SecondOption
		{
			get { return secondOption; }
			private set
			{
				if (secondOption != value)
				{
					secondOption = value;
					OnPropertyChanged("SecondOption");
				}
			}
		}

		/// <summary>
		/// Gets the third game option.
		/// </summary>
		public ImageSource ThirdOption
		{
			get { return thirdOption; }
			private set
			{
				if (thirdOption != value)
				{
					thirdOption = value;
					OnPropertyChanged("ThirdOption");
				}
			}
		}

		/// <summary>
		/// Gets the fourth game option.
		/// </summary>
		public ImageSource FourthOption
		{
			get { return fourthOption; }
			private set
			{
				if (fourthOption != value)
				{
					fourthOption = value;
					OnPropertyChanged("FourthOption");
				}
			}
		}

		/// <summary>
		/// Gets the game board background
		/// </summary>
		public ImageSource Background
		{
			get { return background; }
			private set
			{
				if (background != value)
				{
					background = value;
					OnPropertyChanged("Background");
				}
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Loads the localized predefined game messages.
		/// </summary>
		private void LoadMessages()
		{
			Feedback = DefaultGameMessage;
			FailureMessage = DefaultFailureMessage;
			Score = 0;
		}

		/// <summary>
		/// Validates the user selected answer.
		/// </summary>
		/// <param name="e"></param>
		private void ValidateAnswer(OddItemSelectedEventArgs e)
		{
			// Calculates the time which needed to complete the activity
			TimeSpan activityTime = DateTime.Now - activityStart;
			int givenAnswer = e.SelectedItemIndex;

			// Calculate the score
			if (givenAnswer == this.expectedAnswer)
			{
				Score += 1;
				Feedback = this.Game.Activities[this.currentActivity].PositiveFeedback;
			}
			else
			{
				Feedback = this.Game.Activities[this.currentActivity].NegativeFeedback;
				FailureMessage = string.Format(DefaultFailureMessage, failures);
			}

			this.gameStatistics.Add(new Tuple<OddOneOutActivity, TimeSpan, int>(this.Game.Activities[this.currentActivity], activityTime, givenAnswer));

			this.currentActivity++;

			RefreshGame();
		}

		private void RefreshGame()
		{
			OddOneOutActivity activity = this.currentActivity <= this.Game.Activities.Length - 1 ? this.Game.Activities[this.currentActivity] : null;
			
			// We have completed the game.
			if (activity == null)
			{
				this.NavigationManager.GoBack();
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
			this.SecondOption = activity[randomOrder[1]].Option.ImageSource;
			this.ThirdOption = activity[randomOrder[2]].Option.ImageSource;
			this.FourthOption = activity[randomOrder[3]].Option.ImageSource;
			this.activityStart = DateTime.Now;
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

		#endregion

		#region Constructor
		
		public OddOneOutViewModel()
			: base()
		{
			this.validateAnswerCommand = new RelayCommand<OddItemSelectedEventArgs>(this.ValidateAnswer);
			this.gameStatistics = new List<Tuple<OddOneOutActivity, TimeSpan, int>>();
		}

		#endregion

		/// <summary>
		/// Initializes the odd one out activity.
		/// </summary>
		/// <param name="parameter">The odd one out game model.</param>
		public override void Initialize(object parameter)
		{
			base.Initialize(parameter);

			this.Game = (OddOneOutGame)parameter;

			this.timeStatistics = new TimeSpan[this.Game.Activities.Length];
			// this.answers = new bool[this.Game.Activities.Length];

			RefreshGame();
		}

		/// <summary>
		/// Gets the validate answer command.
		/// </summary>
		public ICommand ValidateAnswerCommand
		{
			get { return this.validateAnswerCommand; }
		}

		#region IPausable Members
		/// <summary>
		/// Indicates if the game is paused.
		/// </summary>
		public bool IsPaused
		{
			get { return isPaused; }
			set
			{
				if (this.isPaused != value)
				{
					this.isPaused = value;
					this.OnPropertyChanged("IsPaused");
				}
			}
		}

		/// <summary>
		/// Pauses the game.
		/// </summary>
		public void Pause()
		{
			IsPaused = true;
		}

		/// <summary>
		/// Resumes the game.
		/// </summary>
		public void Resume()
		{
			IsPaused = false;
		}

		#endregion
	}
}
