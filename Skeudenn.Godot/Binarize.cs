using Godot;

// UNDONE Add a label to see the threshold value
// UNDONE Change the default value of the initial threshold
public partial class Binarize : VBoxContainer
{
   private HSlider? slider;
   private Skeudenn.UI.Binarize binarize = new Skeudenn.UI.Binarize();

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      slider = GetNode<HSlider>("%HSlider");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   private void _on_h_slider_value_changed(double value)
   {
      binarize.Apply(value);
   }

   private void _on_visibility_changed()
   {
      if (this.Visible)
      {
         binarize.Apply(slider!.Value);
      }
      else
      {
         binarize.Remove();
      }
   }
}
