namespace Skeudenn.UI
{
   public sealed record Binarize // ncrunch: no coverage
   {
      private readonly Skeudenn.Binarize binarize;

      public Binarize()
      {
         binarize = new();
      }

      private Binarize(Skeudenn.Binarize binarize)
      {
         this.binarize = binarize;
      }

      public Binarize Update(ImageProcessors imageProcessors)
      {
         return new Binarize(binarize.Update(imageProcessors));
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
