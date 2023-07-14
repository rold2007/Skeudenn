namespace Skeudenn
{
   using SixLabors.ImageSharp;
   using SixLabors.ImageSharp.Processing;
   using SixLabors.ImageSharp.Processing.Processors.Binarization;
   using System;

   public sealed record Binarize
   {
      public void Apply(double threshold)
      {
         float binaryThreshold = Convert.ToSingle(threshold / 255.0);
         BinaryThresholdProcessor binaryThresholdProcessor = new BinaryThresholdProcessor(binaryThreshold, Color.Black, Color.White, BinaryThresholdMode.Luminance);

         ImageProcessors.Instance.Add(this, binaryThresholdProcessor);
      }
   }
}