using System;
using Gtk;

class PhotoView
{
    private HBox box;
    private VBox sideBar;
    private Bin imageBin;
    private Image photo;

    public PhotoView()
    {
        Initialize();
    }

    public Widget AsWidget { get { return box; } }

    public void Display(string path)
    {
        // Force the GC to run to claim the memory that the different
        // new pixbuf keep using, that for some reason doesn't get
        // reclaimed automatically.
        // Reference: https://github.com/gtkd-developers/GtkD/issues/127
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(path);
        Display(pixbuf);
    }

    private void Display(Gdk.Pixbuf pixbuf)
    {
        double ratio = Math.Min(
            photo.Allocation.Width / Convert.ToDouble(pixbuf.Width),
            photo.Allocation.Height / Convert.ToDouble(pixbuf.Height)
        );
        
        if (ratio < 1)
        {
            pixbuf = pixbuf.ScaleSimple(
                Convert.ToInt32(Math.Round(pixbuf.Width * ratio)),
                Convert.ToInt32(Math.Round(pixbuf.Height * ratio)),
                Gdk.InterpType.Bilinear);
        }
        photo.Pixbuf = pixbuf;
    }

    private void Initialize()
    {
        box = new HBox(false, 0);
        sideBar = new VBox(false, 0);
        VSeparator sideBarSeparator = new VSeparator();

        imageBin = new Frame();

        box.PackStart(sideBar, false, false, 3);
        box.PackStart(sideBarSeparator, false, false, 5);
        box.PackStart(imageBin, true, true, 2);

        Label sideBarLabel = new Label("Side Bar");
        sideBar.PackStart(sideBarLabel, true, true, 3);

        photo = new Image();
        imageBin.Add(photo);
    }
}
