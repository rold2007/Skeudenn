using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skeudenn;

namespace Skeudenn.Controller
{
   public sealed record HelpAbout
   {
      private Version version = new Version();

      public string Text()
      {
         return version.Text;
      }
   }
}
