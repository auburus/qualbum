using Gtk;
using System.IO;

namespace Qualbum {

    public class ToolbarPresenter
    {
        private ToolbarView view;
        private Gtk.Window window;
        private WorkingDirModel workingDir;

        public ToolbarPresenter(Gtk.Window window, WorkingDirModel model)
        {
            this.view = new ToolbarView(this);
            this.window = window;
            this.workingDir = model;
        }

        public Widget Widget { get { return this.view.AsWidget; } }

        public void ChooseFolder()
        {
            ShowChooseFolderDialog();

        }

        public void ShowChooseFolderDialog()
        {
            Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog(
                    "Select folder",
                    this.window,
                    FileChooserAction.SelectFolder,
                    "Cancel" , ResponseType.Cancel,
                    "Select", ResponseType.Accept);

            if (fc.Run() == (int)ResponseType.Accept)
            {
                this.workingDir.ChangeDirectory(new DirectoryInfo(fc.CurrentFolder));
            }

            fc.Destroy();
        }

    }
}
