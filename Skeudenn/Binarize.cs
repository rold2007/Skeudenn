namespace Skeudenn
{
   using Shouldly;
   using SixLabors.ImageSharp;
   using SixLabors.ImageSharp.Processing;
   using SixLabors.ImageSharp.Processing.Processors.Binarization;
   using System;

   public sealed record Binarize // ncrunch: no coverage
   {
      public ImageProcessors? ImageProcessors
      {
         get;
         private set;
      }

      public Binarize()
      {
      }

      private Binarize(ImageProcessors imageProcessors)
      {
         imageProcessors.ShouldNotBeNull();
         ImageProcessors = imageProcessors;
      }

      public Binarize Update(ImageProcessors imageProcessors)
      {
         ImageProcessors.ShouldBeNull();

         return new Binarize(imageProcessors);
      }

      public void Apply(double threshold)
      {
         ImageProcessors.ShouldNotBeNull();

         float binaryThreshold = Convert.ToSingle(threshold / 255.0);

         BinaryThresholdProcessor binaryThresholdProcessor = new(binaryThreshold, Color.White, Color.Black, BinaryThresholdMode.Luminance);

         ImageProcessors.Add(this, binaryThresholdProcessor);
      }

      public void Remove()
      {
         ImageProcessors.ShouldNotBeNull();

         ImageProcessors.Remove(this);
      }
   }
}