using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kinemat.IO.Helpers
{
	public static class StreamManipulation
	{
		public static BitmapImage StreamToBitmapImage(Stream stream)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = new MemoryStream();
			stream.CopyTo(bitmapImage.StreamSource);
			bitmapImage.StreamSource.Position = 0;
			bitmapImage.EndInit();
			return bitmapImage;
		}

		public static BitmapImage ExtractBitmapImage(ZipArchive archive, string file)
		{
			using (Stream stream = archive.GetEntry(file).Open())
			{
				BitmapImage bitmapImage = StreamManipulation.StreamToBitmapImage(stream);
				return bitmapImage;
			}
		}
	}
}
