using Godot;
using System;
using System.Diagnostics;

// TODO: Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
// TODO: Fix ExportDebug and ExportRelease configurations because they rely on $(Configuration) which is wrong in these cases
// TODO: The current setup creates two ".mono" folders. Need to find a way to have only one. It might be fixed with Godot 4 when it supports .Net 6...
// TODO: Activate/configure NCrunch.
// TODO: Configure a Continuous Integration (AppVeyor?) for the solution
public class FileMenu : MenuButton
{
   private FileDialog openImageFileDialog;

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed", this, "SubMenuClicked");
      openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");
   }

   private void _on_OpenImageFileDialog_file_selected(String path)
   {
      _on_OpenImageFileDialog_files_selected(new String[] { path });
   }

   private void _on_OpenImageFileDialog_files_selected(String[] paths)
   {
      // TODO: Load and display the image properly after opening image file
      Godot.Image image = new Godot.Image();

      byte[] imageData = new byte[500 * 500];

      for (int i = 0; i < 500 * 400; i++)
      {
         imageData[i] = 128;

      }

      image.CreateFromData(500, 500, false, Godot.Image.Format.L8, imageData);

      TextureRect textureRect = GetParent().GetParent().GetNode("ScrollContainer/TextureRect") as TextureRect;

      Godot.ImageTexture imageTex = new ImageTexture();

      imageTex.CreateFromImage(image);

      textureRect.Texture = imageTex;
   }

   public void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            // TODO: Set a proper position, size and starting directory when opening openImageFileDialog
            // TODO: Add file extension filter for images with openImageFileDialog
            openImageFileDialog.SetPosition(new Vector2(50, 200));
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
