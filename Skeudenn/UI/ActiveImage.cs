using System;

namespace Skeudenn.UI
{
   // HACK Find a better class name. Rename tests too...
   public sealed record ActiveImage
   {
      public event EventHandler? UpdateData;

      // UNDONE Remove this static instance. It makes the unit test more complicated.
      private static ActiveImage? instance;

      private ActiveImage()
      {
      }

      public static ActiveImage Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new ActiveImage();

               // UNDONE Need to restore image processors without using a static class
               //ImageProcessors.Instance.ImageProcessorChanged += instance.Instance_ImageProcessorChanged;
            }

            return instance;
         }
      }

      private void Instance_ImageProcessorChanged(object? sender, EventArgs e)
      {
         EventHandler? updateData = UpdateData;

         if (updateData != null)
         {
            updateData.Invoke(sender, e);
         }
      }
   }
}
