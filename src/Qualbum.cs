using Gtk;
using System;
using System.IO;
using Gdk;
using System.Linq;

class Qualbum : Gtk.Window
{
    DisplayController DisplayController;
    Label ActiveDirectoryLabel;
    Label ImageCounterLabel;

    public Qualbum() : base("Qualbum")
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

        MenuView menu = new MenuView();
        app.PackStart(menu.Widget, false, true, 0);

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

    public static void Initialize()
    {
        Application.Init();
        new Qualbum();
        Application.Run();
    }

    void OnChooseFolderClicked(object obj, EventArgs args)
    {
        ShowChooseFolderDialog();
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

    public void DeleteCurrentImage()
    {
        FileSystemInfo currentImageFile =
            DisplayController.ImageFiles[DisplayController.ImageIndex];

        File.SetAttributes(
            currentImageFile.FullName,
            File.GetAttributes(currentImageFile.FullName) | FileAttributes.Hidden
        );

        DisplayController.ImageFiles.RemoveAt(DisplayController.ImageIndex);
        ChangeToNextImage(0);
    }


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
            }
        }
        else if ((args.Event.State & Gdk.ModifierType.None) ==
            Gdk.ModifierType.None)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Left:
                    ChangeToNextImage(-1);
                    break;
                case Gdk.Key.Right:
                    ChangeToNextImage(1);
                    break;
                case Gdk.Key.Delete:
                    DeleteCurrentImage();
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
