using Godot;
using System.Diagnostics;
using Skeudenn.UI;

public class HelpMenu : MenuButton
{
   private AcceptDialog aboutBoxDialog;

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed", this, "SubMenuClicked");
      aboutBoxDialog = GetNode<AcceptDialog>("AboutBoxDialog");
   }

   public void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            Skeudenn.UI.MainView mainView = new Skeudenn.UI.MainView();

            aboutBoxDialog.WindowTitle = "About";
            aboutBoxDialog.DialogText = mainView.AboutText();
            aboutBoxDialog.DialogText += System.Environment.NewLine;
            aboutBoxDialog.DialogText += System.Environment.NewLine;
            aboutBoxDialog.DialogText += "Made with Godot";
            aboutBoxDialog.DialogText += System.Environment.NewLine;
            aboutBoxDialog.DialogText += "https://godotengine.org/license";

            // HACK Apply a centered position to the about box
            aboutBoxDialog.SetPosition(new Vector2(50, 200));

            aboutBoxDialog.ShowModal(true);
            break;

         default:
            Debug.Fail("Unknown menu id.");
            break;
      }
   }
}

