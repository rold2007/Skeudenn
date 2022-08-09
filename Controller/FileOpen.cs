using System.IO;

namespace Skeudenn.Controller
{
#if NET5_0_OR_GREATER
   public sealed record FileOpen
#else
   public sealed class FileOpen
#endif
   {
      private Skeudenn.Image image = new Skeudenn.Image();

      public Image OpenFile(string path)
      {
         using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
         {
            return OpenFile(fileStream);
         }
      }

      public Image OpenFile(Stream imageStream)
      {
         return new Image(image.OpenFile(imageStream));
      }
   }
}
