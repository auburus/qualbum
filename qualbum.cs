using Gtk;

class SharpApp : Window {
    public SharpApp() : base("Center")
    {
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);

        DeleteEvent += delegate {Application.Quit(); };

        Show();
    }

    public static void Main()
    {
        Application.Init();
        new SharpApp();
        Application.Run();
    }
}
