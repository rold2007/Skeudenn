using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Skeudenn
{
   public sealed record Image
   {
      private Image<L8>? image;

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

      public byte[] ImageData()
      {
         byte[] imageData = new byte[image!.Width * image!.Height];
         Span<byte> theSpan = new Span<byte>(imageData);

         // UNDONE Need to restore image processors without using a static class
         //using (Image<L8> destinationImage = ImageProcessors.Instance.ProcessImage(image))
         //{
         //   destinationImage.CopyPixelDataTo(theSpan);
         //}
         image.CopyPixelDataTo(theSpan);

         return imageData;
      }
   }
}
