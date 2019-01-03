using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Kinect.Toolkit.Controls.Gestures
{
    public class ContextPoint
    {
        public DateTime Time
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }
    }
}
