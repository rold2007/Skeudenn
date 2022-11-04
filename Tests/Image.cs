using System;
using System.IO;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace Skeudenn.Tests
{
   public sealed class Image
   {
      static byte[] GenerateImageData(Size imageSize)
      {
         byte[] imagePixels = new byte[imageSize.Width * imageSize.Height];
         Random random = new Random();

         random.NextBytes(imagePixels);

         return imagePixels;
      }

      static MemoryStream GenerateImage(byte[] imagePixels, Size imageSize)
      {
         MemoryStream memoryStream = null;

         try
         {
            memoryStream = new MemoryStream();

            using (SixLabors.ImageSharp.Image<L8> tempImage = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height))
            {
               // UNDONE Save randomly in different formats
               tempImage.SaveAsBmp(memoryStream);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
         }
         catch
         {
            if (memoryStream != null)
            {
               memoryStream.Dispose();
               memoryStream = null;
            }
         }

         return memoryStream;
      }

      [Fact]
      public void ImageData()
      {
         Size imageSize = new Size(3, 5);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);

            image.Size.ShouldBe(imageSize);
            image.ImageData().ShouldBe(imagePixels);
         }
      }

      [Fact]
      public void PixelPosition()
      {
         Size imageSize = new Size(3, 5);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);

            System.Drawing.PointF windowPosition = new System.Drawing.PointF(42, 54);
            System.Drawing.PointF pixelPosition = image.PixelPosition(windowPosition);

            windowPosition.X.ShouldBe(42);
            windowPosition.Y.ShouldBe(54);
         }
      }

      [Fact]
      public void ZoomedSize()
      {
         Size imageSize = new Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);
         }
      }
   }
}
