using System;
using System.Windows;
using System.Windows.Controls;
using Kinemat.Models.Book;

namespace Kinemat.Viewer.Utilities
{
	/// <summary>
	/// A data template selector class for book pages
	/// </summary>
	public class BookPageTemplateSelector : DataTemplateSelector
	{
		#region Private members

		private DataTemplate blankPageTemplate;
		private DataTemplate softPageTemplate;
		private DataTemplate interactivePageTemplate;
		private DataTemplate frontCoverPageTemplate;
		private DataTemplate backCoverPageTemplate;
		
		#endregion

		#region Public properties

		/// <summary>
		/// Gets or sets the data template of a blank book page.
		/// </summary>
		public DataTemplate BlankPageTemplate
		{
			get { return blankPageTemplate; }
			set
			{
				if (blankPageTemplate != value)
					blankPageTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the data template of a soft (normal) book page.
		/// </summary>
		public DataTemplate SoftPageTemplate
		{
			get { return softPageTemplate; }
			set
			{
				if (softPageTemplate != value)
					softPageTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the data template of an interactive book page.
		/// </summary>
		public DataTemplate InteractivePageTemplate
		{
			get { return interactivePageTemplate; }
			set
			{
				if (interactivePageTemplate != value)
					interactivePageTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the data template of the front cover book page.
		/// </summary>
		public DataTemplate FrontCoverPageTemplate
		{
			get { return frontCoverPageTemplate; }
			set
			{
				if (frontCoverPageTemplate != value)
					frontCoverPageTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the data template of the back cover book page.
		/// </summary>
		public DataTemplate BackCoverPageTemplate
		{
			get { return backCoverPageTemplate; }
			set
			{
				if (backCoverPageTemplate != value)
					backCoverPageTemplate = value;
			}
		}

		#endregion

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (container == null)
				throw new ArgumentNullException("container");

			DataTemplate selectedDataTemplate;

			// Convert the item object to a BookPage object
			BookPage bookPage = item as BookPage;

			// If the BookPage object is null, then the original item was not a BookPage.
			// So we cannot handle it!!
			if (bookPage == null)
				throw new ArgumentException();

			switch (bookPage.Type)
			{
				case PageType.Blank:
					selectedDataTemplate = BlankPageTemplate;
					break;
				case PageType.Soft:
					selectedDataTemplate = SoftPageTemplate;
					break;
				case PageType.FrontCover:
					selectedDataTemplate = FrontCoverPageTemplate;
					break;
				case PageType.Interactive:
					selectedDataTemplate = InteractivePageTemplate;
					break;
				case PageType.BackCover:
					selectedDataTemplate = BackCoverPageTemplate;
					break;
				default:
					selectedDataTemplate = BackCoverPageTemplate;
					break;
			}

			return selectedDataTemplate;
		}
	}
}
