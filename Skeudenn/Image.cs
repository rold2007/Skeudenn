using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

// UNDONE Upgrade to ImageSharp 3.0
namespace Skeudenn
{
   public sealed record Image
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
