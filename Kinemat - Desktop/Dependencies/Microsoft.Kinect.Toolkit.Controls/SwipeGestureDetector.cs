using System;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;

namespace Kinect.Toolbox
{
    internal class SwipeGestureDetector : GestureDetector
    {
        #region Internal properties

        public float SwipeMinimalLength { get; set; }
        public float SwipeMaximalHeight { get; set; }
        public int SwipeMininalDuration { get; set; }
        public int SwipeMaximalDuration { get; set; }

        #endregion

        #region Private constants

        private const int DefaultHandPointerSamplesToGather = 50;
        private const float GestureMinimalLength = 0.4f;
        private const float GestureMaximalHeight = 0.2f;
        private const int GestureMinimalDuration = 250;
        private const int GestureMaximalDuration = 1500;

        #endregion

        #region Constructors

        public SwipeGestureDetector(int handPointerSamples = DefaultHandPointerSamplesToGather)
            : base(handPointerSamples)
        {
            SwipeMinimalLength = GestureMinimalLength;
            SwipeMaximalHeight = GestureMaximalHeight;
            SwipeMininalDuration = GestureMinimalDuration;
            SwipeMaximalDuration = GestureMaximalDuration;
        }

        #endregion

        // public event Action<SwipeGesture> OnGestureDetected;

        protected bool ScanPositions(Func<HandPointer, HandPointer, bool> heightFunction,
                                     Func<HandPointer, HandPointer, bool> directionFunction,
                                     Func<HandPointer, HandPointer, bool> lengthFunction,
                                     int minTime, int maxTime)
        {

            //foreach (HandPointer handPointer in handPointerSampleTracker)
            //{


            //}
            //int start = 0;

            //for (int index = 1; index < Entries.Count - 1; index++)
            //{
            //    if (!heightFunction(Entries[0].Position, Entries[index].Position) || !directionFunction(Entries[index].Position, Entries[index + 1].Position))
            //    {
            //        start = index;
            //    }

            //    if (lengthFunction(Entries[index].Position, Entries[start].Position))
            //    {
            //        double totalMilliseconds = (Entries[index].Time - Entries[start].Time).TotalMilliseconds;
            //        if (totalMilliseconds >= minTime && totalMilliseconds <= maxTime)
            //        {
            //            return true;
            //        }
            //    }
            //}

            return false;
        }

        protected override void LookForGesture()
        {
            // Swipe to right
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < SwipeMaximalHeight, // Height
                (p1, p2) => p2.X - p1.X > -0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > SwipeMinimalLength, // Length
                SwipeMininalDuration, SwipeMaximalDuration)) // Duration
            {
                // RaiseGestureDetected("SwipeToRight");
                return;
            }

            // Swipe to left
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < SwipeMaximalHeight,  // Height
                (p1, p2) => p2.X - p1.X < 0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > SwipeMinimalLength, // Length
                SwipeMininalDuration, SwipeMaximalDuration))// Duration
            {
                // RaiseGestureDetected("SwipeToLeft");
                return;
            }


        }

        //protected void RaiseGestureDetected(SwipeGesture swipeGesture)
        //{
        //    // Too close?
        //    if (DateTime.Now.Subtract(lastGestureTimeStamp).TotalMilliseconds > MinimalPeriodBetweenGestures)
        //    {
        //        if (OnGestureDetected != null)
        //            OnGestureDetected(swipeGesture);

        //        lastGestureTimeStamp = DateTime.Now;
        //    }
        //}
    }
}