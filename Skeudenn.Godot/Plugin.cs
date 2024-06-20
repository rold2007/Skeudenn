using Godot;

// TODO Allow to change the window title depending on the plugin shown
public partial class Plugin : Window
{
   // TODO Restore [rendering] to default value in project.godot
   private VBoxContainer? vBoxContainer;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
	  vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   // TODO The plugin window cannot be moved outside of the UI. See how to make it possible to move it outside like a normal GUI
   public void AddPluginWindow(Node node)
   {
	  vBoxContainer!.AddChild(node);
   }

   private void _on_close_requested()
   {
	  Hide();
   }
}
