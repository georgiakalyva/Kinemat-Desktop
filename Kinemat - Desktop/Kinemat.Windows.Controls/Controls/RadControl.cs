using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Kinemat.Windows.Controls
{
    /// <summary>
    /// Static container for the Theme attached property.
    /// 
    /// </summary>
    public static class RadControl
    {
        private static bool? isInDesignMode = new bool?();

        /// <summary>
        /// Gets whether a control is running in the context of a designer.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// <c>True</c> if this instance is in design mode; otherwise, <c>False</c>.
        /// 
        /// </value>
        public static bool IsInDesignMode
        {
            get
            {
                if (RadControl.isInDesignMode.HasValue)
                    return RadControl.isInDesignMode.Value;
                RadControl.isInDesignMode = new bool?(DesignerProperties.GetIsInDesignMode((DependencyObject)new ContentControl()));
                return RadControl.isInDesignMode.Value;
            }
            internal set
            {
                RadControl.isInDesignMode = new bool?(value);
            }
        }

        static RadControl()
        {
        }

        /// <summary>
        /// Gets whether a control is hosted within element host.
        /// 
        /// </summary>
        /// <param name="element"/>
        /// <returns>
        /// <c>True</c> if the element is in element host; otherwise, <c>False</c>.
        /// </returns>
        internal static bool IsInElementHost(DependencyObject element)
        {
            if (!DesignerProperties.GetIsInDesignMode(element) && !BrowserInteropHelper.IsBrowserHosted)
                return PresentationSource.FromVisual(element as Visual) != null;
            else
                return false;
        }

        internal static bool HasPermissions()
        {
            return typeof(RadControl).Assembly.PermissionSet.IsUnrestricted();
        }
    }
}