using Kinemat.Models.Games;
using Kinemat.Viewer.ViewModels;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Kinemat.Viewer.Views
{
	/// <summary>
	/// Interaction logic for GameScreenView.xaml
	/// </summary>
	public partial class GameScreenView : UserControl
	{
		#region Dependency properties

		public static readonly DependencyProperty CurrentGameProperty = DependencyProperty.Register(
			"CurrentGame", typeof(string), typeof(GameScreenView), new PropertyMetadata());

		#endregion

		#region Property wrappers

		public string CurrentGame
		{
			get { return (string)GetValue(CurrentGameProperty); }
			set { SetValue(CurrentGameProperty, value); }
		}

		#endregion
		/// <summary>
		/// Name of the non-transitioning visual state.
		/// </summary>
		internal const string NormalState = "Normal";

		/// <summary>
		/// Name of the fade in transition.
		/// </summary>
		internal const string FadeInTransitionState = "FadeIn";

		/// <summary>
		/// Name of the fade out transition.
		/// </summary>
		internal const string FadeOutTransitionState = "FadeOut";

		/// <summary>
		/// 
		/// </summary>
		internal const string VirtualDrumKitNormalState = "VirtualDrumKitNormal";
		
		/// <summary>
		/// 
		/// </summary>
		internal const string VirtualDrumKitFadeInState = "VirtualDrumKitFadeIn";
		
		/// <summary>
		/// 
		/// </summary>
		internal const string VirtualDrumKitFadeOutState = "VirtualDrumKitFadeOut";


		/// <summary>
		/// 
		/// </summary>
		internal const string OddOneOutNormalState = "OddOneOutNormal";

		/// <summary>
		/// 
		/// </summary>
		internal const string OddOneOutFadeInState = "OddOneOutFadeIn";

		/// <summary>
		/// 
		/// </summary>
		internal const string OddOneOutFadeOutState = "OddOneOutFadeOut";

		/// <summary>
		/// 
		/// </summary>
		internal const string NoGoNoNormalState = "NoGoNoNormal";

		/// <summary>
		/// 
		/// </summary>
		internal const string NoGoNoFadeInState = "NoGoNoFadeIn";

		/// <summary>
		/// 
		/// </summary>
		internal const string NoGoNoFadeOutState = "NoGoNoFadeOut";


		public GameScreenView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Close the full screen view of the image
		/// </summary>
		private void OnCloseOverlayGrid(object sender, RoutedEventArgs e)
		{
			GameScreenViewModel viewModel = this.DataContext as GameScreenViewModel;

			if (viewModel.CurrentGame == "OddOneOut")
			{
				VisualStateManager.GoToElementState(OddOneOutGrid, OddOneOutNormalState, false);
				VisualStateManager.GoToElementState(OddOneOutGrid, OddOneOutFadeOutState, false);
			}

			viewModel.CanNavigateToNextPage = true;
			viewModel.IsInteractivePage = false;
			// Always go to normal state before a transition
			//VisualStateManager.GoToElementState(SimonSaysGrid, NormalState, false);
			//VisualStateManager.GoToElementState(SimonSaysGrid, FadeOutTransitionState, true);
		}

		/// <summary>
		/// Overlay the full screen view of the image
		/// </summary>
		private void OnShowOverlayGrid(object sender, RoutedEventArgs e)
		{
			GameScreenViewModel viewModel = this.DataContext as GameScreenViewModel;

			if (viewModel.CurrentGame == "OddOneOut")
			{
				VisualStateManager.GoToElementState(OddOneOutGrid, OddOneOutNormalState, false);
				VisualStateManager.GoToElementState(OddOneOutGrid, OddOneOutFadeInState, false);
				OddOneOutGB.Start(viewModel.CurrentPage.KinectGame as OddOneOutGame, OnCloseOverlayGrid);
			}

			if(viewModel.CurrentGame == "SimonSaysGame")
			{
				VisualStateManager.GoToElementState(SimonSaysGrid, NormalState, false);
				VisualStateManager.GoToElementState(SimonSaysGrid, FadeInTransitionState, false);
				SimonSaysBoard.Start(viewModel.CurrentPage.KinectGame as SimonSaysGame, OnCloseOverlayGrid);
			}

			if (this.CurrentGame == "VirtualDrumKit")
			{
				VisualStateManager.GoToElementState(SimonSaysGrid, VirtualDrumKitNormalState, false);
				VisualStateManager.GoToElementState(SimonSaysGrid, VirtualDrumKitFadeInState, false);
			}

			if (this.CurrentGame == "NoGoNo")
			{
				VisualStateManager.GoToElementState(SimonSaysGrid, NoGoNoNormalState, false);
				VisualStateManager.GoToElementState(SimonSaysGrid, NoGoNoFadeInState, false);
			
			}

			// Always go to normal state before a transition
		   //  this.SelectedImage = ((ContentControl)e.OriginalSource).Content as ImageSource;
			//VisualStateManager.GoToElementState(SimonSaysGrid, NormalState, false);
			//VisualStateManager.GoToElementState(SimonSaysGrid, FadeInTransitionState, false);
		}

		private void UserControl_KeyUp(object sender, KeyEventArgs e)
		{

		}
	}
}
