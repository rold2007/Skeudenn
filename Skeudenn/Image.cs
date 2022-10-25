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

      public Image<L8> ImageClone
      {
         get
         {
            return image.Clone();
         }
      }

      public Size Size
      {
         get
         {
            return new Size(image.Width, image.Height);
         }
      }

      private Image(Image<L8> image)
      {
         this.image = image;
      }

      static public Image OpenFile(Stream imageStream)
      {
         // TODO Handle UnknownImageFormatException and any other potential exception from Load()
         return new Image(SixLabors.ImageSharp.Image.Load<L8>(imageStream));
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
