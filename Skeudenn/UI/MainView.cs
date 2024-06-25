using System.Collections.Generic;
using System.IO;

// TODO Add possibility to save as TIFF with NO compression. This is not supported by Paint.Net and I need it. This will also require to allow basic image editing.
namespace Skeudenn.UI
{
   // HACK There should never be more than one Skeudenn.UI.MainView instance...Be careful if any method needs to become non-static.
   public sealed record MainView // ncrunch: no coverage
   {
      public static bool CanExit()
      {
         return true;
      }

      public static void Exit()
      {
      }

      public static Image OpenFile(string path)
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

      public static Image OpenFile(Stream imageStream)
      {
         return new Image(Skeudenn.Image.OpenFile(imageStream));
      }

      public static List<Image> OpenFiles(string[] paths, out bool error)
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

      public static string AboutText()
      {
         return Version.Text;
      }
   }
}
