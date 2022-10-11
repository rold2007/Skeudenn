using Godot;
using System;
using System.Diagnostics;

// TODO Make the image MDI-like with tabs, etc.
// TODO Try the new "New flow containers" (HFlowContainer and VFlowContainer) now available in Godot 3.5: https://godotengine.org/article/godot-3-5-cant-stop-wont-stop
// HACK Configure a Continuous Integration (AppVeyor?) for the solution
// HACK Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
// HACK Fix ExportDebug and ExportRelease configurations because they rely on $(Configuration) which is wrong in these cases
// HACK The current setup creates two ".mono" folders. Need to find a way to have only one. It might be fixed with Godot 4 when it supports .Net 6...
public class FileMenu : MenuButton
{
   private FileDialog openImageFileDialog;

   private Label pixelPosition;
   private Skeudenn.UI.MainMenu mainMenu = new Skeudenn.UI.MainMenu();

   // UNDONE imageNode should not be in FileMenu...
   private Image imageNode;

   public override void _Ready()
   {
      OS.WindowMaximized = true;

      GetPopup().Connect("id_pressed", this, "SubMenuClicked");
      openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");
      imageNode = GetNode<Image>("%Image");
      pixelPosition = GetNode("%PixelPosition") as Label;

      imageNode.MouseMove += ImageNode_MakeMeDoWork;
   }

   private void _on_OpenImageFileDialog_file_selected(String path)
   {
      _on_OpenImageFileDialog_files_selected(new String[] { path });
   }

   private void _on_OpenImageFileDialog_files_selected(String[] paths)
   {
      // TODO Support more than one path
      Skeudenn.UI.Image skeudennImage = mainMenu.OpenFile(paths[0]);

      imageNode.ImageUI = skeudennImage;
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

   private void ImageNode_MakeMeDoWork(object sender, Image.TextureMouseMoveEventArgs e)
   {
      pixelPosition.Text = e.PixelPosition.ToString();
   }
}
