using System.IO;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Skeudenn.Controller;
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
            tempImage[0, 0] = new L8(254);
            tempImage[2, 4] = new L8(253);

            using (MemoryStream memoryStream = new MemoryStream())
            {
               tempImage.SaveAsBmp(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               FileOpen fileOpen = new FileOpen();

               Controller.Image image = fileOpen.OpenFile(memoryStream);

               image.Size.Width.ShouldBe(3);
               image.Size.Height.ShouldBe(5);

               byte[] imageData = image.ImageData();

               // TODO Test all pixels instead of only first and last
               imageData[0].ShouldBe((byte)254);
               imageData[imageData.Length - 1].ShouldBe((byte)253);
            }
         }
      }
   }
}
