using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class DisplayController
{
    public HBox Box;
    public List<FileSystemInfo> ImageFiles;
    public int ImageIndex; 

    private VBox SideBar;
    //private Image BigImage;
    private Bin imageBin;

    private PhotoPresenter photoPresenter;

    public DisplayController()
    {
        Box = new HBox(false, 0);
        SideBar = new VBox(false, 0);
        VSeparator sideBarSeparator = new VSeparator();

        imageBin = new Frame();
        //BigImage = new Image();
        ImageFiles = new List<FileSystemInfo>();

        photoPresenter = new PhotoPresenter(new WorkingFolderModel());
        photoPresenter.InsertView(imageBin);

        Box.PackStart(SideBar, false, false, 3);
        Box.PackStart(sideBarSeparator, false, false, 5);
        Box.PackStart(imageBin, true, true, 2);
        //Box.PackStart(BigImage, true, true, 2);

        Label sideBarLabel = new Label("Side Bar");
        SideBar.PackStart(sideBarLabel, true, true, 3);
    }

    public void ChangeDirectory(DirectoryInfo newDirectory)
    {
        ImageFiles = Finder.FindImages(newDirectory).ToList();
        ImageIndex = 0;
    }

    public void NextImage(int step)
    {
        if (ImageFiles.Any()) {
            ImageIndex = (ImageIndex + ImageFiles.Count + step) % ImageFiles.Count;
            photoPresenter.Display(ImageFiles[ImageIndex].FullName);
            //ShowCurrentImage();
        }
    }

    private void ShowCurrentImage()
    {
        Gdk.Pixbuf currentImage = GetCurrentImage();
        DisplayImage(currentImage);
    }
 
    private Gdk.Pixbuf GetCurrentImage()
    {
        //if (ImageFiles.count <= index) {
        // TODO check item exists
        return new Gdk.Pixbuf(ImageFiles[ImageIndex].FullName);
    }

    private void DisplayImage(Gdk.Pixbuf pixBuf)
    {
        // Mogut a photoview
    }
}
