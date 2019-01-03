using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinemat.Viewer.Design
{
	internal class LoadingScreenDesignTimeViewModel : ViewModels.ViewModelBase
	{
		#region Private members

		private bool isLoading;
		private string loadingMessage;

		#endregion

		#region Public properties

		public bool IsLoading
		{
			get { return isLoading; }
			set
			{
				if (isLoading != value)
				{
					isLoading = value;
					OnPropertyChanged("IsLoading");
				}
			}
		}

		public string LoadingMessage
		{
			get { return loadingMessage; }
			set
			{
				if (loadingMessage != value)
				{
					loadingMessage = value;
					OnPropertyChanged("LoadingMessage");
				}
			}
		}

		#endregion


		#region Constructors

		public LoadingScreenDesignTimeViewModel()
		{
			IsLoading = true;
			loadingMessage = "Initializing book...";
		}

		#endregion
	}
}
