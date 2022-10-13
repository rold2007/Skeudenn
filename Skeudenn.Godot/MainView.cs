using Godot;
using System;

public class MainView : PanelContainer
{
   private FileMenu fileMenu;
   private Image imageNode;
   private Label pixelPosition;
   private Skeudenn.UI.MainView mainView = new Skeudenn.UI.MainView();

   public override void _Ready()
   {
      fileMenu = GetNode<FileMenu>("%FileMenu");

      // TODO Remove the event handler on Tree_Exiting signal. Do this for all events in all classes.
      fileMenu.OpenFiles += FileMenu_OpenFiles;

      imageNode = GetNode<Image>("%Image");

      // TODO Remove the event handler on Tree_Exiting signal. Do this for all events in all classes.
      imageNode.MouseMove += ImageNode_MouseMove;
      pixelPosition = GetNode("%PixelPosition") as Label;
   }

   private void FileMenu_OpenFiles(object sender, FileMenu.OpenFilesEventArgs e)
   {
      // TODO Support more than one path
      Skeudenn.UI.Image skeudennImage = mainView.OpenFile(e.Paths[0]);

      imageNode.ImageUI = skeudennImage;
   }

   private void ImageNode_MouseMove(object sender, Image.TextureMouseMoveEventArgs e)
   {
      pixelPosition.Text = e.PixelPosition.ToString();
   }
}
