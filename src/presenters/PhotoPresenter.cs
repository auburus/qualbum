using System;
using Gtk;
using System.IO;

class PhotoPresenter
{
    private WorkingDirModel model;
    private PhotoView view;

    public PhotoPresenter(WorkingDirModel model)
    {
        this.model = model;
        this.view = new PhotoView();
    }

    public void FirstPhoto()
    {
        this.model.GoFirstPhoto();
        Display(this.model.CurrentPhoto);
    }

    public void NextPhoto()
    {
        this.model.IncrementPhoto(1);
        Display(this.model.CurrentPhoto);
    }

    public void PrevPhoto()
    {
        this.model.IncrementPhoto(-1);
        Display(this.model.CurrentPhoto);
    }

    public void DeletePhoto()
    {
        this.model.DeleteCurrentPhoto();
        Display(this.model.CurrentPhoto);
    }

    // Proxy function until we finish refactoring
    public void Display(FileInfo photoFile)
    {
        if (photoFile == null) {
            this.view.Display(this.defaultPhoto);
            return;
        } 

        this.view.Display(photoFile);
        this.view.UpdateCounterLabel(model.CurrentPhotoIndex,
                model.TotalNumberPhotos);
    }

    public void Rotate(Gdk.PixbufRotation rotation)
    {
        this.view.Rotate(rotation);

        // Rotate the original picture and save it
        Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(this.model.CurrentPhoto.FullName);
        pixbuf = pixbuf.RotateSimple(rotation);

        // All images provided by finder are jpgs or pngs
        String extension = this.model.CurrentPhoto.Extension
            .Substring(1).ToLower();

        if (extension == "jpg") {
            extension = "jpeg";
        }
        pixbuf.Save(this.model.CurrentPhoto.FullName, extension);
    }

    public Widget Widget { get { return this.view.AsWidget; } }

    private FileInfo defaultPhoto {
        get {
            return new FileInfo(
                Path.Combine(
                    new FileInfo(
                        System.Reflection.Assembly.GetExecutingAssembly().Location
                    ).Directory.FullName,
                    "config" + Path.DirectorySeparatorChar + "default.jpg"
                )
            );
        }
    }

    public void WorkingDirectoryChangedHandler(object sender,
            DirectoryChangedEventArgs eventArgs)
    {
        this.view.ActiveDirectoryLabel.Text = eventArgs.NewDirectory.FullName;
        this.FirstPhoto();
    }

}
