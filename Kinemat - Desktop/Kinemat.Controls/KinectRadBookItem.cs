using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Kinemat.Controls
{
    public class KinectRadBookItem : RadBookItem
    {
        #region Constructors

        public KinectRadBookItem()
        {
            KinectRegion.AddHandPointerEnterHandler(this, OnHandPointerEnter);
            KinectRegion.AddHandPointerLeaveHandler(this, OnHandPointerLeave);
        }

        #endregion

        #region Private methods

        private void OnHandPointerLeave(object sender, HandPointerEventArgs e)
        {
        }

        private void OnHandPointerEnter(object sender, HandPointerEventArgs e)
        {
        }

        #endregion
    }
}
