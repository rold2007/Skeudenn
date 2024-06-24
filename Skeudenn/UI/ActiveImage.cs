using System;

namespace Skeudenn.UI
{
   // HACK Find a better class name. Rename tests too...
   public sealed record ActiveImage // ncrunch: no coverage
   {
      public event EventHandler? UpdateData;

      public ActiveImage()
      {
      }

      public ActiveImage Update(ImageProcessors imageProcessors)
      {
         imageProcessors.ImageProcessorChanged -= Instance_ImageProcessorChanged;
         imageProcessors.ImageProcessorChanged += Instance_ImageProcessorChanged;

         return this;
      }

      private void Instance_ImageProcessorChanged(object? sender, EventArgs e)
      {
         UpdateData?.Invoke(sender, e);
      }
   }
}
