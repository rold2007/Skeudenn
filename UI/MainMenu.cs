using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skeudenn.Controller;

namespace Skeudenn.UI
{
#if NET5_0_OR_GREATER
   public sealed record MainMenu
#else
   public sealed class MainMenu
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
         using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
         {
            Controller.Image image = fileOpen.OpenFile(fileStream);

            return new Image(image);
         }
      }

      public Image OpenFile(Stream imageStream)
      {
         Controller.Image image = fileOpen.OpenFile(imageStream);

         return new Image(image);
      }

      public string AboutText()
      {
         return helpAbout.Text();
      }
   }
}
