using System;
using System.Collections.Generic;
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
         return OpenImage(fileOpen.OpenFile(path));
      }

      public Image OpenFile(Stream imageStream)
      {
         return OpenImage(fileOpen.OpenFile(imageStream));
      }

      public List<Image> OpenFiles(string[] paths, out bool error)
      {
         List<Image> images = new List<Image>();

         error = false;

         // UNDONE Move the error/exception validation logic to Skeudenn.UI.MainView so that it can be tested and reused
         foreach (string path in paths)
         {
            Skeudenn.UI.Image skeudennImage = OpenFile(path);

            if (skeudennImage == null)
            {
               error = true;
            }
            else
            {
               skeudennImage.Name = System.IO.Path.GetFileName(path);
               images.Add(skeudennImage);
            }
         }

         return images;
      }

      public string AboutText()
      {
         return helpAbout.Text();
      }

      private Image OpenImage(Controller.Image image)
      {
         if (image != null)
         {
            return new Image(image);
         }

         return null;
      }
   }
}
