using Gtk;
using System;
using System.IO;
using Gdk;
using System.Linq;
using System.Collections.Generic; // Remove when remove dict

class QualbumMain : Gtk.Window
{
    Label ActiveDirectoryLabel;
    Label ImageCounterLabel;
    private WorkingDirModel workingDir;
    private PhotoPresenter photoPresenter;
    private MenuPresenter menuPresenter;
    private ToolbarPresenter toolbarPresenter;

    public QualbumMain() : base("Qualbum")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetIconFromFile("icon.png");

        workingDir = new WorkingDirModel();
        workingDir.DirectoryChangedEvent += changeWorkingDirectoryHandler;

        photoPresenter = new PhotoPresenter(workingDir);
        menuPresenter = new MenuPresenter();
        toolbarPresenter = new ToolbarPresenter();

        DeleteEvent += new DeleteEventHandler(OnDelete);
        KeyPressEvent += new KeyPressEventHandler(OnKeyPress);
        
        VBox app = BuildApp();

        Add(app);
        Maximize();

        ShowAll();
    }

    public static void Main()
    {
        Application.Init();
        new QualbumMain();
        Application.Run();
    }

    private VBox BuildApp()
    {
        // All app is a vbox
        VBox app = new VBox(false, 0);

        app.PackStart(menuPresenter.Widget, false, true, 0);
        app.PackStart(toolbarPresenter.Widget, false, false, 0);


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

        Container mainScreen = new HBox();
        app.PackStart(mainScreen, true, true, 3);

        mainScreen.Add(photoPresenter.Widget);
        //photoPresenter.AttachWidget(mainScreen);

        return app;
    }

    void ShowChooseFolderDialog()
    {
        FileChooserDialog fc = new FileChooserDialog(
                "Select folder",
                this,
                FileChooserAction.SelectFolder,
                "Cancel" , ResponseType.Cancel,
                "Select", ResponseType.Accept);

        if (fc.Run() == (int)ResponseType.Accept)
        {
            workingDir.ChangeDirectory(new DirectoryInfo(fc.CurrentFolder));
        }

        fc.Destroy();
    }

    public void changeWorkingDirectoryHandler(object sender, DirectoryChangedEventArgs eventArgs)
    {
        ActiveDirectoryLabel.Text = eventArgs.NewDirectory.FullName;
        photoPresenter.FirstPhoto();
    }

    /*
    void UpdateImageCounterLabel()
    {
        if (DisplayController.ImageFiles.Any()) {
            ImageCounterLabel.Text =
                (DisplayController.ImageIndex + 1).ToString() +
                " of " +
                DisplayController.ImageFiles.Count.ToString();
        }
    }
    */

    [GLib.ConnectBefore]
    void OnKeyPress(object sender, Gtk.KeyPressEventArgs args)
    {
        if ((args.Event.State & Gdk.ModifierType.ControlMask) ==
            Gdk.ModifierType.ControlMask)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.o:
                    ShowChooseFolderDialog();
                    break;

                case Gdk.Key.h:
                    List<System.Drawing.Color> hash1 = 
                        workingDir.GetHashFromCurrent();

                    photoPresenter.NextPhoto();

                    Console.WriteLine("\n\n----------------\n");

                    List<System.Drawing.Color> hash2 = 
                        workingDir.GetHashFromCurrent();
                    Console.WriteLine("");

                    Console.WriteLine(
                        workingDir.HashDifference(hash1, hash2)
                    );
                    break;
            }
        }
        else if ((args.Event.State & Gdk.ModifierType.None) ==
            Gdk.ModifierType.None)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Left:
                    photoPresenter.PrevPhoto();
                    break;
                case Gdk.Key.Right:
                    photoPresenter.NextPhoto();
                    break;
                case Gdk.Key.Delete:
                    photoPresenter.DeletePhoto();
                    break;
            }
        }
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
