using Gtk;
using System.IO;

namespace Qualbum
{

    public class ToolbarPresenter
    {
        private ToolbarView view;
        private ApplicationController appController;

        public ToolbarPresenter(ApplicationController appController)
        {
            this.appController = appController;
            this.view = new ToolbarView(this);
        }

        public Widget Widget { get { return this.view.AsWidget; } }

        public void ImportFolder()
        {
            appController.ImportFolder();
        }
    }
}
