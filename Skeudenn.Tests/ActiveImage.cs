using Shouldly;
using System;
using Xunit;

namespace Skeudenn.Tests
{
   public sealed class ActiveImage
   {
      [Fact]
      public void UpdateData()
      {
         bool updateData = false;
         ImageProcessors imageProcessors = new();
         UI.ActiveImage? activeImage = new UI.ActiveImage().Update(imageProcessors);

         void handler(object? s, EventArgs e)
         {
            updateData = true;
            activeImage!.UpdateData -= handler;
         }

         activeImage!.UpdateData += handler;

         UI.Binarize binarize = new UI.Binarize().Update(imageProcessors);

         binarize.Apply(128);

         updateData.ShouldBeTrue();

         updateData = false;

         activeImage.UpdateData -= handler;

         binarize.Apply(128);

         updateData.ShouldBeFalse();
      }
   }
}
