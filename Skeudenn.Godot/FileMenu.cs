using Godot;
using System;
using System.Diagnostics;

// HACK Configure a Continuous Integration (AppVeyor?) for the solution
// HACK Fix warning MSB3243. It doesn't happen when compiling with Skeudenn.Godot.sln
namespace Skeudenn.Godot
{
   public partial class FileMenu : MenuButton
   {
      // TODO Change the file dialog for a native one. Maybe add a user option to choose which one to use. Based on this comment "Native file dialogs are now supported for all desktop platforms via FileDialog's use_native_dialog property." from https://github.com/godotengine/godot-proposals/issues/1123#issuecomment-2111027828
      private FileDialog? openImageFileDialog;

      public partial class OpenFilesEventArgs(string[] paths) : EventArgs
      {
         public string[] Paths { get; private set; } = paths;
      }

      public event EventHandler<OpenFilesEventArgs>? OpenFiles;

      public override void _Ready()
      {
         GetPopup().Connect("id_pressed", new Callable(this, "SubMenuClicked"));
         openImageFileDialog = GetNode<FileDialog>("OpenImageFileDialog");

         openImageFileDialog.Filters = ["*.bmp, *.gif, *.jpg, *.jpeg, *.pbm, *.png, *.tif, *.tiff, *.tga, *.webp;Supported Images"];
      }

      private void OnOpenImageFileDialogFileSelected(string path)
      {
         OnOpenImageFileDialogFilesSelected([path]);
      }

      private void OnOpenImageFileDialogFilesSelected(string[] paths)
      {
         if (paths.Length > 0)
         {
            OpenFiles?.Invoke(this, new OpenFilesEventArgs(paths));
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
}