using Godot;
using System;
using System.Drawing;

public partial class Image : VBoxContainer
{
   private TextureRect textureRect;
   private ImageTexture imageTexture;
   private Godot.Image image;
   private Skeudenn.UI.Image skeudennImage;

   public partial class TextureMouseMoveEventArgs : EventArgs
   {
      public TextureMouseMoveEventArgs(PointF pixelPosition)
      {
         PixelPosition = pixelPosition;
      }

      public PointF PixelPosition { get; private set; }
   }

   public event EventHandler<TextureMouseMoveEventArgs> MouseMove;
   public event EventHandler ZoomLevelChanged;

   public Skeudenn.UI.Image ImageUI
   {
      set
      {
         skeudennImage = value;

         if (skeudennImage == null)
         {
            textureRect.Visible = false;

            Initialize();
         }
         else
         {
            byte[] imageData = skeudennImage.ImageData();

            image = Godot.Image.CreateFromData(skeudennImage.Size.Width, skeudennImage.Size.Height, false, Godot.Image.Format.L8, imageData);
            imageTexture = ImageTexture.CreateFromImage(image);
            textureRect.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);
            textureRect.Visible = true;
         }
      }
   }

   public float ZoomLevel
   {
      get
      {
         return skeudennImage.ZoomLevel;
      }
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
    {
      textureRect = GetNode("%TextureRect") as TextureRect;

      Initialize();
   }

   private void Initialize()
   {
      imageTexture = new ImageTexture();
      textureRect.Texture = imageTexture;
      image = new Godot.Image();

      // UNDONE Find a replacement in Godot4
      //imageTexture.Storage = ImageTexture.StorageEnum.CompressLossless;
   }

   private void _on_ZoomIn_pressed()
   {
      // HACK Remove the texture blurring when zooming in. The easiest/fastest way might be to create a child image
      skeudennImage.ZoomIn();
      textureRect.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);

      TriggerZoomChangedEvent();
   }

   private void _on_ZoomReset_pressed()
   {
      skeudennImage.ZoomReset();
      textureRect.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);

      TriggerZoomChangedEvent();
   }

   private void _on_ZoomOut_pressed()
   {
      skeudennImage.ZoomOut();
      textureRect.CustomMinimumSize = new Vector2(skeudennImage.ZoomedSize.Width, skeudennImage.ZoomedSize.Height);

      TriggerZoomChangedEvent();
   }

   private void TriggerZoomChangedEvent()
   {
      EventHandler handler = ZoomLevelChanged;

      if (handler != null)
      {
         handler(this, EventArgs.Empty);
      }
   }

   private void _on_TextureRect_gui_input(object inputEvent)
   {
      if (inputEvent is InputEventMouseMotion eventMouseMotion)
      {
         EventHandler<TextureMouseMoveEventArgs> handler = MouseMove;

         if (handler != null)
         {
            PointF pixelPosition = skeudennImage.PixelPosition(new PointF(eventMouseMotion.Position.X, eventMouseMotion.Position.Y));

            handler(this, new TextureMouseMoveEventArgs(pixelPosition));
         }
      }
   }
}
