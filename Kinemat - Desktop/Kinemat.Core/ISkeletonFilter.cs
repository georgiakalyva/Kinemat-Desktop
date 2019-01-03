using System.Collections.Generic;
using Microsoft.Kinect;

namespace Kinemat.Core
{
    /// <summary>
    /// Used to filter a set of skeletons into a subset of interest.
    /// </summary>
    public interface ISkeletonFilter
    {
        /// <summary>
        /// Filters the specified enumerable set of skeletons to obtain a smaller subset of interest.
        /// </summary>
        /// <param name="skeletons">
        /// Enumerable set of skeletons to be filtered.
        /// </param>
        /// <returns>
        /// Enumerable set of skeletons output by filtering operation.
        /// </returns>
        IEnumerable<Skeleton> Filter(IEnumerable<Skeleton> skeletons);
    }
}
