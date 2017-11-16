using Gtk;
using System;

public class ToolbarView
{
    private ToolbarPresenter Presenter;

    public ToolbarView()
    {
        this.Presenter = new ToolbarPresenter(this);
    }

    public Toolbar Widget
    {
        get
        {
            Toolbar toolbar = new Toolbar();
            toolbar.ToolbarStyle = ToolbarStyle.Icons;

            ToolButton opentb = new ToolButton(Stock.Open);
            opentb.Clicked += OnChooseFolderClicked;

            toolbar.Insert(opentb, 0);

            return toolbar;
        }
    }

    private void OnChooseFolderClicked(object obj, EventArgs args)
    {
        Presenter.ChooseFolderClicked();
        //ShowChooseFolderDialog();
    }
}

