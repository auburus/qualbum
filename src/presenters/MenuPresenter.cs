using Gtk;

public class MenuPresenter
{
    private MenuView view;

    public MenuPresenter()
    {
        this.view = new MenuView(this);
    }

    public Widget Widget { get { return this.view.AsWidget; } }

    public void Exit()
    {
        Application.Quit();
    }
}
