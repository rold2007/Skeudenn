namespace Skeudenn.Controller
{
#if NET5_0_OR_GREATER
   public sealed record HelpAbout
#else
   public sealed class HelpAbout
#endif
   {
      private Version version = new Version();

      public string Text()
      {
         return version.Text;
      }
   }
}
