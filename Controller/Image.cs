using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeudenn.Controller
{
#if NET5_0_OR_GREATER
   public sealed record Image
#else
   public sealed class Image
#endif
   {
      private Skeudenn.Image image;

      public Image<L8> ImageClone
      {
         get
         {
            return image.ImageClone;
         }
      }

      public Image(Skeudenn.Image image)
      {
         this.image = image;
      }
   }
}
