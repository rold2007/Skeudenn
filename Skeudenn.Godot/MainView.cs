using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MainView : PanelContainer
{
   private FileMenu fileMenu;
   private Image imageNode;
   private Label pixelPosition;
   private Label zoomLevel;
   private Skeudenn.UI.MainView mainView = new Skeudenn.UI.MainView();
   private Tabs tabs;
   private List<Skeudenn.UI.Image> allImages;
   private AcceptDialog acceptDialog;

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

      acceptDialog = GetNode<AcceptDialog>("%AcceptDialog");

      allImages = new List<Skeudenn.UI.Image>();
   }

   private void FileMenu_OpenFiles(object sender, FileMenu.OpenFilesEventArgs e)
   {
      bool error;

      // HACK I don't like out parameters. Replace this with a more standard way of dealing with errors in the UI
      List<Skeudenn.UI.Image> images = mainView.OpenFiles(e.Paths, out error);

      foreach (Skeudenn.UI.Image image in images)
      {
         allImages.Add(image);
         tabs.AddTab(image.Name);
         tabs.CurrentTab = tabs.GetTabCount() - 1;
      }

      if (error)
      {
         // HACK Set a proper position, size and starting directory when opening acceptDialog
         // HACK Most properties could be initialized only once inside _Ready()
         acceptDialog.SetPosition(new Vector2(50, 100));
         acceptDialog.RectMinSize = new Vector2(0, 0);
         acceptDialog.SetSize(new Vector2(640, 480));
         acceptDialog.WindowTitle = "Image file load error";
         acceptDialog.DialogText = "One or more file could not be loaded.";
         acceptDialog.ShowModal(true);
      }

      if (allImages.Count() > 0)
      {
         imageNode.ImageUI = allImages.Last();

         PrintZoomLevel();
      }
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

