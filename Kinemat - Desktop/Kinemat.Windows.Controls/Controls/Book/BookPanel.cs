using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
    /// <summary>
    /// Virtualized panel for RadBook.
    /// 
    /// </summary>
    public class BookPanel : VirtualizingPanel
    {
        private int startingIndex;
        private Size halfSize;
        private Size pageMaxDesiredSize;

        /// <summary>
        /// The index from which realization will begin.
        /// 
        /// </summary>
        public int StartingIndex
        {
            get
            {
                return this.startingIndex;
            }
            internal set
            {
                this.startingIndex = value;
                this.InvalidateMeasure();
            }
        }

        internal int GenerateSpan { get; set; }

        /// <summary>
        /// This collection is used to realize items when a large jump between pages is made, i.e. RightPageIndex goes from 0 to 100.
        /// 
        /// </summary>
        internal IList<int> IndexCollection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the BookPanel class.
        /// 
        /// </summary>
        public BookPanel()
        {
            this.IndexCollection = (IList<int>)new List<int>();
            this.GenerateSpan = 8;
        }

        internal void CleanContainers()
        {
            RadBook radBook = ItemsControl.GetItemsOwner((DependencyObject)this) as RadBook;
            if (radBook == null)
                return;
            int count = radBook.Items.Count;
            IItemContainerGenerator containerGenerator1 = this.ItemContainerGenerator;
            int num = this.startingIndex + Math.Min(this.GenerateSpan, count - this.startingIndex);
            ItemContainerGenerator containerGenerator2 = radBook.ItemContainerGenerator;
            for (int index = 0; index < this.Children.Count; ++index)
            {
                UIElement uiElement = this.Children[index];
                int itemIndex = containerGenerator2.IndexFromContainer((DependencyObject)uiElement);
                if (itemIndex != -1 && (itemIndex < this.startingIndex || itemIndex >= num) && !this.IndexCollection.Contains(itemIndex))
                {
                    GeneratorPosition position = containerGenerator1.GeneratorPositionFromIndex(itemIndex);
                    containerGenerator1.Remove(position, 1);
                    this.RemoveInternalChildRange(index, 1);
                    --index;
                }
            }
        }

        /// <summary>
        /// Override for OnItemsChanged.
        /// 
        /// </summary>
        /// <param name="sender"/><param name="args"/>
        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            base.OnItemsChanged(sender, args);
            ItemsControl itemsOwner = ItemsControl.GetItemsOwner((DependencyObject)this);
            if (itemsOwner == null)
                return;
            if (args.Action == NotifyCollectionChangedAction.Remove && args.ItemUICount > 0)
            {
                int index = args.Position.Index;
                if (args.Position.Offset > 0)
                    ++index;
                this.RemoveInternalChildRange(index, args.ItemCount);
            }
            if (args.Action == NotifyCollectionChangedAction.Replace)
            {
                int index1 = args.Position.Index;
                if (args.Position.Offset > 0)
                    ++index1;
                if (index1 != -1 && index1 < this.Children.Count)
                {
                    int index2 = this.ItemContainerGenerator.IndexFromGeneratorPosition(args.Position);
                    if (args.ItemUICount > 0)
                    {
                        this.RemoveInternalChildRange(index1, args.ItemCount);
                        if (RadControl.IsInDesignMode && itemsOwner != null && itemsOwner.Items[index2] is RadBookItem)
                            this.InsertInternalChild(index1, (UIElement)(itemsOwner.Items[index2] as RadBookItem));
                    }
                }
            }
            this.InvalidateMeasure();
        }

        /// <summary>
        /// Override for Measure.
        /// 
        /// </summary>
        /// <param name="availableSize"/>
        /// <returns/>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.halfSize = new Size(availableSize.Width / 2.0, availableSize.Height);
            this.pageMaxDesiredSize = new Size();
            RadBook radBook = ItemsControl.GetItemsOwner((DependencyObject)this) as RadBook;
            if (radBook == null)
                return availableSize;
            UIElementCollection children = this.Children;
            int count = radBook.Items.Count;
            IItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
            if (radBook.IsVirtualizing)
            {
                int itemsCount = Math.Min(this.GenerateSpan, count - this.startingIndex);
                this.PrepareItems(containerGenerator, this.startingIndex, itemsCount);
                foreach (int itemIndex in (IEnumerable<int>)this.IndexCollection)
                {
                    using (containerGenerator.StartAt(containerGenerator.GeneratorPositionFromIndex(itemIndex), GeneratorDirection.Forward, true))
                    {
                        bool isNewlyRealized;
                        FrameworkElement frameworkElement = containerGenerator.GenerateNext(out isNewlyRealized) as FrameworkElement;
                        if (frameworkElement != null)
                        {
                            if (isNewlyRealized)
                            {
                                this.AddInternalChild((UIElement)frameworkElement);
                                containerGenerator.PrepareItemContainer((DependencyObject)frameworkElement);
                            }
                            frameworkElement.Measure(this.halfSize);
                            this.pageMaxDesiredSize = new Size()
                            {
                                Height = Math.Max(this.pageMaxDesiredSize.Height, frameworkElement.DesiredSize.Height),
                                Width = Math.Max(this.pageMaxDesiredSize.Width, frameworkElement.DesiredSize.Width)
                            };
                        }
                    }
                }
                this.CleanContainers();
            }
            else
                this.PrepareItems(containerGenerator, 0, count);
            double val1 = this.pageMaxDesiredSize.Width * 2.0;
            return new Size()
            {
                Width = double.IsInfinity(availableSize.Width) ? val1 : Math.Min(val1, availableSize.Width),
                Height = double.IsInfinity(availableSize.Height) ? this.pageMaxDesiredSize.Height : Math.Min(this.pageMaxDesiredSize.Height, availableSize.Height)
            };
        }

        /// <summary>
        /// Override for Arrange.
        /// 
        /// </summary>
        /// <param name="finalSize"/>
        /// <returns/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect rect1 = new Rect(0.0, 0.0, finalSize.Width / 2.0, finalSize.Height);
            Rect rect2 = new Rect(finalSize.Width / 2.0, 0.0, finalSize.Width / 2.0, finalSize.Height);
            foreach (UIElement uiElement in this.Children)
            {
                int column = Grid.GetColumn((UIElement)(uiElement as FrameworkElement));
                uiElement.Arrange(column == 0 ? rect1 : rect2);
            }
            return base.ArrangeOverride(finalSize);
        }

        private void PrepareItems(IItemContainerGenerator itemContainerGenerator, int startGeneratorPositionIndex, int itemsCount)
        {
            GeneratorPosition position = itemContainerGenerator.GeneratorPositionFromIndex(startGeneratorPositionIndex);
            using (itemContainerGenerator.StartAt(position, GeneratorDirection.Forward, true))
            {
                for (int index = 0; index < itemsCount; ++index)
                {
                    bool isNewlyRealized;
                    FrameworkElement frameworkElement = itemContainerGenerator.GenerateNext(out isNewlyRealized) as FrameworkElement;
                    if (isNewlyRealized)
                    {
                        this.AddInternalChild((UIElement)frameworkElement);
                        itemContainerGenerator.PrepareItemContainer((DependencyObject)frameworkElement);
                    }
                    frameworkElement.Measure(this.halfSize);
                    this.pageMaxDesiredSize = new Size()
                    {
                        Height = Math.Max(this.pageMaxDesiredSize.Height, frameworkElement.DesiredSize.Height),
                        Width = Math.Max(this.pageMaxDesiredSize.Width, frameworkElement.DesiredSize.Width)
                    };
                }
            }
        }
    }
}
