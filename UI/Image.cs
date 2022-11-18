using System;
using System.Drawing;

namespace Skeudenn.UI
{
#if NET5_0_OR_GREATER
   public sealed record Image
#else
   public sealed class Image
#endif
   {
      private Controller.Image image;

      public Size Size
      {
         get
         {
            return image.Size;
         }
      }

      public Size ZoomedSize
      {
         get
         {
            return new Size(Convert.ToInt32(Size.Width * image.ZoomLevel / 100.0), Convert.ToInt32(Size.Height * image.ZoomLevel / 100.0));
         }
      }

      public Image(Controller.Image image)
      {
         this.image = image;
      }

      public byte[] ImageData()
      {
         return image.ImageData();
      }

      public System.Drawing.PointF PixelPosition(System.Drawing.PointF windowPosition)
      {
         return windowPosition;
      }

      public void ZoomIn()
      {
         image.ZoomIn();
      }

      public void ZoomOut()
      {
         image.ZoomOut();
      }

      public void ZoomReset()
      {
         image.ZoomReset();
      }
   }
}
