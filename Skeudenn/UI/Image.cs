using System;
using System.Drawing;

namespace Skeudenn.UI
{
   public sealed record Image // ncrunch: no coverage
   {
      private readonly Skeudenn.Image image;
      private readonly Zoom zoom = new();

      public Size Size
      {
         get
         {
            return image.Size;
         }
      }

      public Size ZoomedSize
      {
         get
         {
            return new Size(Convert.ToInt32(Size.Width * zoom.ZoomLevel / 100.0), Convert.ToInt32(Size.Height * zoom.ZoomLevel / 100.0));
         }
      }

      public float ZoomLevel
      {
         get
         {
            return zoom.ZoomLevel;
         }
      }

      // HACK This will need to be managed at a lower level (not UI) once it is possible to save/rename tabs/files.
      public string Name
      {
         get; set;
      } = string.Empty;

      public bool Valid
      {
         get
         {
            return image.Valid;
         }
      }

      public Image(Skeudenn.Image image)
      {
         this.image = image;
      }

      public byte[] ImageData(Skeudenn.ImageProcessors imageProcessors)
      {
         return image.ImageData(imageProcessors);
      }

      // To prevent crash when calling the default version on an invalid image
      public override string ToString()
      {
         return Name;
      }

      public PointF PixelPosition(PointF windowPosition)
      {
         return new PointF((float)Math.Floor(windowPosition.X / (zoom.ZoomLevel / 100.0f)), (float)Math.Floor(windowPosition.Y / (zoom.ZoomLevel / 100.0f)));
      }

      public void ZoomIn()
      {
         zoom.ZoomIn();
      }

      public void ZoomOut()
      {
         zoom.ZoomOut();
      }

      public void ZoomReset()
      {
         zoom.ZoomReset();
      }
   }
}
