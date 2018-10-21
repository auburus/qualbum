using Gtk;
using System.IO;

namespace Qualbum
{

    public class DialogPresenter
    {
        private Gtk.Window window;

        public DialogPresenter(Gtk.Window window)
        {
            this.window = window;
        }

        public DirectoryInfo ChooseFolderDialog()
        {
            DirectoryInfo choosenDir = null;

            Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog(
                    "Select folder",
                    this.window,
                    FileChooserAction.SelectFolder,
                    "Cancel" , ResponseType.Cancel,
                    "Select", ResponseType.Accept);

            if (fc.Run() == (int)ResponseType.Accept)
            {
                choosenDir = new DirectoryInfo(fc.CurrentFolder);
            }

            fc.Destroy();

            return choosenDir;
        }
    }

}
