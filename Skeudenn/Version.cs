using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeudenn
{
   public sealed record Version
   {
      public string Text
      {
         get
         {
            return "v1.0";
         }
      }
   }
}
