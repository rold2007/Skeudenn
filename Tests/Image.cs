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
      [Fact]
      public void ImageData()
      {
         using (SixLabors.ImageSharp.Image<L8> tempImage = new SixLabors.ImageSharp.Image<L8>(3, 5))
         {
            Random random = new Random();

            for (int y = 0; y < 5; y++)
            {
               for (int x = 0; x < 3; x++)
               {
                  tempImage[x, y] = new L8((byte)random.Next(0, 256));
               }
            }

            tempImage[0, 0] = new L8(254);
            tempImage[2, 4] = new L8(253);

            using (MemoryStream memoryStream = new MemoryStream())
            {
               tempImage.SaveAsBmp(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               image.Size.Width.ShouldBe(3);
               image.Size.Height.ShouldBe(5);

               byte[] imageData = image.ImageData();

               imageData[0].ShouldBe((byte)254);
               imageData[imageData.Length - 1].ShouldBe((byte)253);

               for (int y = 0; y < 5; y++)
               {
                  for (int x = 0; x < 3; x++)
                  {
                     imageData[y * 3 + x].ShouldBe(tempImage[x, y].PackedValue);
                  }
               }
            }
         }
      }

      [Fact]
      public void PixelPosition()
      {
         using (SixLabors.ImageSharp.Image<L8> tempImage = new SixLabors.ImageSharp.Image<L8>(3, 5))
         {
            tempImage[0, 0] = new L8(254);
            tempImage[2, 4] = new L8(253);

            using (MemoryStream memoryStream = new MemoryStream())
            {
               tempImage.SaveAsBmp(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               UI.MainView mainView = new UI.MainView();
               UI.Image image = mainView.OpenFile(memoryStream);

               System.Drawing.PointF windowPosition = new System.Drawing.PointF(42, 54);
               System.Drawing.PointF pixelPosition = image.PixelPosition(windowPosition);

               windowPosition.X.ShouldBe(42);
               windowPosition.Y.ShouldBe(54);
            }
         }
      }
   }
}
