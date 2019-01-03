using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xaml;
using Kinemat.Viewer.Navigation;
using Kinemat.Viewer.Utilities;
using Microsoft.Kinect.Toolkit;
using System.Windows.Input;
using Kinemat.Controls;

namespace Kinemat.Viewer.ViewModels
{
    /// <summary>
    /// Home screen view model
    /// </summary>
    [ExportNavigable(NavigableContextName = NavigableContexts.HomeScreen)]
    public class HomeScreenViewModel : ViewModelBase
    {
        /// <summary>
        /// Command that is executed when an experience option is selected
        /// </summary>
        private RelayCommand<RoutedEventArgs> bookSelected;

        /// <summary>
        /// Gets the observable collection of experiences selectable from the home screen
        /// </summary>
        //public ObservableCollection<BookEntry> Books { get; private set; }

        /// <summary>
        /// Gets the experience selected command
        /// </summary>
        public ICommand BookSelectedCommand
        {
            get { return this.bookSelected; }
        }

        /// <summary>
        /// Initializes a new <see cref="HomeScreenViewModel"/> instance
        /// </summary>
        public HomeScreenViewModel() :
            base()
        {
        }

        public override void Initialize(object parameter)
        {
            base.Initialize(parameter);

            Kinemat.Models.Book.BookPage page = parameter as Kinemat.Models.Book.BookPage;
        }

        private void OnBookSelected(RoutedEventArgs e)
        {
        }
    }
}
