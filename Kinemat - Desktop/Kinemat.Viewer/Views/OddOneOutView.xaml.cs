using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kinemat.Viewer.Views
{
    /// <summary>
    /// Interaction logic for OddOneOutView.xaml
    /// </summary>
    public partial class OddOneOutView : UserControl
    {
        #region Dependency properties

        public static readonly DependencyProperty FirstBubbleProperty = DependencyProperty.Register(
            "FirstBubble", typeof(ImageSource), typeof(OddOneOutView), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty SecondBubbleProperty = DependencyProperty.Register(
            "SecondBubble", typeof(ImageSource), typeof(OddOneOutView), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty ThirdBubbleProperty = DependencyProperty.Register(
            "ThirdBubble", typeof(ImageSource), typeof(OddOneOutView), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty FourthBubbleProperty = DependencyProperty.Register(
            "FourthBubble", typeof(ImageSource), typeof(OddOneOutView), new PropertyMetadata(default(ImageSource)));

        #endregion

        #region CLR property wrappers for the dependency properties

        /// <summary>
        /// Gets or sets the content of the first option bubble.
        /// </summary>
        public ImageSource FirstBubble
        {
            get { return (ImageSource)GetValue(FirstBubbleProperty); }
            set { SetValue(FirstBubbleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the second option bubble.
        /// </summary>
        public ImageSource SecondBubble
        {
            get { return (ImageSource)GetValue(SecondBubbleProperty); }
            set { SetValue(SecondBubbleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the third option bubble.
        /// </summary>
        public ImageSource ThirdBubble
        {
            get { return (ImageSource)GetValue(ThirdBubbleProperty); }
            set { SetValue(ThirdBubbleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the fourth option bubble.
        /// </summary>
        public ImageSource FourthBubble
        {
            get { return (ImageSource)GetValue(FourthBubbleProperty); }
            set { SetValue(FourthBubbleProperty, value); }
        }

        #endregion

        #region Constructors
        
        public OddOneOutView()
        {
            InitializeComponent();
        }

        #endregion
    }
}
