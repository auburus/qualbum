using System;
using Gtk;

class PhotoPresenter
{
    private WorkingFolderModel _model;
    private PhotoView _view;

    public PhotoPresenter(WorkingFolderModel model)
    {
        _model = model;
        _view = new PhotoView(this);
    }

    public void NextPhoto()
    {
        throw new NotImplementedException();
    }
    
    public void PrevPhoto()
    {
        throw new NotImplementedException();
    }

    // Proxy function until we finish refactoring
    public void Display(string path)
    {
        this._view.Display(path);
    }

    public void InsertView(Container container)
    {
        container.Add(this._view.AsWidget);
    }
}
