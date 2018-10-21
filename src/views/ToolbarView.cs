using Gtk;
using System;

namespace Qualbum
{

    public class ToolbarView
    {
        private ToolbarPresenter presenter;

        public ToolbarView(ToolbarPresenter presenter)
        {
            this.presenter = presenter;
        }

        public Toolbar AsWidget
        {
            get
            {
                Toolbar toolbar = new Toolbar();
                toolbar.ToolbarStyle = ToolbarStyle.Icons;

                ToolButton opentb = new ToolButton(Stock.Open);
                opentb.Clicked += ImportFolderClicked;

                toolbar.Insert(opentb, 0);

                return toolbar;
            }
        }

        private void ImportFolderClicked(object obj, EventArgs args)
        {
            presenter.ImportFolder();
        }
    }

}
