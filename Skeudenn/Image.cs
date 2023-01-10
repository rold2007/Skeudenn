using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Skeudenn
{
#if NET5_0_OR_GREATER
   public sealed record Image
#else
   public sealed class Image
#endif
   {
      private Image<L8> image;

      public System.Drawing.Size Size
      {
         get
         {
            return new System.Drawing.Size(image.Width, image.Height);
         }
      }

      private Image(Image<L8> image)
      {
         this.image = image;
      }

      static public Image OpenFile(Stream imageStream)
      {
         Image image = null;

         try
         {
            image = new Image(SixLabors.ImageSharp.Image.Load<L8>(imageStream));
         }
         catch
         {
         }

         return image;
      }

      public byte[] ImageData()
      {
         byte[] imageData = new byte[image.Width * image.Height];
         Span<byte> theSpan = new Span<byte>(imageData);

         image.CopyPixelDataTo(theSpan);

         return imageData;
      }
   }
}
