using System.Drawing;

namespace Skeudenn.Controller
{
#if NET5_0_OR_GREATER
   public sealed record Image
#else
   public sealed class Image
#endif
   {
      private readonly Skeudenn.Image image;
      private readonly Zoom zoom = new Zoom();

      public Size Size
      {
         get
         {
            return image.Size;
         }
      }

      public float ZoomLevel
      {
         get
         {
            return zoom.ZoomLevel;
         }
      }

      public Image(Skeudenn.Image image)
      {
         this.image = image;
      }

      public byte[] ImageData()
      {
         return image.ImageData();
      }

      public void ZoomIn()
      {
         zoom.ZoomIn();
      }

      public void ZoomOut()
      {
         zoom.ZoomOut();
      }

      public void ZoomReset()
      {
         zoom.ZoomReset();
      }
   }
}
