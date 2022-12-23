using Godot;
using System;
using System.Diagnostics;

// HACK Configure a Continuous Integration (AppVeyor?) for the solution
// HACK Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
// HACK Fix ExportDebug and ExportRelease configurations because they rely on $(Configuration) which is wrong in these cases
// HACK The current setup creates two ".mono" folders. Need to find a way to have only one. It might be fixed with Godot 4 when it supports .Net 6...
public class FileMenu : MenuButton
{
   private FileDialog openImageFileDialog;

   public class OpenFilesEventArgs : EventArgs
   {
      public OpenFilesEventArgs(String[] paths)
      {
         Paths = paths;
      }

      public String[] Paths { get; private set; }
   }

   public event EventHandler<OpenFilesEventArgs> OpenFiles;

   public override void _Ready()
   {
      OS.WindowMaximized = true;

      GetPopup().Connect("id_pressed", this, "SubMenuClicked");
      openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");
   }

   private void _on_OpenImageFileDialog_file_selected(String path)
   {
      _on_OpenImageFileDialog_files_selected(new String[] { path });
   }

   private void _on_OpenImageFileDialog_files_selected(String[] paths)
   {
      EventHandler<OpenFilesEventArgs> handler = OpenFiles;

      if (handler != null)
      {
         handler(this, new OpenFilesEventArgs(paths));
      }
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
}
