using Kinemat.Models.Book;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Kinemat.Viewer.Utilities
{
    public class BookPageContentStyleSelector : StyleSelector
    {
        #region Private members

        private Style leftBookPageContentStyle;
        private Style rightBookPageContentStyle;
        private Style coverPageContentStyle;

        #endregion

        // IMPORTANT - This style selector is suitable for the BookPage model class only!
        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style selectedStyle;
            BookPage bookPage = item as BookPage;

            if (bookPage == null)
                throw new InvalidOperationException("This style selector can be used with the BookPage model class only.");

            switch (bookPage.Type)
            {
                case PageType.Soft:
                case PageType.Interactive:
                    if (bookPage.Position == Kinemat.Models.Book.BookPagePosition.Left)
                        selectedStyle = LeftBookPageContentStyle;
                    else
                        selectedStyle = RightBookPageContentStyle;
                    break;
                //case PageType.FrontCover:
                //case PageType.BackCover:
                //    selectedStyle = CoverPageContentStyle;
                //    break;
                default:
                    selectedStyle = base.SelectStyle(item, container);
                    break;
            }

            return selectedStyle;
        }

        #region Public properties

        public Style LeftBookPageContentStyle
        {
            get { return leftBookPageContentStyle; }
            set
            {
                if (leftBookPageContentStyle != value)
                    leftBookPageContentStyle = value;
            }
        }

        public Style RightBookPageContentStyle
        {
            get { return rightBookPageContentStyle; }
            set
            {
                if (rightBookPageContentStyle != value)
                    rightBookPageContentStyle = value;
            }
        }

        private Style CoverPageContentStyle
        {
            get { return coverPageContentStyle; }
            set
            {
                if (coverPageContentStyle != value)
                    coverPageContentStyle = value;
            }
        }

        #endregion
    }
}
