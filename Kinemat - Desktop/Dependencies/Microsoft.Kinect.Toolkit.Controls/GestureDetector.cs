using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Microsoft.Kinect;

namespace Microsoft.Kinect.Toolkit.Controls
{
    internal abstract class GestureDetector
    {
        #region Private constants

        private const int DefaultHandPointerSamplesToGather = 50;
        private const int DefaultMinimalPeriodBetweenGestures = 0;

        #endregion

        #region Protected members

        protected readonly HandPointerSampleTracker handPointerSampleTracker;
        protected DateTime lastGestureTimeStamp = DateTime.Now;

        #endregion

        #region Public properties

        public int MinimalPeriodBetweenGestures { get; set; }

        #endregion

        // public event Action<string> OnGestureDetected;




        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="GestureDetector"/>
        /// </summary>
        /// <param name="handPointerSamples">The maximum number of <see cref="Microsoft.Kinect.Toolkit.Controls.HandPointer"/> samples to keep in mind when searching for a gesture</param>
        protected GestureDetector(int handPointerSamples = DefaultHandPointerSamplesToGather)
        {
            MinimalPeriodBetweenGestures = DefaultMinimalPeriodBetweenGestures;
            handPointerSampleTracker = new HandPointerSampleTracker(handPointerSamples);
        }

        #endregion


        public virtual void Add(HandPointer handPointer)
        {
            handPointerSampleTracker.AddSample(handPointer.X, handPointer.Y, handPointer.TimestampOfLastUpdate);
            LookForGesture();
        }

        protected abstract void LookForGesture();


    }
}