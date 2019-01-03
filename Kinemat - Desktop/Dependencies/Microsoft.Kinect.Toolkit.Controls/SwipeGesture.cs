using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Kinect.Toolkit.Controls
{
    public class SwipeGesture : INotifyPropertyChanged
    {
        #region Private members

        private SwipeDirection swipeDirection;

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// INotifyPropertyChanged implementation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the direction of the swipe gesture
        /// </summary>
        public SwipeDirection SwipeDirection
        {
            get { return swipeDirection; }
            set
            {
                if (swipeDirection != value)
                {
                    swipeDirection = value;
                    OnPropertyChanged("SwipeDirection");
                }
            }
        }

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            // If we have at least one subscribed method
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
