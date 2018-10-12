using Gtk;
using System;

public class MenuView
{
    private MenuPresenter presenter;

    public MenuView(MenuPresenter presenter)
    {
        this.presenter = presenter;
    }

    public Widget AsWidget
    { 
        get
        {
            MenuBar menuBar = new MenuBar();

            MenuItem file = FileMenuItem();
            menuBar.Append(file);

            // Add help menu
            MenuItem help = HelpMenuItem();
            menuBar.Append(help);

            return menuBar;
        }
    }

    private MenuItem FileMenuItem()
    {
        MenuItem file = new MenuItem("File");

        Menu fileMenu = new Menu();
        file.Submenu = fileMenu;

        MenuItem exit = new MenuItem("Exit");
        exit.Activated += OnExit;
        fileMenu.Append(exit);

        return file;
    }
    
    private MenuItem HelpMenuItem()
    {
        MenuItem help = new MenuItem("Help");

        Menu helpMenu = new Menu();
        help.Submenu = helpMenu;

        MenuItem about = new MenuItem("About");
        about.Activated += OnAbout;

        helpMenu.Append(about);

        return help;
    }

    private void OnExit(object sender, EventArgs args)
    {
        presenter.Exit();
    }

    private void OnAbout(object sender, EventArgs args)
    {
        AboutDialog dialog = new AboutDialog();

        dialog.ProgramName = "Qualbum";
        dialog.Version = "0.0.1";
        dialog.Comments = @"Thanks for using this program. Forward any comments
            or suggestions to the following website:";
        dialog.Authors = new string [] {"Jordi Nonell"};
        dialog.Website = "https://jnonell.com";

        dialog.Run();
        dialog.Destroy();
    }

}
