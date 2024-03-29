using Godot;
using System;
using System.Diagnostics;

// HACK Configure a Continuous Integration (AppVeyor?) for the solution
// HACK Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
public partial class FileMenu : MenuButton
{
   private FileDialog? openImageFileDialog;

   public partial class OpenFilesEventArgs : EventArgs
   {
      public OpenFilesEventArgs(string[] paths)
      {
         Paths = paths;
      }

      public string[] Paths { get; private set; }
   }

   public event EventHandler<OpenFilesEventArgs>? OpenFiles;

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed",new Callable(this,"SubMenuClicked"));
      openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");

      openImageFileDialog.Filters = new string[] { "*.bmp, *.gif, *.jpg, *.jpeg, *.pbm, *.png, *.tif, *.tiff, *.tga, *.webp;Supported Images" };
   }

   private void _on_OpenImageFileDialog_file_selected(string path)
   {
      _on_OpenImageFileDialog_files_selected(new string[] { path });
   }

   private void _on_OpenImageFileDialog_files_selected(string[] paths)
   {
      if (paths.Length > 0)
      {
         EventHandler<OpenFilesEventArgs>? handler = OpenFiles;

         if (handler != null)
         {
            handler(this, new OpenFilesEventArgs(paths));
         }
      }
   }

   // HACK Use the new MenuBar instead of the custom MenuButton to implement the menu. Not sure how it works. Wait for a tutorial...
   private void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            // HACK Set a proper position, size and starting directory when opening openImageFileDialog
            openImageFileDialog!.Position = new Vector2I(50, 100);
            openImageFileDialog!.MinSize = new Vector2I(0, 0);
            openImageFileDialog!.Size = new Vector2I(640, 480);

            openImageFileDialog!.Show();
            openImageFileDialog!.Invalidate();
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
