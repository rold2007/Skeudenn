using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;

namespace Skeudenn
{
   public sealed record Image
   {
      private Image<L8> image;

      public Image<L8> ImageClone
      {
         get
         {
            return image.Clone();
         }
      }

      private Image(Image<L8> image)
      {
         this.image = image;
      }

      public Image()
      {
      }

      public Image OpenFile(Stream imageStream)
      {
         return new Image(SixLabors.ImageSharp.Image.Load<L8>(imageStream));
      }
   }
}
