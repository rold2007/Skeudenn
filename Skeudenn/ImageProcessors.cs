using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing.Processors;
using System.Collections.Generic;
using System;

namespace Skeudenn
{
   public sealed record ImageProcessors
   {
      private static ImageProcessors? instance;

      private List<object> uniqueProcessors = new List<object>();
      private List<IImageProcessor> imageProcessors = new List<IImageProcessor>();

      public event EventHandler? ImageProcessorChanged;

      private ImageProcessors()
      {
      }

      public static ImageProcessors Instance
      {
         get
         {
            instance ??= new ImageProcessors();

            return instance;
         }
      }

      // UNDONE Add tests and bring back code coverage to 100%
      public void Add(object uniqueSource, IImageProcessor imageProcessor)
      {
         int index = uniqueProcessors.FindIndex((item) => object.ReferenceEquals(item, uniqueSource));

         if (index != -1)
         {
            uniqueProcessors.RemoveAt(index);
            imageProcessors.RemoveAt(index);
         }

         uniqueProcessors.Add(uniqueSource);
         imageProcessors.Add(imageProcessor);

         EventHandler? imageProcessorChanged = ImageProcessorChanged;

         if(imageProcessorChanged != null)
         {
            imageProcessorChanged(this, EventArgs.Empty);
         }
      }

      public Image<L8> ProcessImage(Image<L8> sourceImage)
      {
         if (imageProcessors.Count == 0)
         {
            return sourceImage;
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
