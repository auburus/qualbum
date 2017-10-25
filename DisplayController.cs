using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class DisplayController
{
    public HBox Box;
    public DirectoryInfo ActiveDirectory;
    private VBox sideBar;
    private Image bigImage;

    public DisplayController()
    {
        Box = new HBox(false, 0);
        sideBar = new VBox(false, 0);
        VSeparator sideBarSeparator = new VSeparator();
        bigImage = new Image();

        Box.PackStart(sideBar, false, false, 3);
        Box.PackStart(sideBarSeparator, false, false, 5);
        Box.PackStart(bigImage, true, true, 2);

        Label sideBarLabel = new Label("Side Bar");
        sideBar.PackStart(sideBarLabel, true, true, 3);
    }

    public void ChangeDirectory(DirectoryInfo newDirectory)
    {
        Gdk.Pixbuf firstPicture;

        ActiveDirectory = newDirectory;
        firstPicture = GetFirstImage(newDirectory);
        DisplayImage(firstPicture);
    }

    private void DisplayImage(Gdk.Pixbuf pixBuf)
    {
        bigImage.Pixbuf = pixBuf;
    }

    Gdk.Pixbuf GetFirstImage(DirectoryInfo directory)
    {
        IEnumerable<FileSystemInfo> images = Finder.FindImages(directory);
        if (!images.Any() )
        {
            throw new DirectoryWithoutImagesException(
                "Directory " + directory.FullName + " doesn't contain any images");
        }

        return new Gdk.Pixbuf(images.First().FullName);
    }
}
