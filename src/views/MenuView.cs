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

            MenuItem file = fileMenuItem();
            menuBar.Append(file);

            MenuItem library = libraryMenuItem();
            menuBar.Append(library);

            MenuItem help = helpMenuItem();
            menuBar.Append(help);

            return menuBar;
        }
    }

    private MenuItem fileMenuItem()
    {
        MenuItem file = new MenuItem("File");

        Menu fileMenu = new Menu();
        file.Submenu = fileMenu;

        MenuItem exit = new MenuItem("Exit");
        exit.Activated += OnExit;
        fileMenu.Append(exit);

        return file;
    }
    
    private MenuItem libraryMenuItem()
    {
        MenuItem library = new MenuItem("Library");

        Menu libraryMenu = new Menu();
        library.Submenu = libraryMenu;

        MenuItem conf = new MenuItem("Configure");
        conf.Activated += OnConfigure;
        libraryMenu.Append(conf);

        MenuItem import = new MenuItem("Import folder...");
        import.Activated += OnImport;
        libraryMenu.Append(import);

        return library;
    }
    
    private MenuItem helpMenuItem()
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

    private void OnConfigure(object sender, EventArgs args)
    {
        // Display configure view (like preferences) with options and stuff
    }

    private void OnImport(object sender, EventArgs args)
    {
        // Select folder to import, and start the import process
    }
}
