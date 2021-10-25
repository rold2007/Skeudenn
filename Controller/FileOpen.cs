using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skeudenn;

namespace Skeudenn.Controller
{
   public sealed record FileOpen
   {
      private Skeudenn.Image image = new Skeudenn.Image();

      public Image OpenFile(Stream imageStream)
      {
         return new Image(image.OpenFile(imageStream));
      }
   }
}
