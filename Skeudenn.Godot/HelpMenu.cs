using Godot;
using System.Diagnostics;

namespace Skeudenn.Godot
{
   public partial class HelpMenu : MenuButton
   {
      private AcceptDialog? aboutBoxDialog;

      public override void _Ready()
      {
         GetPopup().Connect("id_pressed", new Callable(this, "SubMenuClicked"));
         aboutBoxDialog = GetNode<AcceptDialog>("AboutBoxDialog");
      }

      public void SubMenuClicked(int id)
      {
         switch (id)
         {
            case 0:
               aboutBoxDialog!.Title = "About";
               aboutBoxDialog!.DialogText = Skeudenn.UI.MainView.AboutText();
               aboutBoxDialog!.DialogText += System.Environment.NewLine;
               aboutBoxDialog!.DialogText += System.Environment.NewLine;
               aboutBoxDialog!.DialogText += "Made with Godot";
               aboutBoxDialog!.DialogText += System.Environment.NewLine;
               aboutBoxDialog!.DialogText += "https://godotengine.org/license";

               // HACK Apply a centered position to the about box
               aboutBoxDialog!.Position = new Vector2I(50, 200);

               aboutBoxDialog!.Show();
               break;

            default:
               Debug.Fail("Unknown menu id.");
               break;
         }
      }
   }
}