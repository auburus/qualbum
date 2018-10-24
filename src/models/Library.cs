using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;
using System.Drawing;

namespace Qualbum {

    /// <summary>
    /// A library is the base folder where all photos end up organized
    /// </sumamry>
    public class Library
    {
        private DirectoryInfo baseFolder;

        public Library(DirectoryInfo dir)
        {
            if (!dir.Exists) {
                throw new Exception(dir.FullName + " doesn't exist!");
            }
            this.baseFolder = dir;
            this.Init();
        }

        public Library(String path) : this(new DirectoryInfo(path)) 
        {
        }

        public DirectoryInfo BaseFolder { get { return this.baseFolder; } }

        public DirectoryInfo QualbumFolder {
            get {
                return this.baseFolder.GetDirectories(".qualbum")[0];
            }
        }

        public DirectoryInfo DeletedFolder {
            get {
                return this.QualbumFolder.GetDirectories("deleted")[0];
            }
        }

        public String ConnectionString {
            get {
                return String.Format("Data Source={0}",
                    Path.Combine(this.QualbumFolder.FullName, "db.sqlite")
                );
            }
        }

        /// Gets all the subdirectories in the tree given by the current directory
        /// It dynamically computes them so if a new one is added, it will get
        /// catched up.
        public IEnumerable<DirectoryInfo> Subdirectories {
            get {
                return this.subdirectories(this.baseFolder);
            }
        }

        /// Makes sure to ignore all the tree under a hidden folder
        private IEnumerable<DirectoryInfo> subdirectories(DirectoryInfo dir)
        {
            return dir.EnumerateDirectories("*")
                .Where( d => (!d.Attributes.HasFlag(FileAttributes.Hidden)))
                .Select( d => this.subdirectories(d))
                .Aggregate(
                    Enumerable.Empty<DirectoryInfo>(),
                    (list, newlist) => list.Concat(newlist)
                )
                .Prepend(dir);
        }

        /// Finds all the subdirectories that match with the partialName string
        public IEnumerable<DirectoryInfo> SubdirectoriesMatch(String partialName)
        {
            foreach (DirectoryInfo dir in this.Subdirectories) {
                if (dir.Name.ToLower().Contains(partialName)) {
                    yield return dir;
                }
            }
        }

        public PhotoCollection PhotoCollection
        {
            get {
                return PhotoCollection.CreateFromDirectory(this.baseFolder);
            }
        }


        /// Prepare all the things for this folder to be a library
        public void Init()
        {
            // TODO Complete initialization
            checkOrCreateQualbumFolder();
        }

        public void ComputeHashes()
        {
            Console.WriteLine("Computing hashes");
            foreach(FileInfo file in this.PhotoCollection)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Console.Write(Finder.FindRelativePath(this.baseFolder, file));
                Console.Write(": ");
                Console.WriteLine(hash(file));
            }
        }

        // TODO change to a collection
        public IEnumerable<FileInfo> FindInvalidPhotos()
        {
            return this.PhotoCollection.Where( x => !this.IsPhotoValid(x));
        }

        /// Attempt to load it, and if it fails, is not valid
        private bool IsPhotoValid(FileInfo photo)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            try {
                Bitmap bmp = new Bitmap(photo.FullName);
                return true;
            } catch (ArgumentException) {
                return false;
            }
        }

        private void checkOrCreateQualbumFolder()
        {
            DirectoryInfo qualbumFolder = new DirectoryInfo(
                Path.Combine(this.BaseFolder.FullName, ".qualbum")
            );
            qualbumFolder.Create(); // It does nothing if it alredy exists

            if ( ! qualbumFolder.Attributes.HasFlag(FileAttributes.Hidden)) {
                qualbumFolder.Attributes |= FileAttributes.Hidden;
            }

            try {
                qualbumFolder.CreateSubdirectory("deleted");
            } catch (IOException) {
                // Do nothing, because the directory already exists
                // There is the possibility that it can't be created for other reasons
                // (i.e. hard disk full), so TODO check on that
            }
        }

        private ulong hash(FileInfo file)
        {
            return ImageHashing.ImageHashing.AverageHash(file.FullName);
        }
    }
}
