using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeudenn
{
#if NET5_0_OR_GREATER
   public sealed record Zoom
#else
   public sealed class Zoom
#endif

   {
      private int zoomIndex;
      private List<int> zoomLevels = new List<int>() { 2, 3, 4, 5, 6, 7, 8, 10, 13, 17, 20, 25, 33, 50, 67, 100, 150, 200, 300, 400, 500, 600, 1000, 1200, 1400, 1600, 2000, 2400, 2800, 3200, 4000, 4800, 5600, 6400 };

      public int ZoomLevel
      {
         get
         {
            return zoomLevels[zoomIndex];
         }
      }

      public Zoom()
      {
         ZoomReset();
      }

      public void ZoomIn()
      {
         zoomIndex = Math.Min(zoomIndex + 1, zoomLevels.Count - 1);
      }

      public void ZoomOut()
      {
         zoomIndex = Math.Max(zoomIndex - 1, 0);
      }

      public void ZoomReset()
      {
         zoomIndex = 15;
      }
   }
}
