using Gtk;

class SharpApp : Window
{
    public SharpApp() : base("Buttons")
    {
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);
        SetIconFromFile("icon.png");

        DeleteEvent += new DeleteEventHandler(OnDelete);

        Fixed fix = new Fixed();

        Button btn1 = new Button("Button 1");
        btn1.Sensitive = false;
        Button btn2 = new Button("Button 2");
        Button btn3 = new Button(Stock.Close);
        Button btn4 = new Button("Button 4");
        btn4.SetSizeRequest(80, 40);

        fix.Put(btn1, 20, 30);
        fix.Put(btn2, 100, 30);
        fix.Put(btn3, 20, 80);
        fix.Put(btn4, 100, 80);

        Add(fix);

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
}
