using Gtk;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class SharpApp : Window
{
    DirectoryInfo activeDirectory;
    MainScreen mainScreen;
    Label activeDirectoryLabel;

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

        toolbar.Insert(opentb, 0);

        app.PackStart(toolbar, false, false, 0);

        // Add activeDirectory
        HBox activeDirectoryBox = new HBox(false, 0);
        activeDirectoryLabel = new Label("No directory currently selected");
        activeDirectoryLabel.Justify = Gtk.Justification.Left;
        activeDirectoryBox.PackStart(activeDirectoryLabel, false, false, 15);

        app.PackStart(activeDirectoryBox, false, false, 5);

        HSeparator separator = new HSeparator();
        app.PackStart(separator, false, false, 0);

        // Define mainscreen
        mainScreen = new MainScreen();

        app.PackStart(mainScreen.Box, true, true, 3);

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

    void OnChooseFolderClicked(object obj, EventArgs args)
    {
        Gdk.Pixbuf firstPicture;

        FileChooserDialog fc = new FileChooserDialog(
                "Select folder",
                this,
                FileChooserAction.SelectFolder,
                "Cancel" , ResponseType.Cancel,
                "Select", ResponseType.Accept);
        if (fc.Run() == (int)ResponseType.Accept)
        {
            activeDirectory = new DirectoryInfo(fc.CurrentFolder);
            activeDirectoryLabel.Text = activeDirectory.FullName;
            firstPicture = GetFirstImage(activeDirectory);
            mainScreen.DisplayImage(firstPicture);
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

    Gdk.Pixbuf GetFirstImage(DirectoryInfo directory)
    {
        IEnumerable<FileSystemInfo> images = Finder.FindImages(directory);
        if (!images.Any() )
        {
            throw new DirectoryWithoutImagesException(
                "Directory " + directory.FullName + " doesn't contain any images");
        }

        return new Gdk.Pixbuf(images.First().FullName);
    }
}

class MainScreen
{
    public HBox Box;
    private Label picLabel ;
    private VBox sideBar;
    private Image bigImage;

    public MainScreen()
    {
        Box = new HBox(false, 0);
        picLabel = new Label("Picture");
        sideBar = new VBox(false, 0);
        VSeparator sideBarSeparator = new VSeparator();
        bigImage = new Image();

        Box.PackStart(sideBar, false, false, 3);
        Box.PackStart(sideBarSeparator, false, false, 5);
        //Box.PackStart(picLabel, true, true, 2);
        Box.PackStart(bigImage, true, true, 2);

        Label sideBarLabel = new Label("Side Bar");
        sideBar.PackStart(sideBarLabel, true, true, 3);

    }

    public void DisplayImage(Gdk.Pixbuf pixBuf)
    {
        bigImage.Pixbuf = pixBuf;
        Console.WriteLine("Hola");
    }
}

class DirectoryWithoutImagesException : System.Exception
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
