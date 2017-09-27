using Gtk;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class SharpApp : Window
{
    Label sideBarLabel;
    String activeFolder;

    public SharpApp() : base("Qualbum")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetIconFromFile("icon.png");

        DeleteEvent += new DeleteEventHandler(OnDelete);
        
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
        //ToolButton opentb2 = new ToolButton(Stock.Open);
        //SeparatorToolItem sep = new SeparatorToolItem();
        //ToolButton quittb = new ToolButton(Stock.Quit);
        //quittb.Clicked += OnExit;

        toolbar.Insert(opentb, 0);
        //toolbar.Insert(opentb2, 1);
        //toolbar.Insert(sep, 2);
        //toolbar.Insert(quittb, 3);

        app.PackStart(toolbar, false, false, 0);

        // Define mainscreen
        HBox mainScreen = new HBox(false, 0);

        Label picLabel = new Label("Picture");
        VBox sideBar = new VBox(false, 0);
        VSeparator sideBarSeparator = new VSeparator();

        mainScreen.PackStart(sideBar, false, false, 3);
        mainScreen.PackStart(sideBarSeparator, false, false, 5);
        mainScreen.PackStart(picLabel, true, true, 2);

        sideBarLabel = new Label("Side Bar");
        sideBar.PackStart(sideBarLabel, true, true, 3);

        app.PackStart(mainScreen, true, true, 3);

        // Add the main box and maximize the app
        Add(app);
        Maximize();

        // Show all
        ShowAll();

        foreach (FileSystemInfo pic in Finder.FindImages(new DirectoryInfo("./")))
        {
            System.Console.WriteLine(pic);
        }
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
            activeFolder = fc.CurrentFolder;
        }

        fc.Destroy();
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

class Finder
{
    /**
     * Find all images in the current directory subtree.
     * It doesn't find images in hidden directories, nor hidden images,
     * and it does that on purpose.
     *
     * The reason why is because if it's hidden, we don't want to "see" it.
     */
    public static IEnumerable<FileSystemInfo> FindImages(DirectoryInfo directory)
    {
        return directory.EnumerateDirectories("*", SearchOption.AllDirectories)
            .Where( d => (d.Attributes & FileAttributes.Hidden) == 0)
            .Select( d => d.EnumerateFiles())
            .Aggregate(
                Enumerable.Empty<FileSystemInfo>(),
                (list, newlist) => list.Concat(newlist)
            )
            .Where( f => (f.Attributes & FileAttributes.Hidden) == 0)
            .Where( f => Regex.IsMatch(f.Extension, @"\.[jpg|jpeg|png|gif]"));
    }
}
