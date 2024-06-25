using Godot;

// UNDONE Add a label to see the threshold value
// TODO The Godot UI should not depend on Skeudenn, only on Skeudenn.UI.
namespace Skeudenn.Godot
{
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

      private void OnHSliderValueChanged(double value)
      {
         binarize.Apply(value);
      }
   }
}