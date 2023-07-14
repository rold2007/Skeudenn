namespace Skeudenn.UI
{
   public sealed record Binarize
   {
      private Skeudenn.Binarize binarize = new Skeudenn.Binarize();

      public void Apply(double threshold)
      {
         // UNDONE Need to force a refresh on the current image
         binarize.Apply(threshold);
      }
   }
}
