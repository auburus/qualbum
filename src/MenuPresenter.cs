using Gtk;

public class MenuPresenter
{
    private MenuView View;

    public MenuPresenter(MenuView view)
    {
        this.View = view;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
