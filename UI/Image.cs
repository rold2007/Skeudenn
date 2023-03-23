﻿using System;
using System.Drawing;

// TODO Remove all nullable (ex: string?) types, at last they shouldn't be seen from theSkeudenn.UI layer. Also remove associated objectInstance!.Method().
namespace Skeudenn.UI
{
   public sealed record Image
   {
      private readonly Skeudenn.Image image;
      private readonly Zoom zoom = new Zoom();

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

      public Image(Skeudenn.Image image)
      {
         this.image = image;
      }

      public byte[] ImageData()
      {
         return image.ImageData();
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
