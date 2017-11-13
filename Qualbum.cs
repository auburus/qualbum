using Gtk;
using System;
using System.IO;
using Gdk;
using System.Linq;

class SharpApp : Gtk.Window
{
    DisplayController DisplayController;
    Label ActiveDirectoryLabel;
    Label ImageCounterLabel;

    public SharpApp() : base("Qualbum")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetIconFromFile("icon.png");

        DeleteEvent += new DeleteEventHandler(OnDelete);
        KeyPressEvent += new KeyPressEventHandler(OnKeyPress);
        
        VBox app = BuildApp();

        Add(app);
        //Maximize();

        ShowAll();
    }

    private VBox BuildApp()
    {
        // All app is a vbox
        VBox app = new VBox(false, 0);

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

        app.PackStart(menuBar, false, true, 0);

        // Add toolbar
        Toolbar toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Icons;

        ToolButton opentb = new ToolButton(Stock.Open);
        opentb.Clicked += OnChooseFolderClicked;

        toolbar.Insert(opentb, 0);

        app.PackStart(toolbar, false, false, 0);

        // Add activeDirectoryLabel
        HBox activeDirectoryBox = new HBox(false, 0);
        ActiveDirectoryLabel = new Label("No directory currently selected");
        activeDirectoryBox.PackStart(ActiveDirectoryLabel, false, false, 15);
        ImageCounterLabel = new Label("0 of 0");
        activeDirectoryBox.PackStart(ImageCounterLabel, false, false, 15);

        app.PackStart(activeDirectoryBox, false, false, 5);

        HSeparator separator = new HSeparator();
        app.PackStart(separator, false, false, 0);

        // Define mainscreen
        DisplayController = new DisplayController();

        app.PackStart(DisplayController.Box, true, true, 3);

        return app;
    }

    public static void Main()
    {
        Application.Init();
        new SharpApp();
        Application.Run();
    }

    void OnChooseFolderClicked(object obj, EventArgs args)
    {
        FileChooserDialog fc = new FileChooserDialog(
                "Select folder",
                this,
                FileChooserAction.SelectFolder,
                "Cancel" , ResponseType.Cancel,
                "Select", ResponseType.Accept);

        if (fc.Run() == (int)ResponseType.Accept)
        {
            ChangeWorkingDirectory(new DirectoryInfo(fc.CurrentFolder));
        }

        fc.Destroy();
    }

    void ChangeWorkingDirectory(DirectoryInfo newDirectory)
    {
        DisplayController.ChangeDirectory(newDirectory);
        ActiveDirectoryLabel.Text = newDirectory.FullName;
        ChangeToNextImage(0);
    }

    void UpdateImageCounterLabel()
    {
        if (DisplayController.ImageFiles.Any()) {
            ImageCounterLabel.Text =
                (DisplayController.ImageIndex + 1).ToString() +
                " of " +
                DisplayController.ImageFiles.Count.ToString();
        }
    }

    void ChangeToNextImage(int step)
    {
        DisplayController.NextImage(step);
        UpdateImageCounterLabel();
    }

    [GLib.ConnectBefore]
    void OnKeyPress(object sender, Gtk.KeyPressEventArgs args)
    {
        switch (args.Event.Key)
        {
            case Gdk.Key.Left:
                ChangeToNextImage(-1);
                break;
            case Gdk.Key.Right:
                ChangeToNextImage(1);
                break;
        }
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

    void OnExit(object sender, EventArgs args)
    {
        Application.Quit();
    }

    void OnDelete(object obj, DeleteEventArgs args)
    {
        Application.Quit();
    }
}

class DirectoryWithoutImagesException : Exception
{
    public DirectoryWithoutImagesException() : base() {}
    public DirectoryWithoutImagesException(string message) : base(message) {}
    public DirectoryWithoutImagesException(string message, System.Exception inner)
        : base(message, inner) {}
    protected DirectoryWithoutImagesException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context
    ) {}
}
