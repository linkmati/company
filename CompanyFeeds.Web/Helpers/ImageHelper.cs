using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CompanyFeeds.Web.Helpers
{
	public static class ImageHelper
	{
		/// <summary>
		/// Generates a image file in PNG format from the original file stream.
		/// </summary>
		public static void GenerateImage(Stream stream, int width, int height, string filePath)
		{
			Image image = Image.FromStream(stream);

			GenerateImage(image, width, height, filePath);

			image.Dispose();
		}


		/// <summary>
		/// Generates a image file in PNG format from an original image.
		/// </summary>
		public static void GenerateImage(Image originalImage, int width, int height, string filePath)
		{
			Image.GetThumbnailImageAbort dummyCallBack = new Image.GetThumbnailImageAbort(ThumbnailCallback);
			System.Drawing.Image thumbnailImage = originalImage.GetThumbnailImage(width, height, dummyCallBack, IntPtr.Zero);

			//Save the thumbnail in PNG format. 

			thumbnailImage.Save(filePath, ImageFormat.Png);
			thumbnailImage.Dispose();
		}

		private static ImageCodecInfo GetCodecInfo(string mimeType)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			ImageCodecInfo result = null;
			foreach (ImageCodecInfo c in codecs)
			{
				if (c.MimeType == mimeType)
				{
					result = c;
					break;
				}
			}

			return result;
		}

		private static bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// Resizes a image by width.
		/// </summary>
		public static void ResizeByWith(Stream stream, int maxWidth, string filePath)
		{
			#region Argument validation
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (maxWidth == 0)
			{
				throw new ArgumentException("maxWidth cannot be 0");
			} 
			#endregion
			string extension = Path.GetExtension(filePath);
			if (extension == null || extension == "")
			{
				throw new ArgumentException("filePath is not valid.");
			}

			Image originalImage = Image.FromStream(stream);
			int width = originalImage.Width;
			int height = originalImage.Height;
			if (originalImage.Width > maxWidth)
			{
				width = maxWidth;
				height = originalImage.Height * maxWidth / originalImage.Width;
			}
		
			GenerateImage(originalImage, width, height, filePath);

			originalImage.Dispose();
		}
	}
}
