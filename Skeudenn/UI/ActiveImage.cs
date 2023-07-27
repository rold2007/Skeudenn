using System;

namespace Skeudenn.UI
{
   // HACK Find a better class name
   public sealed record ActiveImage
   {
      public event EventHandler? UpdateData;

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

               ImageProcessors.Instance.ImageProcessorChanged += instance.Instance_ImageProcessorChanged;
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
