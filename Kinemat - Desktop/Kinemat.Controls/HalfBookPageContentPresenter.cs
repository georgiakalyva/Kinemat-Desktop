using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Kinemat.Controls
{
    /// <summary>
    /// <see cref="HalfBookPageContentPresenter"/> simulates a single large element spanning two pages. 
    /// Replace the <see cref="System.Windows.Controls.ContentPresenter"/> of two opposite book page elements and adjust the <see cref="Dock"/>
    /// property to get the spanning effect.
    /// </summary>
    public class HalfBookPageContentPresenter : ContentPresenter
    {
        #region Private members

        private Dock dock;

        #endregion

        #region Public properties

        /// <summary>
        /// Position of the book item in the book when facing front.
        /// </summary>
        public Dock Dock
        {
            get { return dock; }
            set
            {
                dock = value;
                UpdateAlignment();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of a <see cref="Kinemat.Controls.HalfBookPageContentPresenter"/>.
        /// </summary>
        public HalfBookPageContentPresenter()
        {
            UpdateAlignment();
        }

        #endregion

        private void UpdateAlignment()
        {
            switch (Dock)
            {
                case Dock.Left:
                    HorizontalAlignment = HorizontalAlignment.Left;
                    VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case Dock.Right:
                    HorizontalAlignment = HorizontalAlignment.Right;
                    VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case Dock.Top:
                    HorizontalAlignment = HorizontalAlignment.Stretch;
                    VerticalAlignment = VerticalAlignment.Top;
                    break;
                case Dock.Bottom:
                    HorizontalAlignment = HorizontalAlignment.Stretch;
                    VerticalAlignment = VerticalAlignment.Bottom;
                    break;
            }
        }

        private Size Double(Size s)
        {
            if (HorizontalAlignment != HorizontalAlignment.Stretch)
                s.Width = s.Width + s.Width;
            if (VerticalAlignment != VerticalAlignment.Stretch)
                s.Height = s.Height + s.Height;
            return s;
        }

        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes
        /// can override this method to define their own Measure pass behavior.
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateAlignment();
            Size size = base.MeasureOverride(this.Double(availableSize));
            return new Size(double.IsPositiveInfinity(availableSize.Width) ? size.Width : availableSize.Width, double.IsPositiveInfinity(availableSize.Height) ? size.Height : availableSize.Height);
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes
        /// can override this method to define their own Arrange pass behavior.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            UpdateAlignment();
            return base.ArrangeOverride(this.Double(finalSize));
        }
    }
}
