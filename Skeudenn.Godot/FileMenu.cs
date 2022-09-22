using Godot;
using Skeudenn.Controller;
using System;
using System.Diagnostics;

// UNDONE Should be using Skeudenn.UI and not Skeudenn.Controller
// TODO Make the image MDI-like with tabs, etc.
// TODO Try the new "New flow containers" (HFlowContainer and VFlowContainer) now available in Godot 3.5: https://godotengine.org/article/godot-3-5-cant-stop-wont-stop
// HACK Configure a Continuous Integration (AppVeyor?) for the solution
// HACK Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
// HACK Fix ExportDebug and ExportRelease configurations because they rely on $(Configuration) which is wrong in these cases
// HACK The current setup creates two ".mono" folders. Need to find a way to have only one. It might be fixed with Godot 4 when it supports .Net 6...
public class FileMenu : MenuButton
{
   private FileDialog openImageFileDialog;
   private TextureRect textureRect;
   private ImageTexture imageTexture;
   private Godot.Image image;
   private Label pixelPosition;
   private FileOpen fileOpen = new FileOpen();

   public override void _Ready()
   {
      OS.WindowMaximized = true;

      GetPopup().Connect("id_pressed", this, "SubMenuClicked");
      openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");

      textureRect = GetNode("%TextureRect") as TextureRect;
      pixelPosition = GetNode("%PixelPosition") as Label;
      imageTexture = new ImageTexture();
      textureRect.Texture = imageTexture;
      image = new Godot.Image();

      imageTexture.Storage = ImageTexture.StorageEnum.CompressLossless;
   }

   private void _on_OpenImageFileDialog_file_selected(String path)
   {
      _on_OpenImageFileDialog_files_selected(new String[] { path });
   }

   private void _on_OpenImageFileDialog_files_selected(String[] paths)
   {
      // UNDONE Support more than one path
      Skeudenn.Controller.Image skeudennImage = fileOpen.OpenFile(paths[0]);
      byte[] imageData = skeudennImage.ImageData();

      image.CreateFromData(skeudennImage.Size.Width, skeudennImage.Size.Height, false, Godot.Image.Format.L8, imageData);
      imageTexture.CreateFromImage(image);
      textureRect.RectSize = new Vector2(skeudennImage.Size.Width, skeudennImage.Size.Height);
   }

   private void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            // TODO Set a proper position, size and starting directory when opening openImageFileDialog
            // TODO Add file extension filter for images with openImageFileDialog
            openImageFileDialog.SetPosition(new Vector2(50, 100));
            openImageFileDialog.RectMinSize = new Vector2(0, 0);
            openImageFileDialog.SetSize(new Vector2(640, 480));
            openImageFileDialog.ShowModal(true);
            openImageFileDialog.Invalidate();
            break;

         case 1:
            GetTree().Quit();
            break;

         default:
            Debug.Fail("Unknown menu id.");
            break;
      }
   }

   // TODO The code to manage the zoom should not be in filemenu.cs... And it should call the controller as soon as possible
   private void _on_ZoomIn_Button_pressed()
   {
      // TODO Make the scroll bars appear when the image is too big to fit the ScrollContainer
      // TODO Remove the texture blurring when zooming in. The easiest/fastest way might be to create a child image
      // TODO Apply zoom in/out steps like in Paint.Net
      // TODO Limit the zoom in/out min/max ratios
      textureRect.RectSize *= 1.5f;
   }

   private void _on_ZoomReset_Button_pressed()
   {
      Vector2 imageSize = image.GetSize();

      // TODO Fix the image which disappear when chaning the window size. USing the reset zoom works a workaround so it is probably related to the RectSize
      textureRect.RectSize = imageSize;
   }

   private void _on_ZoomOut_Button_pressed()
   {
      textureRect.RectSize /= 1.5f;
   }

   private void _on_TextureRect_gui_input(object inputEvent)
   {
      if (inputEvent is InputEventMouseMotion eventMouseMotion)
      {
         // TODO Take the zoom into account to remap to the image pixel position
         pixelPosition.Text = eventMouseMotion.Position.ToString();
      }
   }
}


