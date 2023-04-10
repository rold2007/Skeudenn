using System.Collections.Generic;
using System.IO;

// UNDONE Add a first image processing tool: binarize
namespace Skeudenn.UI
{
   public sealed record MainView
   {
      private Version version = new Version();

      public bool CanExit()
      {
         return true;
      }

      public void Exit()
      {
      }

      public Image? OpenFile(string path)
      {
         try
         {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
               return OpenFile(fileStream);
            }
         }
         catch
         {
            return null;
         }
      }

      public Image? OpenFile(Stream imageStream)
      {
         Skeudenn.Image? image = Skeudenn.Image.OpenFile(imageStream);

         return (image != null ? new Image(image) : null);
      }

      public List<Image> OpenFiles(string[] paths, out bool error)
      {
         List<Image> images = new List<Image>();

         error = false;

         foreach (string path in paths)
         {
            Skeudenn.UI.Image? skeudennImage = OpenFile(path);

            if (skeudennImage == null)
            {
               error = true;
            }
            else
            {
               skeudennImage.Name = Path.GetFileName(path);
               images.Add(skeudennImage);
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
