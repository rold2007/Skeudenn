using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Skeudenn
{
   public sealed record Image // ncrunch: no coverage
   {
      private readonly Image<L8>? image;

      public System.Drawing.Size Size
      {
         get
         {
            return new System.Drawing.Size(image!.Width, image!.Height);
         }
      }

      public bool Valid
      {
         get
         {
            return image != null;
         }
      }

      private Image(Image<L8>? image)
      {
         this.image = image;
      }

      static public Image OpenFile(Stream? imageStream)
      {
         Image<L8>? image = null;

         try
         {
            if (imageStream != null)
            {
               image = SixLabors.ImageSharp.Image.Load<L8>(imageStream);
            }
         }
         catch
         {
         }

         return new Image(image);
      }

      public byte[] ImageData(Skeudenn.ImageProcessors imageProcessors)
      {
         byte[] imageData = new byte[image!.Width * image!.Height];
         Span<byte> theSpan = new(imageData);

         // TODO It might be better to deal with the ImageProcessors in some App Context class so that the
         // class using the UI layer doesn't have to send it each time it calls ImageData()
         using (Image<L8> destinationImage = imageProcessors.ProcessImage(image))
         {
            destinationImage.CopyPixelDataTo(theSpan);
         }

         return imageData;
      }
   }
}
