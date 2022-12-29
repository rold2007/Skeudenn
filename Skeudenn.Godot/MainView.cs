using Godot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class MainView : PanelContainer
{
   private FileMenu fileMenu;
   private Image imageNode;
   private Label pixelPosition;
   private Label zoomLevel;
   private Skeudenn.UI.MainView mainView = new Skeudenn.UI.MainView();
   private Tabs tabs;
   private List<Skeudenn.UI.Image> allImages;

   public override void _Ready()
   {
      fileMenu = GetNode<FileMenu>("%FileMenu");

      fileMenu.OpenFiles += FileMenu_OpenFiles;

      imageNode = GetNode<Image>("%Image");

      imageNode.MouseMove += ImageNode_MouseMove;
      imageNode.ZoomLevelChanged += ImageNode_ZoomLevelChanged;
      pixelPosition = GetNode("%PixelPosition") as Label;
      zoomLevel = GetNode("%ZoomLevel") as Label;

      tabs = GetNode<Tabs>("%Tabs");

      allImages = new List<Skeudenn.UI.Image>();
   }

   private void FileMenu_OpenFiles(object sender, FileMenu.OpenFilesEventArgs e)
   {
      foreach (string path in e.Paths)
      {
         Skeudenn.UI.Image skeudennImage = mainView.OpenFile(path);
         // HACK The UI shouldn't be responsible to decide the tabs name
         string filename = System.IO.Path.GetFileName(path);

         allImages.Add(skeudennImage);
         tabs.AddTab(filename);
         tabs.CurrentTab = tabs.GetTabCount() - 1;

         // UNDONE No need to assign this in the loop. Only assign the first or last image.
         imageNode.ImageUI = skeudennImage;
      }

      PrintZoomLevel();
   }

   private void ImageNode_MouseMove(object sender, Image.TextureMouseMoveEventArgs e)
   {
      pixelPosition.Text = e.PixelPosition.ToString();
   }

   private void ImageNode_ZoomLevelChanged(object sender, System.EventArgs e)
   {
      PrintZoomLevel();
   }

   private void PrintZoomLevel()
   {
      zoomLevel.Text = "Zoom Level: " + imageNode.ZoomLevel.ToString() + "%";
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

   private void _on_MainMenu_tree_exiting()
   {
      fileMenu.OpenFiles -= FileMenu_OpenFiles;
      imageNode.MouseMove -= ImageNode_MouseMove;
   }
}

