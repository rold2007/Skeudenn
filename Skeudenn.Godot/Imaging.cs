using Godot;
using System.Diagnostics;

public partial class Imaging : MenuButton
{
   private Binarize? binarize;

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed", new Callable(this, "SubMenuClicked"));

      // TODO Move this to a method which will be called only the first time we try to show Binarize
      PackedScene scene = GD.Load<PackedScene>("res://Binarize.tscn");

      binarize = scene.Instantiate<Binarize>();

      binarize.Visible = false;

      AddChild(binarize);
   }

   // UNDONE Split the Window from VBoxContainer to create a plugin Window which can display any sub-UI
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
