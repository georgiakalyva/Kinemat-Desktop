using System;
using System.Windows;

namespace Microsoft.Kinect.Toolkit.Controls
{
    /// <summary>
    /// Helper class to take a list of position samples and return information about the movement.
    /// </summary>
    internal class HandPointerSampleTracker
    {
        /// <summary>
        /// Number of samples to track.
        /// </summary>
        private readonly int bufferSize;

        /// <summary>
        /// Available samples.
        /// </summary>
        private readonly Sample[] samples;

        /// <summary>
        /// Head of the queue
        /// </summary>
        private int head;

        /// <summary>
        /// Tail of the queue
        /// </summary>
        private int tail;

        /// <summary>
        /// Initializes a new instance of the HandPointerSampleTracker class.
        /// </summary>
        /// <param name="sampleCount">maximum number of samples to keep</param>
        public HandPointerSampleTracker(int sampleCount)
        {
            head = 0;
            tail = 0;
            bufferSize = sampleCount + 1;
            samples = new Sample[bufferSize];
        }

        /// <summary>
        /// Adds a sample
        /// </summary>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        /// <param name="timeStamp">timestamp, in milliseconds</param>
        public void AddSample(double x, double y, long timeStamp)
        {
            Sample sample = new Sample { X = x, Y = y, TimeStamp = timeStamp };

            samples[head] = sample;
            head = (head + 1) % bufferSize;
            if (head == tail)
            {
                // Circular queue is full, throw out an item
                tail = (tail + 1) % bufferSize;
            }
        }

        /// <summary>
        /// Clears all samples
        /// </summary>
        public void Clear()
        {
            head = tail;
        }

        /// <summary>
        /// Returns the average velocity of the sample history between minimum age and maximum age
        /// </summary>
        /// <param name="minAgeMs">minimum age of sample to consider</param>
        /// <param name="maxAgeMs">maximum age of sample to consider</param>
        /// <param name="presentTimestamp">timestamp representing the current frame</param>
        /// <returns>Velocity in units per second</returns>
        public Point GetAverageVelocity(int minAgeMs, int maxAgeMs, long presentTimestamp)
        {
            Point average, maxNegative, maxPositive;

            this.GetVelocityMetrics(minAgeMs, maxAgeMs, out average, out maxNegative, out maxPositive, presentTimestamp);

            return average;
        }

        /// <summary>
        /// Returns the maximum velocity of the sample history between minimum age and maximum age, filtering out samples that differ from the direction
        /// determined by analyzing the sample history between the minimum and maximum ages.
        /// </summary>
        /// <param name="minAgeMs">minimum age of sample to consider</param>
        /// <param name="maxAgeMs">maximum age of sample to consider</param>
        /// <param name="minAgeMsDirection">minimum age of sample to consider for determining direction</param>
        /// <param name="maxAgeMsDirection">maximum age of sample to consider for determining direction</param>
        /// <param name="presentTimestamp">timestamp representing the current frame</param>
        /// <returns>Velocity in units per second</returns>
        public Vector GetMaximumVelocity(int minAgeMs, int maxAgeMs, int minAgeMsDirection, int maxAgeMsDirection, long presentTimestamp)
        {
            Point average, maxNegative, maxPositive;

            this.GetVelocityMetrics(minAgeMsDirection, maxAgeMsDirection, out average, out maxNegative, out maxPositive, presentTimestamp);

            // determine direction
            bool positiveX = Math.Abs(maxPositive.X) > Math.Abs(maxNegative.X);
            bool positiveY = Math.Abs(maxPositive.Y) > Math.Abs(maxNegative.Y);

            this.GetVelocityMetrics(minAgeMs, maxAgeMs, out average, out maxNegative, out maxPositive, presentTimestamp);

            var outPoint = new Point
                             {
                                 X = positiveX ? maxPositive.X : maxNegative.X,
                                 Y = positiveY ? maxPositive.Y : maxNegative.Y
                             };

            return new Vector(outPoint.X, outPoint.Y);
        }

        /// <summary>
        /// Outputs the average, maximum positive, and maximum negative velocities over a time window
        /// </summary>
        /// <param name="minAgeMs">minimum age of sample to consider</param>
        /// <param name="maxAgeMs">maximum age of sample to consider</param>
        /// <param name="average">average velocity</param>
        /// <param name="maxNegative">maximum negative velocity</param>
        /// <param name="maxPositive">maximum positive velocity</param>
        /// <param name="presentTimestamp">timestamp representing the current frame</param>
        private void GetVelocityMetrics(int minAgeMs, int maxAgeMs, out Point average, out Point maxNegative, out Point maxPositive, long presentTimestamp)
        {
            int sumDivisor = 0;
            var sum = new Point(0.0, 0.0);
            maxNegative = new Point(0.0, 0.0);
            maxPositive = new Point(0.0, 0.0);

            int scan = this.head;

            while (scan != this.tail)
            {
                scan = (scan == 0) ? this.bufferSize - 1 : scan - 1;
                int next = (scan == 0) ? this.bufferSize - 1 : scan - 1;

                // if we've reached the head, we can't do another pair
                if (next == this.head)
                {
                    break;
                }

                long currentSampleSpan = presentTimestamp - this.samples[scan].TimeStamp;
                long nextSampleSpan = presentTimestamp - this.samples[next].TimeStamp;

                if (currentSampleSpan <= maxAgeMs && currentSampleSpan >= minAgeMs && nextSampleSpan <= maxAgeMs && nextSampleSpan >= minAgeMs)
                {
                    // convert from ms to seconds
                    double timeDelta = (this.samples[scan].TimeStamp - this.samples[next].TimeStamp) / 1000.0;

                    // don't calculate for samples that have the same timestamp
                    if (Math.Abs(timeDelta) > double.Epsilon)
                    {
                        double horzontalVelocity = (this.samples[scan].X - this.samples[next].X) / timeDelta;
                        double verticalVelocity = (this.samples[scan].Y - this.samples[next].Y) / timeDelta;

                        if (horzontalVelocity < maxNegative.X)
                        {
                            maxNegative.X = horzontalVelocity;
                        }

                        if (verticalVelocity < maxNegative.Y)
                        {
                            maxNegative.Y = verticalVelocity;
                        }

                        if (horzontalVelocity > maxPositive.X)
                        {
                            maxPositive.X = horzontalVelocity;
                        }

                        if (verticalVelocity > maxPositive.Y)
                        {
                            maxPositive.Y = verticalVelocity;
                        }

                        sum.X += horzontalVelocity;
                        sum.Y += verticalVelocity;

                        ++sumDivisor;
                    }
                }
            }

            if (sumDivisor < double.Epsilon)
            {
                average = new Point(0.0, 0.0);
            }
            else
            {
                sum.X /= sumDivisor;
                sum.Y /= sumDivisor;

                average = sum;
            }
        }

        //private bool GetSwipeMetrics()
        //{
        //    int start = head;
        //    int scan = start;

        //    while (scan != tail)
        //    {
        //        // scan = (scan == 0) ? bufferSize - 1 : scan - 1;
        //        scan = (scan == 0) ? bufferSize - 1 : scan;
        //        // int next = (scan == 0) ? this.bufferSize - 1 : scan - 1;


        //    }
        //    int start = 0;

        //    for (int index = 1; index < samples.Length - 1; index++)
        //    {
        //        if (!heightFunction(Entries[0].Position, Entries[index].Position) || !directionFunction(Entries[index].Position, Entries[index + 1].Position))
        //        {
        //            start = index;
        //        }

        //        if (lengthFunction(Entries[index].Position, Entries[start].Position))
        //        {
        //            double totalMilliseconds = (Entries[index].Time - Entries[start].Time).TotalMilliseconds;
        //            if (totalMilliseconds >= minTime && totalMilliseconds <= maxTime)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        #region Calculator functions

        private Func<Sample, Sample, float, bool> HeightFunction = (firstSample, secondSample, maximalHeight) => Math.Abs(secondSample.Y - firstSample.Y) < maximalHeight;
        private Func<Sample, Sample, bool> ProgressionToRight = (firstSample, secondSample) => secondSample.X - firstSample.X > -0.01f;
        private Func<Sample, Sample, float, bool> LengthFunction = (firstSample, secondSample, minimalLength) => Math.Abs(secondSample.X - firstSample.X) > minimalLength;


        #endregion

        private struct Sample
        {
            public long TimeStamp;

            public double X;
            public double Y;
        }
    }
}