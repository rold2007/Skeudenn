using Shouldly;

namespace Skeudenn.UI
{
   public sealed record Binarize
   {
      private Skeudenn.Binarize binarize;

      public Binarize()
      {
         binarize = new();
      }

      private Binarize(ImageProcessors imageProcessors)
      {
         binarize = new Skeudenn.Binarize().Update(imageProcessors);
      }

      public Binarize Update(ImageProcessors imageProcessors)
      {
         return new Binarize(imageProcessors);
      }

      public void Apply(double threshold)
      {
         binarize.Apply(threshold);
      }

      public void Remove()
      {
         binarize.Remove();
      }
   }
}
