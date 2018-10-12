using Gtk;

public class ToolbarPresenter
{
    private ToolbarView view;

    public ToolbarPresenter()
    {
        this.view = new ToolbarView(this);
    }

    public Widget Widget { get { return this.view.AsWidget; } }

    public void ChooseFolderClicked()
    {
    }
}
