using System;
using Gtk;

class PhotoView
{
    private PhotoPresenter _presenter;
    private Image photo;

    public PhotoView(PhotoPresenter presenter)
    {
        _presenter = presenter;
        Initialize();
    }

    public Widget AsWidget { get { return photo; } }

    public void Display(string path)
    {
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
        photo = new Image();
    }
}
