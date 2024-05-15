using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing.Processors;
using System.Collections.Generic;
using System;

namespace Skeudenn
{
   public sealed record ImageProcessors
   {
      private List<object> uniqueProcessors = new List<object>();
      private List<IImageProcessor> imageProcessors = new List<IImageProcessor>();

      public event EventHandler? ImageProcessorChanged;

      public ImageProcessors()
      {
      }

      // UNDONE Churn tests as I think one of them fails one in a while
      public void Add(object uniqueSource, IImageProcessor imageProcessor)
      {
         Update(uniqueSource, imageProcessor);
      }

      public void Remove(object uniqueSource)
      {
         Update(uniqueSource, null);
      }

      private void Update(object uniqueSource, IImageProcessor? imageProcessor)
      {
         bool update = false;
         int index = uniqueProcessors.FindIndex((item) => object.ReferenceEquals(item, uniqueSource));

         if (index != -1)
         {
            uniqueProcessors.RemoveAt(index);
            imageProcessors.RemoveAt(index);
            update = true;
         }

         if (imageProcessor != null)
         {
            uniqueProcessors.Add(uniqueSource);
            imageProcessors.Add(imageProcessor);
            update = true;
         }

         if (update)
         { 
            EventHandler? imageProcessorChanged = ImageProcessorChanged;

            if (imageProcessorChanged != null)
            {
               imageProcessorChanged(this, EventArgs.Empty);
            }
         }
      }

      public Image<L8> ProcessImage(Image<L8> sourceImage)
      {
         if (imageProcessors.Count == 0)
         {
            // Always return a new image to be able to wrap ProcessImage() with using
            return sourceImage.Clone();
         }
         else
         {
            Image<L8> destinationImage = sourceImage.Clone();
            Rectangle rectangle = Rectangle.FromLTRB(0, 0, sourceImage.Width, sourceImage.Height);

            foreach (IImageProcessor imageProcessor in imageProcessors)
            {
               imageProcessor.CreatePixelSpecificProcessor<L8>(Configuration.Default, destinationImage, rectangle).Execute();
            }

            return destinationImage;
         }
      }
   }
}
