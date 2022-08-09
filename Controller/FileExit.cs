namespace Skeudenn.Controller
{
#if NET5_0_OR_GREATER

   public sealed record FileExit
#else
   public sealed class FileExit
#endif
   {
      public void Exit()
      {
      }
   }
}
