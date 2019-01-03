using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// This class represents an item that is added to the ObservableQueue.
    ///             The queue takes care of executing all its pending turns.
    /// 
    /// </summary>
    internal class PendingTurn
    {
        public FoldPosition From { get; set; }

        public FoldPosition To { get; set; }

        public int NewIndex { get; set; }

        public int OldIndex { get; set; }

        public PendingTurn(FoldPosition from, FoldPosition to, int newIndex, int oldIndex)
        {
            this.From = from;
            this.To = to;
            this.NewIndex = newIndex;
            this.OldIndex = oldIndex;
        }
    }
}