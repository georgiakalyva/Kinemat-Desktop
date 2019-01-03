using Kinemat.Core.Mathematics;
using System;
using System.Windows.Shapes;

namespace Kinemat.Kinect.Interactions.Gestures
{
    public class Entry
    {
        public DateTime Time { get; set; }
        public Vector3 Position { get; set; }
        public Ellipse DisplayEllipse { get; set; }
    }
}
