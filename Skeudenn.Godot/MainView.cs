using Godot;
using System.Collections.Generic;

public class MainView : PanelContainer
{
   private FileMenu fileMenu;
   private Image imageNode;
   private Label pixelPosition;
   private Skeudenn.UI.MainView mainView = new Skeudenn.UI.MainView();
   private Tabs tabs;
   private List<Skeudenn.UI.Image> allImages;

   public override void _Ready()
   {
      fileMenu = GetNode<FileMenu>("%FileMenu");

      // TODO Remove the event handler on Tree_Exiting signal. Do this for all events in all classes.
      fileMenu.OpenFiles += FileMenu_OpenFiles;

      imageNode = GetNode<Image>("%Image");

      // TODO Remove the event handler on Tree_Exiting signal. Do this for all events in all classes.
      imageNode.MouseMove += ImageNode_MouseMove;
      pixelPosition = GetNode("%PixelPosition") as Label;

      tabs = GetNode<Tabs>("%Tabs");

      allImages = new List<Skeudenn.UI.Image>();
   }

   private void FileMenu_OpenFiles(object sender, FileMenu.OpenFilesEventArgs e)
   {
      // TODO Support more than one path
      Skeudenn.UI.Image skeudennImage = mainView.OpenFile(e.Paths[0]);
      // TODO Maybe the UI shouldn't be responsible to decide the tabs name
      string filename = System.IO.Path.GetFileName(e.Paths[0]);

      allImages.Add(skeudennImage);
      tabs.AddTab(filename);
      tabs.CurrentTab = tabs.GetTabCount() - 1;

      imageNode.ImageUI = skeudennImage;
   }

   private void ImageNode_MouseMove(object sender, Image.TextureMouseMoveEventArgs e)
   {
      pixelPosition.Text = e.PixelPosition.ToString();
   }

   private void _on_Tabs_resized()
   {
      // Replace with function body.
      if (tabs != null && imageNode != null)
      {
         // HACK Need to deal better with RectMinSize. It is forced at 300,300 but it doesn't look nice when the UI is small.
         imageNode.RectSize = tabs.RectSize;
      }
   }

   private void _on_Tabs_tab_changed(int tab)
   {
      ChangeTab(tab);
   }

   private void _on_Tabs_tab_close(int tab)
   {
      tabs.RemoveTab(tab);
      allImages.RemoveAt(tab);

      ChangeTab(tabs.CurrentTab);
   }

   private void ChangeTab(int tab)
   {
      if (tab >= 0)
      {
         imageNode.ImageUI = allImages[tab];
      }
      else
      {
         imageNode.ImageUI = null;
      }
   }
}
