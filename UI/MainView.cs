using System.Collections.Generic;
using System.IO;

namespace Skeudenn.UI
{
#if NET5_0_OR_GREATER
   public sealed record MainView
#else
   public sealed class MainView
#endif
   {
      private Version version = new Version();

      public bool CanExit()
      {
         return true;
      }

      public void Exit()
      {
      }

      public Image OpenFile(string path)
      {
         return OpenImage(ControllerOpenFile(path));
      }

      public Image OpenFile(Stream imageStream)
      {
         return OpenImage(ControllerOpenFile(imageStream));
      }

      public List<Image> OpenFiles(string[] paths, out bool error)
      {
         List<Image> images = new List<Image>();

         error = false;

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
         return version.Text;
      }

      private Image OpenImage(Controller.Image image)
      {
         if (image != null)
         {
            return new Image(image);
         }

         return null;
      }

      // UNDONE Rename/remove once the Controller layer is removed
      private Skeudenn.Controller.Image ControllerOpenFile(string path)
      {
         try
         {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
               return ControllerOpenFile(fileStream);
            }
         }
         catch
         {
            return null;
         }
      }

      // UNDONE Rename/remove once the Controller layer is removed
      private Skeudenn.Controller.Image ControllerOpenFile(Stream imageStream)
      {
         Skeudenn.Image image = Skeudenn.Image.OpenFile(imageStream);

         if (image != null)
         {
            return new Skeudenn.Controller.Image(image);
         }

         return null;
      }
   }
}
