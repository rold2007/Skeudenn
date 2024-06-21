using Godot;
using Skeudenn;

// UNDONE Add a label to see the threshold value
public partial class Binarize : VBoxContainer
{
   private HSlider? slider;
   private Skeudenn.UI.Binarize binarize = new();

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      slider = GetNode<HSlider>("%HSlider");
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }

   public void Apply()
   {
      binarize.Apply(slider!.Value);
   }

   public void Remove()
   {
      binarize.Remove();
   }

   public void Update(ImageProcessors imageProcessors)
   {
      binarize = binarize.Update(imageProcessors);
   }

   private void _on_h_slider_value_changed(double value)
   {
      binarize.Apply(value);
   }
}
