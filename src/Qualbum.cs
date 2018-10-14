using Gtk;
using System;
using System.IO;
using Gdk;
using System.Linq;
using System.Collections.Generic; // Remove when remove dict

class Qualbum : Gtk.Window
{
    private WorkingDirModel workingDir;
    private LibraryModel library;
    private PhotoPresenter photoPresenter;
    private MenuPresenter menuPresenter;
    private ToolbarPresenter toolbarPresenter;

    public Qualbum() : base("Qualbum")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetIconFromFile("icon.png");

        workingDir = new WorkingDirModel();

        photoPresenter = new PhotoPresenter(workingDir);
        menuPresenter = new MenuPresenter();
        toolbarPresenter = new ToolbarPresenter(this, workingDir);

        DeleteEvent += new DeleteEventHandler(OnDelete);
        KeyPressEvent += new KeyPressEventHandler(OnKeyPress);
        workingDir.DirectoryChangedEvent +=
            photoPresenter.WorkingDirectoryChangedHandler;
        
        Widget app = BuildApp();

        Add(app);
        Maximize();

        ShowAll();
    }

    public static void Main()
    {
        Application.Init();
        new Qualbum();
        Application.Run();
    }

    private Widget BuildApp()
    {
        // All app is a vbox
        VBox app = new VBox(false, 0);

        app.PackStart(menuPresenter.Widget, false, true, 0);
        app.PackStart(toolbarPresenter.Widget, false, false, 0);
        app.PackStart(photoPresenter.Widget, true, true, 3);

        return app;
    }

    [GLib.ConnectBefore]
    void OnKeyPress(object sender, Gtk.KeyPressEventArgs args)
    {
        if ((args.Event.State & Gdk.ModifierType.ControlMask) ==
            Gdk.ModifierType.ControlMask)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.o:
                    toolbarPresenter.ChooseFolder();
                    break;

                case Gdk.Key.h:
                    List<System.Drawing.Color> hash1 = 
                        workingDir.GetHashFromCurrent();

                    photoPresenter.NextPhoto();

                    Console.WriteLine("\n\n----------------\n");

                    List<System.Drawing.Color> hash2 = 
                        workingDir.GetHashFromCurrent();
                    Console.WriteLine("");

                    Console.WriteLine(
                        workingDir.HashDifference(hash1, hash2)
                    );
                    break;

            }
        }
        else if ((args.Event.State & Gdk.ModifierType.ShiftMask) ==
            Gdk.ModifierType.ShiftMask)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Left:
                    photoPresenter.Rotate(Gdk.PixbufRotation.Counterclockwise);
                    break;
                case Gdk.Key.Right:
                    photoPresenter.Rotate(Gdk.PixbufRotation.Clockwise);
                    break;
            }
        }
        else if ((args.Event.State & Gdk.ModifierType.None) ==
            Gdk.ModifierType.None)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Left:
                    photoPresenter.PrevPhoto();
                    break;
                case Gdk.Key.Right:
                    photoPresenter.NextPhoto();
                    break;
                case Gdk.Key.Delete:
                    photoPresenter.DeletePhoto();
                    break;
            }
        }
    }

    void OnExit(object sender, EventArgs args)
    {
        Application.Quit();
    }

    void OnDelete(object obj, DeleteEventArgs args)
    {
        Application.Quit();
    }
}

class DirectoryWithoutImagesException : Exception
{
    public DirectoryWithoutImagesException() : base() {}
    public DirectoryWithoutImagesException(string message) : base(message) {}
    public DirectoryWithoutImagesException(string message, System.Exception inner)
        : base(message, inner) {}
    protected DirectoryWithoutImagesException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context
    ) {}
}
