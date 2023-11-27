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
         EventHandler? handler = null;

         handler  = (s, e) =>
         {
            updateData = true;
            UI.ActiveImage.Instance.UpdateData -= handler;
         };

         UI.ActiveImage.Instance.UpdateData += handler;

         UI.Binarize binarize = new UI.Binarize();

         binarize.Apply(128);

         updateData.ShouldBeTrue();

         updateData = false;

         UI.ActiveImage.Instance.UpdateData -= handler;

         binarize.Apply(128);

         updateData.ShouldBeFalse();
      }
   }
}
