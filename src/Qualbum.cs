using Gtk;
using System;
using System.IO;
using Gdk;
using System.Linq;
using System.Collections.Generic; // Remove when remove dict

namespace Qualbum {

    public class Qualbum : Gtk.Window
    {
        private ApplicationController appController;

        public Qualbum() : base("Qualbum")
        {
            SetDefaultSize(800, 600);
            SetPosition(WindowPosition.Center);
            SetIconFromFile("icon.png");


            appController = new ApplicationController(this);

            DeleteEvent += new DeleteEventHandler(OnDelete);
            KeyPressEvent += new KeyPressEventHandler(appController.OnKeyPress);

            Add(appController.Widget);
            Maximize();

            ShowAll();
        }

        public static void Run()
        {
            Application.Init();
            new Qualbum();
            Application.Run();
        }

        public static DirectoryInfo BaseFolder {
            get {
                return new FileInfo(
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                    ).Directory;
            }
        }

        public static DirectoryInfo ConfigFolder {
            get {
                return Qualbum.BaseFolder.GetDirectories("config")[0];
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

    //TODO Move that to a proper file
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
}
