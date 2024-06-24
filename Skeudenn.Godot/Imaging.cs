using Godot;
using System;
using System.Diagnostics;

public partial class Imaging : MenuButton
{
   private Plugin? binarizePluginWindow;

   public override void _Ready()
   {
      GetPopup().Connect("id_pressed", new Callable(this, "SubMenuClicked"));
   }

   public void SubMenuClicked(int id)
   {
      switch (id)
      {
         case 0:
            InitializeBinarizePluginWindow();
            binarizePluginWindow!.Popup();
            break;

         default:
            Debug.Fail("Unknown menu id.");
            break;
      }
   }

   private void InitializeBinarizePluginWindow()
   {
      if (binarizePluginWindow == null)
      {
         PackedScene pluginScene = GD.Load<PackedScene>("res://Plugin.tscn");
         PackedScene binarizeScene = GD.Load<PackedScene>("res://Binarize.tscn");

         binarizePluginWindow = pluginScene.Instantiate<Plugin>();

         binarizePluginWindow.Visible = false;

         Binarize? binarize = binarizeScene.Instantiate<Binarize>();

         binarize.Update(Globals.ImageProcessors);
         binarizePluginWindow.Size = new Vector2I(Convert.ToInt32(binarize.GetMinimumSize().X), Convert.ToInt32(binarize.GetMinimumSize().Y));
         binarizePluginWindow.MaxSize = binarizePluginWindow.Size;

         binarizePluginWindow.AboutToPopup += binarize.Apply;
         binarizePluginWindow.CloseRequested += binarize.Remove;

         // HACK Maybe this AddChild should be done on the MainView instead of this MenuButton...
         AddChild(binarizePluginWindow);

         binarizePluginWindow!.AddPluginWindow(binarize!);
      }
   }
}
