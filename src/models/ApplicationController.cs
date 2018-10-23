using System.IO;
using Gtk;
using Gdk;

namespace Qualbum
{
    public class ApplicationController
    {
        private Library library;

        private DialogPresenter dialogPresenter;
        private MenuPresenter menuPresenter;
        private ToolbarPresenter toolbarPresenter;
        private PhotoPresenter photoPresenter;


        public ApplicationController(Qualbum qualbum)
        {
            this.dialogPresenter = new DialogPresenter(qualbum);
            this.menuPresenter = new MenuPresenter();
            this.toolbarPresenter = new ToolbarPresenter(this);

            // Rigth now, it defaults to this library
            library = new Library("/home/jordi/files/Projectes/qualbum/test/library");
            this.photoPresenter = new PhotoPresenter(new Importer(library));
        }

        public void ImportFolder()
        {
            DirectoryInfo dir = dialogPresenter.ChooseFolderDialog();

            if (dir == null) {
                return;
            }

            photoPresenter.ChangeCollection(
                PhotoCollection.CreateFromDirectory(dir),
                dir
            );
        }

        public Widget Widget
        {
            get {
                VBox app = new VBox(false, 0);

                app.PackStart(menuPresenter.Widget, false, true, 0);
                app.PackStart(toolbarPresenter.Widget, false, false, 0);
                app.PackStart(photoPresenter.Widget, true, true, 3);

                return app;
            }
        }

        [GLib.ConnectBefore]
        public void OnKeyPress(object sender, Gtk.KeyPressEventArgs args)
        {
            if ((args.Event.State & Gdk.ModifierType.ControlMask) ==
                Gdk.ModifierType.ControlMask)
            {
                switch (args.Event.Key)
                {
                    case Gdk.Key.o:
                        ImportFolder();
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

    }
}
