using System;
using Gtk;
using System.IO;

class PhotoView
{
    public Label ActiveDirectoryLabel;
    private Label imageCounterLabel;

    private VBox mainBox;
    private Bin imageBin;
    private Image photo;

    private Label pathLabel;
    private Label sizeLabel;
    private Label dateLabel;
    private Label dimLabel;

    private Gdk.Pixbuf pixbuf;

    public PhotoView()
    {
        Initialize();
    }

    public Widget AsWidget { get { return mainBox; } }

    public void Display(FileInfo photoFile)
    {
        // Force the GC to run to claim the memory that the different
        // new pixbuf keep using, that for some reason doesn't get
        // reclaimed automatically.
        // Reference: https://github.com/gtkd-developers/GtkD/issues/127
        GC.Collect();
        GC.WaitForPendingFinalizers();

        pixbuf = new Gdk.Pixbuf(photoFile.FullName);
        Display(pixbuf);
    }

    public void UpdateCounterLabel(int currentPhoto, int totalPhotos)
    {
        this.imageCounterLabel.Text = (currentPhoto+1).ToString() + " of " +
            totalPhotos.ToString();
    }

    public void Rotate(Gdk.PixbufRotation rotation)
    {
        this.pixbuf = this.pixbuf.RotateSimple(rotation);
        Display(pixbuf);
    }

    public void FillLabels(FileInfo photoFile)
    {
        pathLabel.Text = photoFile.FullName;

        float size = photoFile.Length;
        String unit = "Bytes";

        if (size / 1024 > 1) {
            size = size / 1024;
            unit = "Kb";
        }
        if (size / 1024 > 1) {
            size = size / 1024;
            unit = "Mb";
        }

        sizeLabel.Text = size.ToString("0.0") + " " + unit;

        dateLabel.Text = Finder.GuessDate(photoFile).ToString("dd MMM yyyy");

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
        mainBox = new VBox(false, 0);

        HBox activeDirectoryBox = new HBox(false, 0);
        ActiveDirectoryLabel = new Label("No directory currently selected");
        activeDirectoryBox.PackStart(ActiveDirectoryLabel, false, false, 15);
        imageCounterLabel = new Label("0 of 0");
        activeDirectoryBox.PackStart(imageCounterLabel, false, false, 5);

        mainBox.PackStart(activeDirectoryBox, false, false, 0);


        // HSeparator separator = new HSeparator();
        // mainBox.PackStart(separator, false, false, 0);


        VBox box = new VBox(false, 0);

        imageBin = new Frame();
        photo = new Image();
        imageBin.Add(photo);

        box.PackStart(imageBin, true, true, 2);


        Table t = new Table(2, 3, false);

        t.Attach(new Label("Path: "), 0, 1, 0, 1,
                Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);
        pathLabel = new Label("");
        t.Attach(pathLabel, 1, 2, 0, 1,
                Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);

        t.Attach(new Label("Size: "), 0, 1, 1, 2,
                Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);
        sizeLabel = new Label("");
        t.Attach(sizeLabel, 1, 2, 1, 2,
                Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);

        t.Attach(new Label("Date: "), 0, 1, 2, 3,
                Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);
        dateLabel = new Label("");
        t.Attach(dateLabel, 1, 2, 2, 3,
                Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);

        // t.Attach(new Label("Dimensions: "), 0, 1, 2, 3,
        //         Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);
        // dimLabel = new Label("dimLabel");
        // t.Attach(dimLabel, 1, 2, 2, 3,
        //         Gtk.AttachOptions.Shrink, Gtk.AttachOptions.Shrink, 0, 0);

        t.ShowAll();

        box.PackStart(t, false, false, 3);

        mainBox.PackStart(box, true, true, 3);
    }
}
