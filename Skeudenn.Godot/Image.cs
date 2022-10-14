using Godot;
using System;
using System.Drawing;

public class Image : VBoxContainer
{
   private TextureRect textureRect;
   private ImageTexture imageTexture;
   private Godot.Image image;
   private Skeudenn.UI.Image skeudennImage;

   public class TextureMouseMoveEventArgs : EventArgs
   {
      public TextureMouseMoveEventArgs(PointF pixelPosition)
      {
         PixelPosition = pixelPosition;
      }

      public PointF PixelPosition { get; private set; }
   }

   public event EventHandler<TextureMouseMoveEventArgs> MouseMove;

   public Skeudenn.UI.Image ImageUI
   {
      set
      {
         skeudennImage = value;

         byte[] imageData = skeudennImage.ImageData();

         image.CreateFromData(skeudennImage.Size.Width, skeudennImage.Size.Height, false, Godot.Image.Format.L8, imageData);
         imageTexture.CreateFromImage(image);
         textureRect.RectMinSize = new Vector2(skeudennImage.Size.Width, skeudennImage.Size.Height);
         textureRect.Visible = true;
      }
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
    {
      textureRect = GetNode("%TextureRect") as TextureRect;
      imageTexture = new ImageTexture();
      textureRect.Texture = imageTexture;
      image = new Godot.Image();
      imageTexture.Storage = ImageTexture.StorageEnum.CompressLossless;
   }

   private void _on_ZoomIn_pressed()
   {
      // TODO Make the scroll bars appear when the image is too big to fit the ScrollContainer
      // TODO Remove the texture blurring when zooming in. The easiest/fastest way might be to create a child image
      // TODO Apply zoom in/out steps like in Paint.Net
      // TODO Limit the zoom in/out min/max ratios
      textureRect.RectMinSize *= 1.5f;
   }

   private void _on_ZoomReset_pressed()
   {
      Vector2 imageSize = image.GetSize();

      // TODO Fix the image which disappear when changing the window size. Using the reset zoom works a workaround so it is probably related to the RectSize
      textureRect.RectMinSize = imageSize;
   }

   private void _on_ZoomOut_pressed()
   {
      textureRect.RectMinSize /= 1.5f;
   }

   private void _on_TextureRect_gui_input(object inputEvent)
   {
      if (inputEvent is InputEventMouseMotion eventMouseMotion)
      {
         // TODO Take the zoom/scrollbar into account to remap to the image pixel position
         EventHandler<TextureMouseMoveEventArgs> handler = MouseMove;

         if (handler != null)
         {
            PointF pixelPosition = skeudennImage.PixelPosition(new PointF(eventMouseMotion.Position.x, eventMouseMotion.Position.y));

            handler(this, new TextureMouseMoveEventArgs(pixelPosition));
         }
      }
   }
}
