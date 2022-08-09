namespace Skeudenn
{
#if NET5_0_OR_GREATER
   public sealed record Version
#else
   public sealed class Version
#endif
    {
        public string Text
      {
         get
         {
            return "v1.0";
         }
      }
   }
}
