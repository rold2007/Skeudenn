using Skeudenn.UI;

// TODO The Godot UI should not depend on Skeudenn, only on Skeudenn.UI.
namespace Skeudenn.Godot
{
   public static class Globals
   {
      private static ImageProcessors imageProcessors = new();
      private static ActiveImage activeImage = new();

      public static ImageProcessors ImageProcessors { get => imageProcessors; set => imageProcessors = value; }
      public static ActiveImage ActiveImage { get => activeImage; set => activeImage = value; }
   }
}