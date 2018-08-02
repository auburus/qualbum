using System;
using Gtk;
using System.IO;

class PhotoPresenter
{
    private WorkingDirModel _model;
    private PhotoView _view;

    public PhotoPresenter(WorkingDirModel model)
    {
        _model = model;
        _view = new PhotoView();
    }

    public void FirstPhoto()
    {
        this._model.GoFirstPhoto();
        Display(this._model.CurrentPhotoPath);
    }

    public void NextPhoto()
    {
        this._model.IncrementPhoto(1);
        Display(this._model.CurrentPhotoPath);
    }

    public void PrevPhoto()
    {
        this._model.IncrementPhoto(-1);
        Display(this._model.CurrentPhotoPath);
    }

    public void DeletePhoto()
    {
        this._model.DeleteCurrentPhoto();
        Display(this._model.CurrentPhotoPath);
    }

    // Proxy function until we finish refactoring
    public void Display(string path)
    {
        if (path == null) {
            this._view.Display(this.defaultPhotoPath);
            return;
        } 

        this._view.Display(path);
    }

    public void AttachWidget(Container container)
    {
        container.Add(this._view.AsWidget);
    }

    private String defaultPhotoPath {
        get {
            return Path.Combine(
                new FileInfo(
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                ).Directory.FullName,
                "config" + Path.DirectorySeparatorChar + "default.jpg"
            );
        }
    }
}
