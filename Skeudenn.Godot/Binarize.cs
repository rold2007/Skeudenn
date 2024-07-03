using Godot;

// TODO The Godot UI should not depend on Skeudenn, only on Skeudenn.UI.
namespace Skeudenn.Godot
{
   public partial class Binarize : VBoxContainer
   {
      private Label? label;
      private HSlider? slider;
      private Skeudenn.UI.Binarize binarize = new();

      // Called when the node enters the scene tree for the first time.
      public override void _Ready()
      {
         label = GetNode<Label>("%Label");
         slider = GetNode<HSlider>("%HSlider");

         UpdateLabelValue();
      }

      // Called every frame. 'delta' is the elapsed time since the previous frame.
      public override void _Process(double delta)
      {
      }

      public void Apply()
      {
         Apply(slider!.Value);
      }

      public void Apply(double value)
      {
         binarize.Apply(value);
      }

      public void Remove()
      {
         binarize.Remove();
      }

      public void Update(ImageProcessors imageProcessors)
      {
         binarize = binarize.Update(imageProcessors);
      }

      private void UpdateLabelValue()
      {
         label!.Text = System.Convert.ToString(slider!.Value);
      }

      private void OnHSliderValueChanged(double value)
      {
         UpdateLabelValue();
         Apply(value);
      }
   }
}
