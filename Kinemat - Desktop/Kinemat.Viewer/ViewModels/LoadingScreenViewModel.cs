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
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Kinemat.IO;

namespace Kinemat.Viewer.ViewModels
{
	/// <summary>
	/// Game screen view model
	/// </summary>
	[ExportNavigable(NavigableContextName = NavigableContexts.LoadingScreen)]
	public class LoadingScreenViewModel : ViewModelBase
	{
		#region Constants

		private const string DefaultLoadingMessage = "Initializing book...";

		#endregion

		public override void OnNavigatedTo()
		{
			base.OnNavigatedTo();
			InitializeBook();
		}

		public override void OnNavigatedFrom()
		{
			base.OnNavigatedFrom();

			this.loadingTimer.Stop();
		}

		#region Private members

		private bool isLoading;
		private string loadingMessage;
		private IEnumerable<BookPage> bookPages;
		private DispatcherTimer loadingTimer = new DispatcherTimer();

		#endregion

		#region Constructors

		public LoadingScreenViewModel()
			:base()
		{
			this.loadingTimer.Interval = TimeSpan.FromSeconds(3);
			this.loadingTimer.Tick += LoadingCompleted;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets when loading a book
		/// </summary>
		public bool IsLoading
		{
			get { return isLoading; }
			protected set
			{
				if (isLoading != value)
				{
					isLoading = value;
					OnPropertyChanged("IsLoading");
				}
			}
		}

		/// <summary>
		/// Gets the loading message.
		/// </summary>
		public string LoadingMessage
		{
			get { return loadingMessage; }
			protected set
			{
				if (loadingMessage != value)
				{
					loadingMessage = value;
					OnPropertyChanged("LoadingMessage");
				}
			}
		}

		#endregion

		#region Private methods

		private void InitializeBook()
		{
			InitializeLoadingInformation();

			// Get the book id.
			// TODO: Use MEF injection
			Guid gameId = ((App)Application.Current).gameId;

			bookPages = ArchiveReader.LoadArchive(gameId);
		}

		private void InitializeLoadingInformation()
		{
			this.IsLoading = true;
			this.LoadingMessage = DefaultLoadingMessage;
			this.loadingTimer.Start();
		}

		#endregion

		#region Event handlers

		private void LoadingCompleted(object sender, EventArgs e)
		{
			this.NavigationManager.NavigateToHome(NavigableContexts.BookScreen, this.bookPages);
		}

		#endregion	
	}
}
