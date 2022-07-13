using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
