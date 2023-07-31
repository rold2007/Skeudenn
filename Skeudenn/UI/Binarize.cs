namespace Skeudenn.UI
{
   public sealed record Binarize
   {
      private Skeudenn.Binarize binarize = new Skeudenn.Binarize();

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
