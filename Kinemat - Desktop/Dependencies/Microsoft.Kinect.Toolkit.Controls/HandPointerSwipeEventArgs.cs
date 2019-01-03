using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Kinect.Toolkit.Controls
{
    public class HandPointerSwipeEventArgs : RoutedEventArgs
    {
        internal HandPointerSwipeEventArgs(HandPointer handPointer, SwipeGesture swipeGesture, RoutedEvent routedEvent, UIElement source)
            : base(routedEvent, source)
        {
            HandPointer = handPointer;
            SwipeGesture = swipeGesture;
        }

        /// <summary>
        /// HandPointer associated with event
        /// </summary>
        public HandPointer HandPointer { get; private set; }

        /// <summary>
        /// Swipe gesture direction for the associated hand pointer
        /// </summary>
        public SwipeGesture SwipeGesture { get; private set; }
    }
}
