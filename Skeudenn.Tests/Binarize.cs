using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using Xunit;

namespace Skeudenn.Tests
{
   public sealed class Binarize
   {
      [Fact]
      public void Apply()
      {
         UI.Binarize binarize = new UI.Binarize();

         binarize.Apply(128);

         Size imageSize = new Size(1, 1);
         byte[] imagePixels = new byte[1];

         imagePixels[0] = 128;

         using (SixLabors.ImageSharp.Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height))
         {
            // UNDONE Need to restore image processors without using a static class
            Assert.Fail();
            //using (SixLabors.ImageSharp.Image<L8> resultImage = ImageProcessors.Instance.ProcessImage(image))
            //{
            //   Convert.ToInt32(resultImage[0, 0].PackedValue).ShouldBe(255);
            //}
         }
      }

      [Fact]
      public void Remove()
      {
         UI.Binarize binarize = new UI.Binarize();

         // Empty call to make sure it doesn't crash
         binarize.Remove();

         binarize.Apply(128);
         binarize.Remove();

         Size imageSize = new Size(1, 1);
         byte[] imagePixels = new byte[1];

         imagePixels[0] = 42;

         using (SixLabors.ImageSharp.Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height))
         {
            // UNDONE Need to restore image processors without using a static class
            Assert.Fail();
            //using (SixLabors.ImageSharp.Image<L8> resultImage = ImageProcessors.Instance.ProcessImage(image))
            //{
            //   Convert.ToInt32(resultImage[0, 0].PackedValue).ShouldBe(42);
            //}
         }
      }
   }
}
