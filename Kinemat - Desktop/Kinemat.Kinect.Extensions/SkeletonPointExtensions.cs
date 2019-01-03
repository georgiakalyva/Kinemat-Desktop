using Kinemat.Core.Mathematics;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinemat.Kinect.Extensions
{
    public static class SkeletonPointExtensions
    {
        public static Vector3 ToVector3(this SkeletonPoint source)
        {
            return new Vector3(source.X, source.Y, source.Z);
        }
    }
}
