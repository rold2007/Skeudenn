using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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

      public Image(Controller.Image image)
      {
         this.image = image;
      }
   }
}
