using System;
using Gtk;
using System.IO;

namespace Qualbum {

    public class PhotoPresenter
    {
        private PhotoView view;
        private PhotoCollection collection;
        private Importer importer;

        public PhotoPresenter(Importer importer, PhotoCollection collection)
        {
            this.importer = importer;
            this.collection = collection;
            this.view = new PhotoView();
        }

        public PhotoPresenter(Importer importer) :
            this(importer, new PhotoCollection()) {}

        public void FirstPhoto()
        {
            Display(collection.First());
        }

        public void NextPhoto()
        {
            Display(collection.Next());
        }

        public void PrevPhoto()
        {
            Display(collection.Prev());
        }

        // Doesn't remove them from disk now, it used to
        public void DeletePhoto()
        {
            importer.Delete(collection.Current);
            collection.Remove();
            Display(collection.Current);
        }

        public void RestoreLastPhoto()
        {
            FileInfo photoFile = importer.RestoreLast();

            if (photoFile != null)
            {
                collection.Insert(photoFile);
                Display(collection.Current);
            } else {
                // TODO Alert there are no photos to restore
            }
        }


        // Proxy function until we finish refactoring
        public void Display(FileInfo photoFile)
        {
            if (photoFile == null) {
                this.view.Display(this.defaultPhoto);
                return;
            } 

            try {
                this.view.Display(photoFile);
            } catch (GLib.GException) {
                this.view.Display(this.defaultPhoto);
            }

            this.view.FillLabels(photoFile);
            this.view.UpdateCounterLabel(collection.CurrentIndex, collection.Count);
        }

        public void Rotate(Gdk.PixbufRotation rotation)
        {
            this.view.Rotate(rotation);

            // Rotate the original picture and save it
            Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(collection.Current.FullName);
            pixbuf = pixbuf.RotateSimple(rotation);

            // All images provided by finder are jpgs or pngs
            String extension = collection.Current.Extension
                .Substring(1).ToLower();

            if (extension == "jpg") {
                extension = "jpeg";
            }
            pixbuf.Save(collection.Current.FullName, extension);
        }

        public Widget Widget { get { return this.view.AsWidget; } }

        private FileInfo defaultPhoto {
            get {
                return Qualbum.ConfigFolder.GetFiles("default.jpg")[0];
            }
        }

        public void ChangeCollection(PhotoCollection collection, DirectoryInfo dir)
        {
            this.collection = collection;
            this.view.ActiveDirectoryLabel.Text = dir.FullName;
            this.FirstPhoto();
        }

    }
}
