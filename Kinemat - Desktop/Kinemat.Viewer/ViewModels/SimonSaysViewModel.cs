using Kinemat.Models.Games;
using Kinemat.Viewer.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Kinemat.Viewer.ViewModels
{
	[ExportNavigable(NavigableContextName = NavigableContexts.HomeScreen)]
	public class SimonSaysViewModel : ViewModelBase
	{
		//#region Private members

		//private SimonSaysGame game;

		//#region Storyboard controls

		//private 


		//#endregion

		////private string sequenceRequired;
		////private string timer;
		////private bool tickHardIsPlaying;
		//private bool tickNormalIsPlaying;
		//private bool tickEasyIsPlaying;
		//private double timerAngle;
		//private int timesFailed;
		//private bool wrongStoryboardIsPlaying;
		//private bool messageStoryboardIsPlaying;
		//private string failureMessage;
		//private string gameMessage;
		//private int simonIndex = 0;
		//private bool canPlay = false;
		//int sec = 0;

		//DispatcherTimer simonSequenceGenerator = new DispatcherTimer();
		//DispatcherTimer gameTimer = new DispatcherTimer();

		
		//private int CurrentRegion = 0;
		//private int index = 0;
		//private int TimerDifficulty = 2;
		//private int maxIndex = 0;
		//private int timerMax;
		//private int secRemaining;
		//private int[] sequence = new int[500];
		//private int[] answers = new int[500];
		//Random rand = new Random();
		////private bool CanPlay = false;
		//// Storyboard tickEasy, tickMedium, tickHard, Yellow, Red, Green, Blue, msg, wrong;

		//#endregion

		//public override void Initialize(object parameter)
		//{
		//	base.Initialize(parameter);
		//}

		//#region Private methods

		//private void InitializeGame()
		//{
		//	switch (this.game.Difficulty)
		//	{
		//		case SimonSaysDifficulty.Ease:
		//			timerMax = 15;
		//			break;
		//		case SimonSaysDifficulty.Normal:
		//			timerMax = 10;
		//			break;
		//		case SimonSaysDifficulty.Hard:
		//			timerMax = 5;
		//			break;
		//	}

		//	gameTimer.Tick += TimerTick;
		//	gameTimer.Interval = TimeSpan.FromSeconds(1);

		//	simonSequenceGenerator.Tick += GenerateSimonSequence;
		//	simonSequenceGenerator.Interval = TimeSpan.FromSeconds(800);

		//	sequence[maxIndex] = rand.Next(0, 4);
		//	this.SequenceRequired = "Sequence Required: " + sequence[maxIndex];

		//	simonSequenceGenerator.Start();
		//}

		//void GenerateSimonSequence(object sender, EventArgs e)
		//{
		//	//fire7_wav.IsMuted = false;
		//	//ice03_wav.IsMuted = false;
		//	//bolt06_wav.IsMuted = false;
		//	//earth04_wav.IsMuted = false;


		//	//if (sequence[SimonIndex] == 3)
		//	//{
		//	//	Yellow.Begin();
		//	//}
		//	//if (sequence[SimonIndex] == 2)
		//	//{
		//	//	Green.Begin();
		//	//}
		//	//if (sequence[SimonIndex] == 1)
		//	//{
		//	//	Red.Begin();
		//	//}
		//	//if (sequence[SimonIndex] == 0)
		//	//{
		//	//	Blue.Begin();
		//	//}

		//	//SimonIndex++;
		//	//if (SimonIndex == maxIndex + 1)
		//	//{
		//	//	SimonSequence.Stop();
		//	//	CanPlay = true;
		//	//	msg.Stop();
		//	//	MSG.Text = "GIVE IT A GO!";
		//	//	secRemaining = TimerMax;
		//	//	timetxt.Text = secRemaining.ToString();
		//	//	Timer.Start();
		//	//	if (TimerDifficulty == 3)
		//	//		tickHard.Begin();
		//	//	if (TimerDifficulty == 2)
		//	//		tickMedium.Begin();
		//	//	if (TimerDifficulty == 1)
		//	//		tickEasy.Begin();
		//	//}
		//}

		//private void TimerTick(object sender, EventArgs e)
		//{
		//	secRemaining--;
		//	Timer = secRemaining.ToString();

		//	if (secRemaining == -1)
		//	{
		//		this.TickHardIsPlaying = false;
		//		this.TickNormalIsPlaying = false;
		//		this.TickEasyIsPlaying = false;
		//		this.gameTimer.Stop();
		//		this.TimerAngle = 360;
		//		this.secRemaining = timerMax;
		//		this.Timer = secRemaining.ToString();
		//		TimesFailed++;
		//		WrongStoryboardIsPlaying = true;
		//		FailureMessage = "Times FAILED: " + TimesFailed;
		//		ProgressSequence(false);
		//	}
		//}

		//private void ProgressSequence(bool right)
		//{
		//	if (right)
		//	{
		//		maxIndex++;
		//		sequence[maxIndex] = rand.Next(0, 4);
		//		index = 0;
		//		SequenceRequired = "Sequence Required: ";

		//		for (int i = 0; i < maxIndex + 1; i++)
		//		{
		//			SequenceRequired += sequence[i] + " ";
		//		}
		//	}
		//	else
		//	{
		//		maxIndex = 0;
		//		index = 0;
		//		sequence[0] = rand.Next(0, 4);
		//		SequenceRequired = "Sequence Required: " + sequence[0];
		//	}

		//	GameMessage = "CONCENTRATE...";
		//	MessageStoryboardIsPlaying = true;
		//	simonIndex = 0;
		//	this.simonSequenceGenerator.Start();
		//	this.CanPlay = false;
		//}

		//#endregion

		//#region Public properties

		//public string SequenceRequired
		//{
		//	get { return this.sequenceRequired; }
		//	set
		//	{
		//		if (this.sequenceRequired != value)
		//		{
		//			this.sequenceRequired = value;
		//			OnPropertyChanged("SequenceRequired");
		//		}
		//	}
		//}

		//public string Timer
		//{
		//	get { return this.timer; }
		//	set
		//	{
		//		if (this.timer != value)
		//		{
		//			this.timer = value;
		//			OnPropertyChanged("Timer");
		//		}
		//	}
		//}

		//public bool TickEasyIsPlaying
		//{
		//	get{return this.tickEasyIsPlaying;}
		//	set
		//	{
		//		if (this.tickEasyIsPlaying != value)
		//		{
		//			this.tickEasyIsPlaying = value;
		//			OnPropertyChanged("TickEaseIsPlaying");
		//		}
		//	}
		//}

		//public bool TickNormalIsPlaying
		//{
		//	get { return this.tickNormalIsPlaying; }
		//	set
		//	{
		//		if (this.tickNormalIsPlaying != value)
		//		{
		//			this.tickNormalIsPlaying = value;
		//			OnPropertyChanged("TickNormalIsPlaying");
		//		}
		//	}
		//}

		//public bool TickHardIsPlaying
		//{
		//	get { return this.tickHardIsPlaying; }
		//	set
		//	{
		//		if (this.tickHardIsPlaying != value)
		//		{
		//			this.tickHardIsPlaying = value;
		//			OnPropertyChanged("TickHardIsPlaying");
		//		}
		//	}
		//}

		//public double TimerAngle
		//{
		//	get { return this.timerAngle; }
		//	set
		//	{
		//		if (this.timerAngle != value)
		//		{
		//			this.timerAngle = value;
		//			OnPropertyChanged("TimerAngle");
		//		}
		//	}
		//}

		//public int TimesFailed
		//{
		//	get { return this.timesFailed; }
		//	set
		//	{
		//		if (this.timesFailed != value)
		//		{
		//			this.timesFailed = value;
		//			OnPropertyChanged("TimesFailed");
		//		}
		//	}
		//}

		//public bool WrongStoryboardIsPlaying
		//{
		//	get { return this.wrongStoryboardIsPlaying; }
		//	set
		//	{
		//		if (this.wrongStoryboardIsPlaying != value)
		//		{
		//			this.wrongStoryboardIsPlaying = value;
		//			OnPropertyChanged("WrongStoryboardIsPlaying");
		//		}
		//	}
		//}

		//public string FailureMessage
		//{
		//	get { return this.FailureMessage; }
		//	set
		//	{
		//		if (this.FailureMessage != value)
		//		{
		//			this.FailureMessage = value;
		//			OnPropertyChanged("FailureMessage");
		//		}
		//	}
		//}

		//public string GameMessage
		//{
		//	get { return this.gameMessage; }
		//	set
		//	{
		//		if (this.gameMessage != value)
		//		{
		//			this.gameMessage = value;
		//			OnPropertyChanged("GameMessage");
		//		}
		//	}
		//}

		//public bool MessageStoryboardIsPlaying
		//{
		//	get { return this.messageStoryboardIsPlaying; }
		//	set
		//	{
		//		if (this.messageStoryboardIsPlaying != value)
		//		{
		//			this.messageStoryboardIsPlaying = value;
		//			OnPropertyChanged("MessageStoryboardIsPlaying");
		//		}
		//	}
		//}

		//public bool CanPlay
		//{
		//	get { return this.canPlay; }
		//	set
		//	{
		//		if (this.canPlay != value)
		//		{
		//			this.canPlay = value;
		//			OnPropertyChanged("CanPlay");
		//		}
		//	}
		//}
	}
}
