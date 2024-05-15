namespace Skeudenn
{
   using Shouldly;
   using SixLabors.ImageSharp;
   using SixLabors.ImageSharp.Processing;
   using SixLabors.ImageSharp.Processing.Processors.Binarization;
   using System;

   public sealed record Binarize
   {
      private ImageProcessors? imageProcessors;

      public Binarize()
      {
      }

      private Binarize(ImageProcessors imageProcessors)
      {
         this.imageProcessors = imageProcessors;
      }

      public Binarize ImageProcessors(ImageProcessors imageProcessors)
      {
         this.imageProcessors.ShouldBeNull();

         return new Binarize(imageProcessors);
      }

      public void Apply(double threshold)
      {
         imageProcessors.ShouldNotBeNull();

         float binaryThreshold = Convert.ToSingle(threshold / 255.0);

         BinaryThresholdProcessor binaryThresholdProcessor = new BinaryThresholdProcessor(binaryThreshold, Color.White, Color.Black, BinaryThresholdMode.Luminance);

         imageProcessors.Add(this, binaryThresholdProcessor);
      }

      public void Remove()
      {
         imageProcessors.ShouldNotBeNull();

         imageProcessors.Remove(this);
      }
   }
}