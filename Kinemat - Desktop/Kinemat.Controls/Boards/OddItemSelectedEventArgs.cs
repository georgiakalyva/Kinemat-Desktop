using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kinemat.Controls.Boards
{
	public class OddItemSelectedEventArgs : RoutedEventArgs
	{
		#region Public properties

		public int SelectedItemIndex { get; private set; }

		#endregion

		#region Constructors

		public OddItemSelectedEventArgs(object source, int selectedItemIndex, RoutedEvent routedEvent)
			: base(routedEvent, source)
		{
			this.SelectedItemIndex = selectedItemIndex;
		}

		#endregion
	}
}
