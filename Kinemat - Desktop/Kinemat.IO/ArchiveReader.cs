using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using Kinemat.Models.Book;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Kinemat.IO.Helpers;
using Kinemat.Models;
using System.Collections.ObjectModel;
using Kinemat.Models.Games;

namespace Kinemat.IO
{
	public class ArchiveReader
	{
		#region Constants

		private const string BookArchivePathPattern = "Library\\{0}.kmt";
		private const string PageElement = "CanvasPage";
		private const string ManifestFile = "book.xml";
		private const string Images = "{0}";

		#endregion

		#region XML constants

		private const string TextAssetText = "text";
		private const string TextAssetLeft = "left";
		private const string ImageAssetLeft = TextAssetLeft;
		private const string TextAssetTop = "top";
		private const string ImageAssetTop = TextAssetTop;
		private const string TextAssetFontSize = "size";
		private const string ImageAssetSize = TextAssetFontSize;
		private const string TextAssetFontColor = "color";
		private const string TextAssetFontFamily = "fontFamily";
		private const string TextAssetFontIsBold = "isBold";
		private const string TextAssetFontIsItalic = "isItalic";
		private const string ImageAssetContentPath = "imagePath";
		private const string ImageAssetRotationAngle = "angle";
		private const string TextAssetTag = "TextAsset";
		private const string ImageAssetTag = "ImageAsset";
		private const string CanvasPage = "CanvasPage";
		private const string LayerAttribute = "layer";

		#endregion

		/// <summary>
		/// Generates the book pages for the given compressed archive.
		/// </summary>
		/// <param name="id">The id of the book archive.</param>
		/// <returns></returns>
		public static IEnumerable<BookPage> LoadArchive(Guid id)
		{
			if (id == null)
				throw new ArgumentNullException("id");

			List<BookPage> pages = new List<BookPage>();

			string bookArchivePath = string.Format(BookArchivePathPattern, id.ToString());

			// Open the book archive
			using (ZipArchive archive = ZipFile.OpenRead(bookArchivePath))
			{
				// Read the manifest file (xml file)
				using (Stream manifestFileStream = archive.GetEntry(ManifestFile).Open())
				{
					XDocument manifestFile = XDocument.Load(manifestFileStream);

					// Extract initial book pages
					IEnumerable<BookPage> bookPages = ExtractBookPages(archive, manifestFile.Root.Elements(CanvasPage));

					// Package the final book pages
					pages.Add(PackageCoverPage(bookPages.First()));
					pages.AddRange(PackageSoftPages(bookPages.Skip(1)));

					return pages;
				}
			}
		}


		#region Private methods

		private static IEnumerable<BookPage> ExtractBookPages(ZipArchive archive, IEnumerable<XElement> pages)
		{
			if (archive == null)
				throw new ArgumentNullException("archive");

			if (pages == null)
				throw new ArgumentNullException("pages");

			IEnumerable<BookPage> bookPages = pages.Select(page => ExtractBookPage(archive, page));

			return bookPages;
		}

		private static BookPage ExtractBookPage(ZipArchive archive, XElement pageElement)
		{
			if (archive == null)
				throw new ArgumentNullException("archive");

			if (pageElement == null)
				throw new ArgumentNullException("pageElement");

			BookPage page = new BookPage
			{
				Background = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, pageElement.Attribute("backgroundPath").Value).RemoveFirstSlash())),
				TextAssets = new ReadOnlyObservableCollection<TextAsset>(new ObservableCollection<TextAsset>(ExtractTextAssets(pageElement.Descendants(TextAssetTag)))),
				ImageAssets = new ReadOnlyObservableCollection<ImageAsset>(new ObservableCollection<ImageAsset>(ExtractImageAssets(archive, pageElement.Descendants(ImageAssetTag)))),
				KinectGame = ExtractKinectActivity(archive, pageElement.Descendants("GameAsset").SingleOrDefault(), pageElement)
			};

			return page;
		}

		private static void ExtractMusicAssets()
		{

		}

		private static IEnumerable<TextAsset> ExtractTextAssets(IEnumerable<XElement> textAssets)
		{
			if (textAssets == null)
				return null;

			if (textAssets.Any(textAsset => textAsset.Name != TextAssetTag))
				throw new InvalidOperationException();

			IEnumerable<TextAsset> assets = textAssets.Select(textAsset => new TextAsset
			{
				Position = new Position
				{
					X = Convert.ToDouble(textAsset.Attribute(TextAssetLeft).Value),
					Y = Convert.ToDouble(textAsset.Attribute(TextAssetTop).Value)
				},
				Text = textAsset.Attribute(TextAssetText).Value,
				FontSize = Convert.ToDouble(textAsset.Attribute(TextAssetFontSize).Value),
				// TODO: Add FontColor support
				FontFamily = textAsset.Attribute(TextAssetFontFamily).Value,
				IsBold = textAsset.Attribute(TextAssetFontIsBold).Value == "true" ? true : false,
				IsItalic = textAsset.Attribute(TextAssetFontIsItalic).Value == "true" ? true : false
			});

			return assets;
		}

		private static IEnumerable<ImageAsset> ExtractImageAssets(ZipArchive archive, IEnumerable<XElement> imageAssets)
		{
			if (archive == null)
				throw new ArgumentNullException("archive");

			if (imageAssets == null)
				return null;

			if (imageAssets.Any(imageAsset => imageAsset.Name != ImageAssetTag))
				throw new InvalidOperationException();

			IEnumerable<ImageAsset> assets = imageAssets.Select(imageAsset => new ImageAsset
			{
				Angle = Convert.ToDouble(imageAsset.Attribute(ImageAssetRotationAngle).Value),
				Content = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, imageAsset.Attribute(ImageAssetContentPath).Value).RemoveFirstSlash())),
				Position = new Position
				{
					X = Convert.ToDouble(imageAsset.Attribute(ImageAssetLeft).Value),
					Y = Convert.ToDouble(imageAsset.Attribute(ImageAssetTop).Value)
				},
				Size = Convert.ToInt32(imageAsset.Attribute(ImageAssetSize).Value),
				Layer = Convert.ToInt32(imageAsset.Attribute(LayerAttribute).Value)
			});

			return assets;
		}

		private void PackageBook(IEnumerable<BookPage> pages)
		{
			List<BookPage> bookPages = new List<BookPage>();

			bookPages.Add(PackageCoverPage(pages.First()));
			bookPages.AddRange(PackageSoftPages(pages.Skip(1)));
		}

		private static KinectActivity ExtractKinectActivity()
		{
			throw new NotImplementedException();
		}
		private static BookPage PackageCoverPage(BookPage page)
		{
			return new BookPage
			{
				Background = page.Background,
				ImageAssets = page.ImageAssets,
				KinectGame = page.KinectGame,
				Narration = page.Narration,
				Soundtrack = page.Soundtrack,
				TextAssets = page.TextAssets,
				Type = PageType.FrontCover,
			};
		}

		private static IEnumerable<BookPage> PackageSoftPages(IEnumerable<BookPage> pages)
		{
			List<BookPage> bookPages = new List<BookPage>();

			foreach (BookPage bookPage in pages)
			{
				bookPages.Add(new BookPage
				{
					Background = bookPage.Background,
					Position = BookPagePosition.Left,
					ImageAssets = bookPage.ImageAssets,
					KinectGame = bookPage.KinectGame,
					Narration = bookPage.Narration,
					Soundtrack = bookPage.Soundtrack,
					TextAssets = bookPage.TextAssets,
					Type = bookPage.KinectGame == null ? PageType.Soft : PageType.Interactive
				});

				bookPages.Add(new BookPage
				{
					Background = bookPage.Background,
					Position = BookPagePosition.Right,
					ImageAssets = bookPage.ImageAssets,
					KinectGame = bookPage.KinectGame,
					Narration = bookPage.Narration,
					Soundtrack = bookPage.Soundtrack,
					TextAssets = bookPage.TextAssets,
					Type = bookPage.KinectGame == null ? PageType.Soft : PageType.Interactive
				});
			}

			return bookPages;
		}

		private static KinectActivity ExtractKinectActivity(ZipArchive archive, XElement activityDescription, XElement page)
		{
			if (archive == null)
				throw new ArgumentNullException("archive");

			if (activityDescription == null)
				return null;

			switch (activityDescription.Attribute("type").Value)
			{
				case "OddOneOut":
					OddOneOutGame oddOneOutActivity = new OddOneOutGame { Name = "OddOneOutActivity" };
					ExtractOddOneOutActivity(archive, page.Descendants("OddOneOutSequence"), oddOneOutActivity);
					return oddOneOutActivity;
				case "SimonSays":
					SimonSaysGame simonSaysActivity = new SimonSaysGame { Name = "SimonSaysActivity" };
					ExtractSimonSaysActivity(archive, activityDescription, simonSaysActivity);
					return simonSaysActivity;
				case "GoNoGo":
					GoNoGoGame noGoNoGame = new GoNoGoGame { Name = "NoGoNoActivity" };
					ExtractNoGoNoActivity(archive, activityDescription, noGoNoGame);
					return noGoNoGame;
				case "VirtualDrumkit":
					break;
				default:
					break;
			}

			return null;
		}

		private static void ExtractNoGoNoActivity(ZipArchive archive, XElement game, GoNoGoGame activity)
		{
			NoGoNoActivity gameActivity = new NoGoNoActivity
			{
				new GameOption
				{
					Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, game.Attribute("GoNoGoFocusImage").Value).RemoveFirstSlash())),
					IsWrong = true
				},
				new GameOption
				{
					Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, game.Attribute("GoNoGoWrongImage1").Value).RemoveFirstSlash())),
					IsWrong = false
				},
				new GameOption
				{
					Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, game.Attribute("GoNoGoWrongImage2").Value).RemoveFirstSlash())),
					IsWrong = false
				},
				new GameOption
				{
					Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, game.Attribute("GoNoGoWrongImage3").Value).RemoveFirstSlash())),
					IsWrong = false
				}
			};

			activity.TriggerInterval = TimeSpan.FromSeconds(Convert.ToInt32(game.Attribute("TriggerInterval").Value));
			activity.Activities = new NoGoNoActivity[] { gameActivity };
		}

		private static void ExtractSimonSaysActivity(ZipArchive archive, XElement game, SimonSaysGame activity)
		{
			activity.Difficulty = (SimonSaysDifficulty)Convert.ToInt32(game.Attribute("numberOfPatterns").Value);
			activity.PatternTimeout = TimeSpan.FromSeconds(Convert.ToInt32(game.Attribute("secondsPerPattern").Value));
			activity.Tries = Convert.ToInt32(game.Attribute("numberOfTries").Value);
		}

		private static void ExtractOddOneOutActivity(ZipArchive archive, IEnumerable<XElement> sequencies, OddOneOutGame activity)
		{
			List<OddOneOutActivity> seqs = new List<OddOneOutActivity>();

			foreach (XElement sequence in sequencies)
			{
				OddOneOutActivity seq = new OddOneOutActivity
				{
					new GameOption
					{
						Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, sequence.Attribute("rightAnswerPath").Value).RemoveFirstSlash())),
						IsWrong = true
					},
					new GameOption
					{
						Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, sequence.Attribute("WrongAnswerPath1").Value).RemoveFirstSlash())),
						IsWrong = false
					},
					new GameOption
					{
						Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, sequence.Attribute("WrongAnswerPath2").Value).RemoveFirstSlash())),
						IsWrong = false
					},
					new GameOption
					{
						Option = new ImageBrush(StreamManipulation.ExtractBitmapImage(archive, string.Format(Images, sequence.Attribute("WrongAnswerPath3").Value).RemoveFirstSlash())),
						IsWrong = false
					}
				};

				seq.PositiveFeedback = sequence.Attribute("positiveFeedback").Value;
				seq.NegativeFeedback = sequence.Attribute("negativeFeedback").Value;

				seqs.Add(seq);
			}

			activity.Activities = seqs.ToArray();
		}

		#endregion
	}
}
