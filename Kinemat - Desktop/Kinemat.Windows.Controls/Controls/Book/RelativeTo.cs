namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// This enumeration is used to determine the pixel point of a corner.
    ///             For example, imagine having a book Width=500 and Height=500.
    ///             If we request the pixel point of the TopRight corner with respect to the Book, then the pixel point will be [500, 0].
    ///             If we request the pixel point of the TopRight corner with respect to the Page(the right page in this case), then the pixel point will be [250, 0], since 250 is half the width of the Book.
    /// 
    /// </summary>
    public enum RelativeTo
    {
        Book,
        Page,
    }
}