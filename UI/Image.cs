using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Skeudenn.UI
{
#if NET5_0_OR_GREATER
   public sealed record Image
#else
   public sealed class Image
#endif
   {
      private Controller.Image image;

      public Image<L8> ImageClone
      {
         get
         {
            return image.ImageClone;
         }
      }

      public Size Size
      {
         get
         {
            return image.Size;
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
   }
}
