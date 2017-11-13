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

    private DirectoryInfo ActiveDirectory;
    private VBox SideBar;
    private Image BigImage;

    public DisplayController()
    {
        Box = new HBox(false, 0);
        SideBar = new VBox(false, 0);
        VSeparator sideBarSeparator = new VSeparator();
        BigImage = new Image();
        ImageFiles = new List<FileSystemInfo>();

        Box.PackStart(SideBar, false, false, 3);
        Box.PackStart(sideBarSeparator, false, false, 5);
        Box.PackStart(BigImage, true, true, 2);

        Label sideBarLabel = new Label("Side Bar");
        SideBar.PackStart(sideBarLabel, true, true, 3);
    }

    public void ChangeDirectory(DirectoryInfo newDirectory)
    {
        ActiveDirectory = newDirectory;
        ImageFiles = Finder.FindImages(newDirectory).ToList();
        ImageIndex = 0;
    }

    public void NextImage(int step)
    {
        if (ImageFiles.Any()) {
            ImageIndex = (ImageIndex + ImageFiles.Count + step) % ImageFiles.Count;
            ShowCurrentImage();
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
        double ratio = Math.Min(
            BigImage.Allocation.Width / Convert.ToDouble(pixBuf.Width),
            BigImage.Allocation.Height / Convert.ToDouble(pixBuf.Height)
        );
        
        if (ratio < 1)
        {
            pixBuf = pixBuf.ScaleSimple(
                Convert.ToInt32(Math.Round(pixBuf.Width * ratio)),
                Convert.ToInt32(Math.Round(pixBuf.Height * ratio)),
                Gdk.InterpType.Bilinear);
        }
        BigImage.Pixbuf = pixBuf;
    }
}
