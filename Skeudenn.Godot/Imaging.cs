using Godot;
using System.Diagnostics;

public partial class Imaging : MenuButton
{
   private Binarize? binarize;

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed", new Callable(this, "SubMenuClicked"));
      binarize = new Binarize();
   }

   // UNDONE Make the close button work on the Binarize scene
   // UNDONE Split the Window from VBoxContainer to create a plugin Window which can display any sub-UI
   // UNDONE Make the Binarize window appear only when we click the menus
   public void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            binarize!.Show();
            break;

         default:
            Debug.Fail("Unknown menu id.");
            break;
      }
   }
}
