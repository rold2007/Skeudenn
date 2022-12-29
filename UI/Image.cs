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

      public float ZoomLevel
      {
         get
         {
            return image.ZoomLevel;
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

      public PointF PixelPosition(PointF windowPosition)
      {
         return new PointF((float)Math.Floor(windowPosition.X / (image.ZoomLevel / 100.0f)), (float)Math.Floor(windowPosition.Y / (image.ZoomLevel / 100.0f)));
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
