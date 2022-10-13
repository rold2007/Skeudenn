using System.IO;
using Skeudenn.Controller;

namespace Skeudenn.UI
{
#if NET5_0_OR_GREATER
   public sealed record MainView
#else
   public sealed class MainView
#endif
   {
      private FileOpen fileOpen = new FileOpen();
      private FileExit fileExit = new FileExit();
      private HelpAbout helpAbout = new HelpAbout();

      public bool CanExit()
      {
         return true;
      }

      public void Exit()
      {
         fileExit.Exit();
      }

      public Image OpenFile(string path)
      {
         return new Image(fileOpen.OpenFile(path));
      }

      public Image OpenFile(Stream imageStream)
      {
         return new Image(fileOpen.OpenFile(imageStream));
      }

      public string AboutText()
      {
         return helpAbout.Text();
      }
   }
}
