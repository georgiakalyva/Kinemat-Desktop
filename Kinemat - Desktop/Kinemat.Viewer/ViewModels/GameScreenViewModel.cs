using Kinemat.Viewer.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Kinemat.Models;
using Kinemat.Models.Book;
using Microsoft.Kinect.Toolkit;
using System.Windows;
using System.Windows.Input;
using Microsoft.Kinect.Toolkit.Controls;
using Telerik.Windows.Controls;
using Kinemat.Controls;
using Kinemat.Models.Games;

namespace Kinemat.Viewer.ViewModels
{
	/// <summary>
	/// Game screen view model
	/// </summary>
	[ExportNavigable(NavigableContextName = NavigableContexts.BookScreen)]
	public class GameScreenViewModel : ViewModelBase
	{
		#region Constants

		private const double DefaultWidth = 800;
		private const double DefaultHeight = 600;

		#endregion

		#region Private members

		/// <summary>
		/// Book width
		/// </summary>
		private double width;

		/// <summary>
		/// Book height
		/// </summary>
		private double height;

		/// <summary>
		/// Indicates whether we have reached an interactive page.
		/// </summary>
		private bool isInteractivePage;

		/// <summary>
		/// Indicates whether we can navigate to the next book page.
		/// </summary>
		private bool canNavigateToNextPage;

		/// <summary>
		/// Indicates whether we can navigate to the previous book page.
		/// </summary>
		private bool canNavigateToPreviousPage;

		/// <summary>
		/// The pages bound to the book view. This collection has been processed (duplicate pages) to be compatible with the book effect.
		/// </summary>
		private ObservableCollection<BookPage> bookPages;

		/// <summary>
		/// Tracks the active page.
		/// </summary>
		private BookPage activePage;

		/// <summary>
		/// Command that is executed when the user requests a game to start.
		/// </summary>
		private RelayCommand<RoutedEventArgs> startGame;

		/// <summary>
		/// Command that is executed after a page turn.
		/// </summary>
		private RelayCommand<RoutedEventArgs> pageTurned;

		/// <summary>
		/// The current soundtrack
		/// </summary>
		private Uri currentSoundtrack;

		/// <summary>
		/// The current narration
		/// </summary>
		private Uri currentNarration;

		private BookPage currentPage;

		private int currentRightPageIndex;

		private string currentGame;
		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new <see cref="GameScreenViewModel"/> instance
		/// </summary>
		public GameScreenViewModel()
			: base()
		{
			startGame = new RelayCommand<RoutedEventArgs>(OnGameStart);
			pageTurned = new RelayCommand<RoutedEventArgs>(OnPageTurned);
			InitializeViewModel();
		}

		/// <summary>
		/// Performs initialization tasks
		/// </summary>
		private void InitializeViewModel()
		{
			// Set the default sizes.
			Width = DefaultWidth;
			Height = DefaultHeight;

			// The front cover page is not an interactive page.
			IsInteractivePage = false;

			// When we see the front cover page, we can navigate to the next, first, book page.
			canNavigateToNextPage = true;

			//bookPages = new ObservableCollection<BookPage>(Kinemat.IO.ArchiveReader.LoadArchive(((App)Application.Current).gameId));
		}

		#endregion

		#region Private methods

		private void SelectGameType(BookPage page)
		{
			if (page.KinectGame is SimonSaysGame)
				CurrentGame = "SimonSaysGame";

			if (page.KinectGame is OddOneOutGame)
				CurrentGame = "OddOneOut";
		}

		/// <summary>
		/// Invoked when the StartGameCommand is executed. Navigates to the appropriate kinect game.
		/// </summary>
		private void OnGameStart(RoutedEventArgs e)
		{
			// Double check if we have reached an interactive page
			if (!(this.currentPage.Type == PageType.Interactive))
				return;

			// The game name is the navigation context to navigate to
			string navigationContext = currentPage.KinectGame.Name;

			// Navigate to the game screen and pass the game
			NavigationManager.NavigateTo(navigationContext, this.currentPage.KinectGame);
		}

		/// <summary>
		/// Invoked when the PageTurnedCommand is executed.
		/// </summary>
		/// <param name="e"></param>
		private void OnPageTurned(RoutedEventArgs e)
		{
			PageTurnEventArgs pageFlipEventArgs = e as PageTurnEventArgs;

			EvaluatePageState(pageFlipEventArgs);
		}

		private void EvaluatePageState(PageTurnEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			// Calculate current page
			if (e.Direction == NavigationDirection.Forward)
			{
				this.currentPage = this.bookPages[e.Page.Index + 1];
			}
			else
			{
				if (e.Page.Index != 1)
					this.currentPage = this.bookPages[e.Page.Index - 2];
				else
					this.currentPage = this.bookPages[0];
			}

			// Set navigation parameters
			if (this.currentPage.Type == PageType.Interactive)
			{
				SelectGameType(this.currentPage);
				CanNavigateToNextPage = false;
				IsInteractivePage = true;
			}
			else if (this.currentPage.Type == PageType.Soft ||
					 this.currentPage.Type == PageType.FrontCover ||
					 this.currentPage.Type == PageType.Blank)
			{
				CanNavigateToNextPage = true;
				IsInteractivePage = false;
			}

			// Always can go back
			CanNavigateToPreviousPage = true;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets the width of the book.
		/// </summary>
		public double Width
		{
			get { return width; }
			private set
			{
				if (width != value)
				{
					width = value;
					OnPropertyChanged("Width");
				}
			}
		}

		/// <summary>
		/// Gets the height of the book.
		/// </summary>
		public double Height
		{
			get { return height; }
			private set
			{
				if (height != value)
				{
					height = value;
					OnPropertyChanged("Height");
				}
			}
		}

		/// <summary>
		/// Gets if we have reached an interactive page.
		/// </summary>
		public bool IsInteractivePage
		{
			get { return isInteractivePage; }
			set
			{
				if (isInteractivePage != value)
				{
					isInteractivePage = value;
					OnPropertyChanged("IsInteractivePage");
				}
			}
		}

		public BookPage CurrentPage
		{
			get { return this.currentPage; }
		}

		/// <summary>
		///  Gets if we can navigate to the next book page.
		/// </summary>
		public bool CanNavigateToNextPage
		{
			get { return canNavigateToNextPage; }
			set
			{
				if (canNavigateToNextPage != value)
				{
					canNavigateToNextPage = value;
					OnPropertyChanged("CanNavigateToNextPage");
				}
			}
		}

		/// <summary>
		///  Gets if we can navigate to the previous book page.
		/// </summary>
		public bool CanNavigateToPreviousPage
		{
			get { return canNavigateToPreviousPage; }
			set
			{
				if (canNavigateToPreviousPage != value)
				{
					canNavigateToPreviousPage = value;
					OnPropertyChanged("CanNavigateToPreviousPage");
				}
			}
		}

		/// <summary>
		/// Gets the book pages bound to the book view.
		/// </summary>
		public ObservableCollection<BookPage> BookPages
		{
			get { return bookPages; }
		}

		/// <summary>
		/// Gets the current soundtrack.
		/// </summary>
		public Uri CurrentSoundtrack
		{
			get { return currentSoundtrack; }
			private set
			{
				if (currentSoundtrack != value)
				{
					currentSoundtrack = value;
					OnPropertyChanged("CurrentSoundtrack");
				}
			}
		}

		/// <summary>
		/// Gets the current narration track.
		/// </summary>
		public Uri CurrentNarration
		{
			get { return currentNarration; }
			private set
			{
				if (currentNarration != value)
				{
					currentNarration = value;
					OnPropertyChanged("CurrentNarration");
				}
			}
		}

		public int CurrentRightPageIndex
		{
			get { return this.currentRightPageIndex; }
			set
			{
				if (this.currentRightPageIndex != value)
				{
					this.currentRightPageIndex = value;
					OnPropertyChanged("CurrentRightPageIndex");
				}
			}
		}

		public string CurrentGame
		{
			get { return this.currentGame; }
			set
			{
				if (this.currentGame != value)
				{
					this.currentGame = value;
					OnPropertyChanged("CurrentGame");
				}
			}
		}

		#endregion

		#region Commands

		/// <summary>
		/// Gets the start game command.
		/// </summary>
		public ICommand StartGameCommand
		{
			get { return startGame; }
		}

		/// <summary>
		/// Gets the page terned command.
		/// </summary>
		public ICommand PageTurnedCommand
		{
			get { return pageTurned; }
		}

		#endregion

		/// <summary>
		/// Performs initialization tasks.
		/// </summary>
		/// <param name="parameter">Object parameter.</param>
		public override void Initialize(object parameter)
		{
			base.Initialize(parameter);

			// Set the book pages
			bookPages = new ObservableCollection<BookPage>((IEnumerable<BookPage>)parameter);

			// The active page is the front cover page.
			activePage = bookPages[0];
		}
	}
}
