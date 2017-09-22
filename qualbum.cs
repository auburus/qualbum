using Gtk;
using System;

class SharpApp : Window
{
    public SharpApp() : base("Qualbum")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetIconFromFile("icon.png");

        DeleteEvent += new DeleteEventHandler(OnDelete);
        
        // All app is a vbox
        VBox app = new VBox(false, 2);

        // Add menu
        MenuBar menuBar = new MenuBar();

            // Add file menu
            MenuItem file = new MenuItem("File");
            menuBar.Append(file);

            Menu fileMenu = new Menu();
            file.Submenu = fileMenu;

            MenuItem exit = new MenuItem("Exit");
            exit.Activated += OnExit;
            fileMenu.Append(exit);


            // Add help menu
            MenuItem help = new MenuItem("Help");
            menuBar.Append(help);

            Menu helpMenu = new Menu();
            help.Submenu = helpMenu;

            MenuItem about = new MenuItem("About");
            about.Activated += OnAbout;

            helpMenu.Append(about);


        app.PackStart(menuBar, false, false, 0);

        // Add toolbar
        Toolbar toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Icons;

        ToolButton opentb = new ToolButton(Stock.Open);
        ToolButton opentb2 = new ToolButton(Stock.Open);
        SeparatorToolItem sep = new SeparatorToolItem();
        ToolButton quittb = new ToolButton(Stock.Quit);
        quittb.Clicked += OnExit;

        toolbar.Insert(opentb, 0);
        toolbar.Insert(opentb2, 1);
        toolbar.Insert(sep, 2);
        toolbar.Insert(quittb, 3);

        app.PackStart(toolbar, false, false, 3);

        // Define mainscreen
        HBox mainScreen = new HBox(false, 3);

        Label picLabel = new Label("Picture");
        VBox sideBar = new VBox(false, 3);
        VSeparator sideBarSeparator = new VSeparator();

        mainScreen.PackStart(sideBar, false, false, 3);
        mainScreen.PackStart(sideBarSeparator, false, false, 5);
        mainScreen.PackStart(picLabel, true, true, 2);

        Label sideBarLabel = new Label("Side Bar");
        sideBar.PackStart(sideBarLabel, true, true, 3);

        app.PackStart(mainScreen, true, true, 3);

        // Add the main box and maximize the app
        Add(app);
        Maximize();

        // Show all
        ShowAll();

    }

    public static void Main()
    {
        Application.Init();
        new SharpApp();
        Application.Run();
    }

    void OnDelete(object obj, DeleteEventArgs args)
    {
        Application.Quit();
    }

    void OnExit(object sender, EventArgs args)
    {
        Application.Quit();
    }

    void OnAbout(object sender, EventArgs args)
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
