using Godot;
using System;
using System.Drawing;

public partial class Image : VBoxContainer
{
   private TextureRect? textureRect;
   private Godot.Image? image;
   private Skeudenn.UI.Image? skeudennImage;

   public partial class TextureMouseMoveEventArgs : EventArgs
   {
      public TextureMouseMoveEventArgs(PointF pixelPosition)
      {
         PixelPosition = pixelPosition;
      }

      public PointF PixelPosition { get; private set; }
   }

   public event EventHandler<TextureMouseMoveEventArgs>? MouseMove;
   public event EventHandler? ZoomLevelChanged;

   public Skeudenn.UI.Image ImageUI
   {
      set
      {
         skeudennImage = value;

         byte[] imageData = skeudennImage.ImageData();

         image!.SetData(skeudennImage.Size.Width, skeudennImage.Size.Height, false, Godot.Image.Format.L8, imageData);

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

      Initialize();
   }

   private void Initialize()
   {
      image = new Godot.Image();
   }

   private void _on_ZoomIn_pressed()
   {
      if (skeudennImage != null)
      {
         // HACK Remove the texture blurring when zooming in. The easiest/fastest way might be to create a child image
         skeudennImage.ZoomIn();
         textureRect!.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
      }

      TriggerZoomChangedEvent();
   }

   private void _on_ZoomReset_pressed()
   {
      if (skeudennImage != null)
      {
         skeudennImage.ZoomReset();
         textureRect!.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
      }

      TriggerZoomChangedEvent();
   }

   private void _on_ZoomOut_pressed()
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
      EventHandler? handler = ZoomLevelChanged;

      if (handler != null)
      {
         handler(this, EventArgs.Empty);
      }
   }

   private void _on_TextureRect_gui_input(InputEvent inputEvent)
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
