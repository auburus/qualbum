using Gtk;
using System;
using System.IO;
using Gdk;
using System.Linq;
using System.Collections.Generic; // Remove when remove dict

namespace Qualbum {

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


            library = new LibraryModel("/home/jordi/files/Projectes/qualbum/test/library");
            workingDir = new WorkingDirModel();


            photoPresenter = new PhotoPresenter(new Importer(library));
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

        public static void Run()
        {
            Application.Init();
            new Qualbum();
            Application.Run();
        }

        public static DirectoryInfo BaseFolder {
            get {
                return new FileInfo(
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                    ).Directory;
            }
        }

        public static DirectoryInfo ConfigFolder {
            get {
                return Qualbum.BaseFolder.GetDirectories("config")[0];
            }
        }

        private Widget BuildApp()
        {
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
                        photoPresenter.NextPhoto();
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

    //TODO Move that to a proper file
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
}
