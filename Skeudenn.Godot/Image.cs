using Godot;
using System;
using System.Drawing;

namespace Skeudenn.Godot
{
   public partial class Image : VBoxContainer
   {
      private TextureRect? textureRect;
      private global::Godot.Image? image;
      private Skeudenn.UI.Image? skeudennImage;

      public partial class TextureMouseMoveEventArgs(PointF pixelPosition) : EventArgs
      {
         public PointF PixelPosition { get; private set; } = pixelPosition;
      }

      public event EventHandler<TextureMouseMoveEventArgs>? MouseMove;
      public event EventHandler? ZoomLevelChanged;

      public Skeudenn.UI.Image ImageUI
      {
         set
         {
            skeudennImage = value;

            UpdateData();

            // HACK Optimization: Keep an instance of TextureRect and call SetImage
            // HACK Optimization: Keep a cache of ImageTexture
            textureRect!.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
            textureRect!.Visible = true;
            textureRect!.Texture = ImageTexture.CreateFromImage(image);
         }
      }

      public float ZoomLevel
      {
         get
         {
            return skeudennImage!.ZoomLevel;
         }
      }

      public bool Valid
      {
         get
         {
            return textureRect!.Visible;
         }
      }

      public void Reset()
      {
         textureRect!.Visible = false;
         textureRect!.Texture = null;
         skeudennImage = null;
      }

      // Called when the node enters the scene tree for the first time.
      public override void _Ready()
      {
         textureRect = GetNode<TextureRect>("%TextureRect");

         Globals.ActiveImage.UpdateData += UpdateData;
         Globals.ActiveImage = Globals.ActiveImage.Update(Globals.ImageProcessors);

         Initialize();
      }

      private void UpdateData(object? sender, EventArgs e)
      {
         UpdateData();

         // HACK There's probably a more efficient way to do this without allocating a new texture but for now it works
         textureRect!.Texture = ImageTexture.CreateFromImage(image);
      }

      private void UpdateData()
      {
         if (skeudennImage != null)
         {
            byte[] imageData = skeudennImage!.ImageData(Globals.ImageProcessors);

            image!.SetData(skeudennImage!.Size.Width, skeudennImage!.Size.Height, false, global::Godot.Image.Format.L8, imageData);
         }
      }

      private void Initialize()
      {
         image = new global::Godot.Image();
      }

      private void OnZoomInPressed()
      {
         if (skeudennImage != null)
         {
            // HACK Remove the texture blurring when zooming in. The easiest/fastest way might be to create a child image
            skeudennImage.ZoomIn();
            textureRect!.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
         }

         TriggerZoomChangedEvent();
      }

      private void OnZoomResetPressed()
      {
         if (skeudennImage != null)
         {
            skeudennImage.ZoomReset();
            textureRect!.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
         }

         TriggerZoomChangedEvent();
      }

      private void OnZoomOutPressed()
      {
         if (skeudennImage != null)
         {
            skeudennImage.ZoomOut();
            textureRect!.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
         }

         TriggerZoomChangedEvent();
      }

      private void TriggerZoomChangedEvent()
      {
         ZoomLevelChanged?.Invoke(this, EventArgs.Empty);
      }

      private void OnTextureRectGUIInput(InputEvent inputEvent)
      {
         if (inputEvent is InputEventMouseMotion eventMouseMotion)
         {
            EventHandler<TextureMouseMoveEventArgs>? handler = MouseMove;

            if (handler != null)
            {
               PointF pixelPosition = skeudennImage!.PixelPosition(new PointF(eventMouseMotion.Position.X, eventMouseMotion.Position.Y));

               handler(this, new TextureMouseMoveEventArgs(pixelPosition));
            }
         }
      }
   }
}