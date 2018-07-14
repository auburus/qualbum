using System;
using Gtk;

class PhotoPresenter
{
    private WorkingDirModel _model;
    private PhotoView _view;

    public PhotoPresenter(WorkingDirModel model)
    {
        _model = model;
        _view = new PhotoView(this);
    }

    public void FirstPhoto()
    {
        this._model.GoFirstPhoto();
        this._view.Display(this._model.CurrentPhoto);
    }

    public void NextPhoto()
    {
        this._model.IncrementPhoto(1);
        this._view.Display(this._model.CurrentPhoto);
    }
    
    public void PrevPhoto()
    {
        this._model.IncrementPhoto(-1);
        this._view.Display(this._model.CurrentPhoto);
    }

    // Proxy function until we finish refactoring
    public void Display(string path)
    {
        this._view.Display(path);
    }

    public void AttachWidget(Container container)
    {
        container.Add(this._view.AsWidget);
    }
}
