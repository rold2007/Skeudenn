using Godot;
using System;
using System.Collections.Generic;

namespace Skeudenn.Godot
{
   public partial class MainView : PanelContainer
   {
	  private FileMenu? fileMenu;
	  private Image? imageNode;
	  private Label? pixelPosition;
	  private Label? zoomLevel;
	  private TabBar? tabs;
	  private readonly List<Skeudenn.UI.Image> allImages = [];
	  private AcceptDialog? acceptDialog;

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
	  }

	  private void FileMenu_OpenFiles(object? sender, FileMenu.OpenFilesEventArgs? e)
	  {

		 // HACK I don't like out parameters. Replace this with a more standard way of dealing with errors in the UI
		 List<Skeudenn.UI.Image> images = Skeudenn.UI.MainView.OpenFiles(e!.Paths, out bool error);

		 foreach (Skeudenn.UI.Image image in images)
		 {
			allImages.Add(image);
			tabs!.AddTab(image.Name);
			tabs!.CurrentTab = tabs.TabCount - 1;
		 }

		 if (error)
		 {
			// HACK Set a proper position, size and starting directory when opening acceptDialog
			// HACK Most properties could be initialized only once inside _Ready()
			acceptDialog!.Position = new Vector2I(50, 100);
			acceptDialog!.MinSize = new Vector2I(0, 0);
			acceptDialog!.Size = new Vector2I(640, 480);
			acceptDialog!.Title = "Image file load error";
			acceptDialog!.DialogText = "One or more file could not be loaded.";

			acceptDialog!.Show();
		 }

		 if (allImages.Count > 0)
		 {
			PrintZoomLevel();
		 }
	  }

	  private void ImageNode_MouseMove(object? sender, Image.TextureMouseMoveEventArgs? e)
	  {
		 pixelPosition!.Text = e!.PixelPosition.ToString();
	  }

	  // TODO Improve the zoom UI to look more like Paint.Net
	  private void ImageNode_ZoomLevelChanged(object? sender, EventArgs? e)
	  {
		 PrintZoomLevel();
	  }

	  private void PrintZoomLevel()
	  {
		 if (imageNode!.Valid)
		 {
			zoomLevel!.Text = "Zoom Level: " + imageNode!.ZoomLevel.ToString() + "%";
		 }
		 else
		 {
			zoomLevel!.Text = string.Empty;
		 }
	  }

	  private void OnTabsResized()
	  {
		 // Replace with function body.
		 if (tabs != null && imageNode != null)
		 {
			// HACK Need to deal better with CustomMinimumSize. It is forced at 300,300 but it doesn't look nice when the UI is small.
			imageNode.Size = tabs.Size;
		 }
	  }

	  private void OnTabsTabChanged(int tab)
	  {
		 ChangeTab(tab);
	  }

	  private void OnTabsTabClose(int tab)
	  {
		 tabs!.RemoveTab(tab);
		 allImages.RemoveAt(tab);

		 if (tabs!.TabCount > 0)
		 {
			ChangeTab(tabs!.CurrentTab);
		 }
		 else
		 {
			imageNode!.Reset();
			pixelPosition!.Text = "(0,0)";
			PrintZoomLevel();
		 }
	  }

	  private void ChangeTab(int tab)
	  {
		 imageNode!.ImageUI = allImages[tab];
		 imageNode!.Visible = true;
	  }

	  private void OnMainMenuTreeExiting()
	  {
		 fileMenu!.OpenFiles -= FileMenu_OpenFiles;
		 imageNode!.MouseMove -= ImageNode_MouseMove;
	  }
   }
}
