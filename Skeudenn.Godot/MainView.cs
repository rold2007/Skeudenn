using Godot;
using System.Collections.Generic;
using System.Linq;

// UNDONE Dependencies, like SixLabors.ImageSharp.dll, are not copied to .godot\mono\temp\bin\Debug anymore. Why ?
public partial class MainView : PanelContainer
{
   private FileMenu fileMenu;
   private Image imageNode;
   private Label pixelPosition;
   private Label zoomLevel;
   private Skeudenn.UI.MainView mainView = new Skeudenn.UI.MainView();
   private TabBar tabs;
   private List<Skeudenn.UI.Image> allImages;
   private AcceptDialog acceptDialog;

   public override void _Ready()
   {
      fileMenu = GetNode<FileMenu>("%FileMenu");

      fileMenu.OpenFiles += FileMenu_OpenFiles;

      imageNode = GetNode<Image>("%Image");

      imageNode.MouseMove += ImageNode_MouseMove;
      imageNode.ZoomLevelChanged += ImageNode_ZoomLevelChanged;
      pixelPosition = GetNode<Label>("%PixelPosition");
      zoomLevel = GetNode<Label>("%ZoomLevel");

      tabs = GetNode<TabBar>("%TabBar");

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
         tabs.CurrentTab = tabs.TabCount - 1;
      }

      if (error)
      {
         // HACK Set a proper position, size and starting directory when opening acceptDialog
         // HACK Most properties could be initialized only once inside _Ready()
         acceptDialog.Position = new Vector2I(50, 100);
         acceptDialog.MinSize = new Vector2I(0, 0);
         acceptDialog.Size = new Vector2I(640, 480);
         acceptDialog.Title = "Image file load error";
         acceptDialog.DialogText = "One or more file could not be loaded.";

         acceptDialog.Show();
      }

      if (allImages.Count() > 0)
      {
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
         // HACK Need to deal better with CustomMinimumSize. It is forced at 300,300 but it doesn't look nice when the UI is small.
         imageNode.Size = tabs.Size;
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

