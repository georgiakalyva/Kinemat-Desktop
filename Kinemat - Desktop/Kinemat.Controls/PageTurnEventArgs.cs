using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace Kinemat.Controls
{
	public class PageTurnEventArgs : RoutedEventArgs
	{
		#region Public properties

		public RadBookItem Page { get; private set; }
		public NavigationDirection Direction { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PageTurnEventArgs"/> class.
		/// </summary>
		/// <param name="source">The object that raised the event.</param>
		/// <param name="page">The <see cref="Telerik.Windows.Controls.Navigation.RadBookItem"/> associated with the event.</param>
		/// <param name="navigationDirection">Navigation direction.</param>
		/// <param name="routedEvent">The routed event.</param>
		public PageTurnEventArgs(object source, RadBookItem page, NavigationDirection navigationDirection, RoutedEvent routedEvent)
			: base(routedEvent, source)
		{
			this.Page = page;
			this.Direction = navigationDirection;
		}

		#endregion
	}
}
