using Godot;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Skeudenn.Controller;
using System;
using System.Diagnostics;
using System.IO;

// TODO Add basic options to the image: Zoom, pixel position in status bar, etc.
// TODO Make the image MDI-like with tabs, etc.
// TODO Try the new "New flow containers" (HFlowContainer and VFlowContainer) now avilable in Godot 3.5: https://godotengine.org/article/godot-3-5-cant-stop-wont-stop
// TODO Configure a Continuous Integration (AppVeyor?) for the solution
// TODO Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
// TODO Fix ExportDebug and ExportRelease configurations because they rely on $(Configuration) which is wrong in these cases
// TODO The current setup creates two ".mono" folders. Need to find a way to have only one. It might be fixed with Godot 4 when it supports .Net 6...
public class FileMenu : MenuButton
{
   private FileDialog openImageFileDialog;
   private TextureRect textureRect;
   private Godot.ImageTexture imageTexture;
   private Godot.Image image;
   private FileOpen fileOpen = new FileOpen();

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed", this, "SubMenuClicked");
      openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");

      // TODO Use the new feature "Access nodes by unique names" now available in Godot 3.5: https://godotengine.org/article/godot-3-5-cant-stop-wont-stop
      textureRect = GetParent().GetParent().GetNode("ScrollContainer/TextureRect") as TextureRect;
      imageTexture = new ImageTexture();
      textureRect.Texture = imageTexture;
      image = new Godot.Image();
   }

   private void _on_OpenImageFileDialog_file_selected(String path)
   {
      _on_OpenImageFileDialog_files_selected(new String[] { path });
   }

   private void _on_OpenImageFileDialog_files_selected(String[] paths)
   {
      // TODO Support more than one path
      Skeudenn.Controller.Image skeudennImage = fileOpen.OpenFile(paths[0]); ;

      // TODO Move the logic to extract the Span/imageData to Skeudenn.Controller.Image
      Image<L8> imageClone = skeudennImage.ImageClone;
      byte[] imageData = new byte[imageClone.Width * imageClone.Height];
      Span<byte> theSpan = new Span<byte>(imageData);

      imageClone.CopyPixelDataTo(theSpan);

      image.CreateFromData(imageClone.Width, imageClone.Height, false, Godot.Image.Format.L8, imageData);
      imageTexture.CreateFromImage(image);
   }

   public void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            // TODO Set a proper position, size and starting directory when opening openImageFileDialog
            // TODO Add file extension filter for images with openImageFileDialog
            openImageFileDialog.SetPosition(new Vector2(50, 200));
            openImageFileDialog.RectMinSize = new Vector2(0, 0);
            openImageFileDialog.ShowModal(true);
            break;

         case 1:
            GetTree().Quit();
            break;

         default:
            Debug.Fail("Unknown menu id.");
            break;
      }
   }
}
