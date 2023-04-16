using System;
using System.IO;
using System.Drawing;
using Shouldly;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
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
         MemoryStream memoryStream = new MemoryStream();

         try
         {
            using (SixLabors.ImageSharp.Image<L8> tempImage = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height))
            {
               IImageFormat[] lossLessImageFormat = { BmpFormat.Instance, PbmFormat.Instance, PngFormat.Instance, TgaFormat.Instance, TiffFormat.Instance };

               tempImage.Save(memoryStream, tempImage.GetConfiguration().ImageFormatsManager.GetEncoder(lossLessImageFormat[Random.Shared.Next(lossLessImageFormat.Length)]));
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
         }
         //ncrunch: no coverage start
         catch
         {
            memoryStream.Dispose();
            memoryStream = new MemoryStream();
         }
         //ncrunch: no coverage end

         return memoryStream;
      }

      [Fact]
      public void ImageData()
      {
         Size imageSize = new Size(3, 5);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.Size.ShouldBe(imageSize);
               image.ImageData().ShouldBe(imagePixels);
            }
         }
      }

      [Fact]
      public void PixelPosition()
      {
         Size imageSize = new Size(3, 5);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               PointF windowPosition = new PointF(42, 54);
               PointF pixelPosition = image.PixelPosition(windowPosition);

               pixelPosition.X.ShouldBe(42);
               pixelPosition.Y.ShouldBe(54);
            }
         }
      }

      [Fact]
      public void PixelPositionZoomed()
      {
         Size imageSize = new Size(3, 5);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.ZoomIn();
               image.ZoomIn();

               PointF windowPosition = new PointF(42, 54);
               PointF pixelPosition = image.PixelPosition(windowPosition);

               pixelPosition.X.ShouldBe(21);
               pixelPosition.Y.ShouldBe(27);
            }
         }
      }

      [Fact]
      public void ZoomedSize()
      {
         Size imageSize = new Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.ZoomedSize.ShouldBe(imageSize);

               image.ZoomIn();

               image.ZoomedSize.ShouldBe(new Size(8, 10));

               image.ZoomOut();

               image.ZoomedSize.ShouldBe(imageSize);

               image.ZoomOut();

               image.ZoomedSize.ShouldBe(new Size(3, 5));
            }
         }
      }

      [Fact]
      public void ZoomIn()
      {
         Size imageSize = new Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.ZoomedSize.ShouldBe(imageSize);

               image.ZoomIn();

               image.ZoomedSize.ShouldBe(new Size(8, 10));

               for (int i = 0; i < 20; i++)
               {
                  image.ZoomIn();
               }

               image.ZoomedSize.ShouldBe(new Size(320, 448));

               image.ZoomIn();

               image.ZoomedSize.ShouldBe(new Size(320, 448));
            }
         }
      }

      [Fact]
      public void ZoomOut()
      {
         Size imageSize = new Size(500, 700);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.ZoomedSize.ShouldBe(imageSize);

               image.ZoomOut();

               image.ZoomedSize.ShouldBe(new Size(335, 469));

               for (int i = 0; i < 20; i++)
               {
                  image.ZoomOut();
               }

               image.ZoomedSize.ShouldBe(new Size(10, 14));

               image.ZoomOut();

               image.ZoomedSize.ShouldBe(new Size(10, 14));
            }
         }
      }

      [Fact]
      public void ZoomReset()
      {
         Size imageSize = new Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.ZoomedSize.ShouldBe(imageSize);

               image.ZoomIn();

               image.ZoomedSize.ShouldBe(new Size(8, 10));

               image.ZoomReset();

               image.ZoomedSize.ShouldBe(imageSize);

               image.ZoomOut();

               image.ZoomedSize.ShouldBe(new Size(3, 5));

               image.ZoomReset();

               image.ZoomedSize.ShouldBe(imageSize);
            }
         }
      }

      [Fact]
      public void ZoomLevel()
      {
         Size imageSize = new Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.ZoomLevel.ShouldBe(100);

               image.ZoomIn();

               image.ZoomLevel.ShouldBe(150);

               image.ZoomReset();

               image.ZoomLevel.ShouldBe(100);

               image.ZoomOut();

               image.ZoomLevel.ShouldBe(67);

               image.ZoomReset();

               image.ZoomLevel.ShouldBe(100);
            }
         }
      }

      [Fact]
      public void Name()
      {
         Size imageSize = new Size(1, 1);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            if (memoryStream.Length > 0)
            {
               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.Name.ShouldBeEmpty();
               image.ToString().ShouldBeEmpty();
               image.Name = "Dummy1";
               image.Name.ShouldBe("Dummy1");
               image.ToString().ShouldBe("Dummy1");
               image.Name = "Dummy2";
               image.Name.ShouldBe("Dummy2");
               image.ToString().ShouldBe("Dummy2");
            }
         }
      }
   }
}
