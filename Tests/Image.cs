﻿using System;
using System.IO;
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
      static byte[] GenerateImageData(System.Drawing.Size imageSize)
      {
         byte[] imagePixels = new byte[imageSize.Width * imageSize.Height];
         Random random = new Random();

         random.NextBytes(imagePixels);

         return imagePixels;
      }

      static MemoryStream GenerateImage(byte[] imagePixels, System.Drawing.Size imageSize)
      {
         MemoryStream memoryStream = null;

         try
         {
            memoryStream = new MemoryStream();

            using (SixLabors.ImageSharp.Image<L8> tempImage = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height))
            {
               IImageFormat[] lossLessImageFormat = { BmpFormat.Instance, PbmFormat.Instance, PngFormat.Instance, TgaFormat.Instance, TiffFormat.Instance };

#if NET5_0_OR_GREATER
               tempImage.Save(memoryStream, tempImage.GetConfiguration().ImageFormatsManager.FindEncoder(lossLessImageFormat[Random.Shared.Next(lossLessImageFormat.Length)]));
#else
               tempImage.Save(memoryStream, tempImage.GetConfiguration().ImageFormatsManager.FindEncoder(lossLessImageFormat[new Random().Next(lossLessImageFormat.Length)]));
#endif
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
         }
         //ncrunch: no coverage start
         catch
         {
            if (memoryStream != null)
            {
               memoryStream.Dispose();
               memoryStream = null;
            }
         }
         //ncrunch: no coverage end

         return memoryStream;
      }

      [Fact]
      public void ImageData()
      {
         System.Drawing.Size imageSize = new System.Drawing.Size(3, 5);
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
         System.Drawing.Size imageSize = new System.Drawing.Size(3, 5);
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
         System.Drawing.Size imageSize = new System.Drawing.Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);

            image.ZoomedSize.ShouldBe(imageSize);

            image.ZoomIn();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(8, 10));

            image.ZoomOut();

            image.ZoomedSize.ShouldBe(imageSize);

            image.ZoomOut();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(3, 5));
         }
      }


      [Fact]
      public void ZoomIn()
      {
         System.Drawing.Size imageSize = new System.Drawing.Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);

            image.ZoomedSize.ShouldBe(imageSize);

            image.ZoomIn();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(8, 10));

            for (int i = 0; i < 20; i++)
            {
               image.ZoomIn();
            }

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(320, 448));

            image.ZoomIn();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(320, 448));
         }
      }

      [Fact]
      public void ZoomOut()
      {
         System.Drawing.Size imageSize = new System.Drawing.Size(500, 700);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);

            image.ZoomedSize.ShouldBe(imageSize);

            image.ZoomOut();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(335, 469));

            for (int i = 0; i < 20; i++)
            {
               image.ZoomOut();
            }

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(10, 14));

            image.ZoomOut();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(10, 14));
         }
      }

      [Fact]
      public void ZoomReset()
      {
         System.Drawing.Size imageSize = new System.Drawing.Size(5, 7);
         byte[] imagePixels = GenerateImageData(imageSize);

         using (MemoryStream memoryStream = GenerateImage(imagePixels, imageSize))
         {
            UI.MainView mainView = new UI.MainView();
            UI.Image image = mainView.OpenFile(memoryStream);

            image.ZoomedSize.ShouldBe(imageSize);

            image.ZoomIn();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(8, 10));

            image.ZoomReset();

            image.ZoomedSize.ShouldBe(imageSize);

            image.ZoomOut();

            image.ZoomedSize.ShouldBe(new System.Drawing.Size(3, 5));

            image.ZoomReset();

            image.ZoomedSize.ShouldBe(imageSize);
         }
      }
   }
}
