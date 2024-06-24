using Shouldly;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Drawing;
using Xunit;

namespace Skeudenn.Tests
{
   public sealed class Binarize
   {
      [Fact]
      public void Apply()
      {
         ImageProcessors imageProcessors = new();
         UI.Binarize binarize = new UI.Binarize().Update(imageProcessors);

         binarize.Apply(128);

         Size imageSize = new(1, 1);
         byte[] imagePixels = [128];
         using SixLabors.ImageSharp.Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height);
         using SixLabors.ImageSharp.Image<L8> resultImage = imageProcessors.ProcessImage(image);
         Convert.ToInt32(resultImage[0, 0].PackedValue).ShouldBe(255);
      }

      [Fact]
      public void Remove()
      {
         ImageProcessors imageProcessors = new();
         UI.Binarize binarize = new UI.Binarize().Update(imageProcessors);

         // Empty call to make sure it doesn't crash
         binarize.Remove();

         binarize.Apply(128);
         binarize.Remove();

         Size imageSize = new(1, 1);
         byte[] imagePixels = [42];
         using SixLabors.ImageSharp.Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imagePixels, imageSize.Width, imageSize.Height);
         using SixLabors.ImageSharp.Image<L8> resultImage = imageProcessors.ProcessImage(image);
         Convert.ToInt32(resultImage[0, 0].PackedValue).ShouldBe(42);
      }
   }
}
