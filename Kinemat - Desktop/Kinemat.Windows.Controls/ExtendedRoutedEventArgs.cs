using System.Windows;

namespace Kinemat.Windows
{
    /// <summary>
    /// Contains state information and event data associated with a routed event.
    /// </summary>
    public class ExtendedRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class.
        /// 
        /// </summary>
        public ExtendedRoutedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class,
        ///             using the supplied routed event identifier.
        /// </summary>
        /// <param name="routedEvent">The routed event identifier for this instance of the RoutedEventArgs class.
        ///             </param>
        public ExtendedRoutedEventArgs(RoutedEvent routedEvent)
            : this(routedEvent, (object)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class, using
        ///             the supplied routed event identifier, and providing the opportunity
        ///             to declare a different source for the event.
        /// </summary>
        /// <param name="routedEvent">The routed event identifier for this instance of the RoutedEventArgs class.
        ///             </param><param name="source">An alternate source that will be reported when the event is handled.
        ///             This pre-populates the Source property.
        ///             </param>
        public ExtendedRoutedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }
    }
}