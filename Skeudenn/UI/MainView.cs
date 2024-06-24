using System.Collections.Generic;
using System.IO;

// TODO Add possibility to save as TIFF with NO compression. This is not supported by Paint.Net and I need it. This will also require to allow basic image editing.
namespace Skeudenn.UI
{
   public sealed record MainView // ncrunch: no coverage
   {
      private Version version = new();

      public bool CanExit()
      {
         return true;
      }

      public void Exit()
      {
      }

      public Image OpenFile(string path)
      {
         try
         {
            using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return OpenFile(fileStream);
         }
         catch
         {
            return new Image(Skeudenn.Image.OpenFile(null));
         }
      }

      public Image OpenFile(Stream imageStream)
      {
         return new Image(Skeudenn.Image.OpenFile(imageStream));
      }

      public List<Image> OpenFiles(string[] paths, out bool error)
      {
         List<Image> images = [];

         error = false;

         foreach (string path in paths)
         {
            Skeudenn.UI.Image skeudennImage = OpenFile(path);

            if (skeudennImage.Valid)
            {
               skeudennImage.Name = Path.GetFileName(path);
               images.Add(skeudennImage);
            }
            else
            {
               error = true;
            }
         }

         return images;
      }

      public string AboutText()
      {
         return version.Text;
      }
   }
}
