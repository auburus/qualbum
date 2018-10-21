using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Qualbum {

    /// Manages the current state of a photo collection
    class PhotoCollection
    {
        private List<FileInfo> photos;
        private int index;


        public PhotoCollection(IEnumerable<FileInfo> photos)
        {
            this.photos = photos.ToList();
            index = 0;
        }

        public PhotoCollection() : this(Enumerable.Empty<FileInfo>()) {}

        public int Count
        {
            get {
                return photos.Count;
            }
        }

        public FileInfo Current {
            get {
                if (index < 0 || index >= photos.Count)
                {
                    return null;
                }
                return photos[index];
            }
        }

        public int CurrentIndex
        {
            get {
                return index;
            }
        }

        public FileInfo First()
        {
            index = 0;
            return this.Current;
        }

        public FileInfo Next()
        {
            index = (index + 1) % this.Count;
            return this.Current;
        }

        public FileInfo Prev()
        {
            index = (index + (this.Count - 1)) % this.Count;
            return this.Current;
        }

        public void Insert(FileInfo f)
        {
            photos.Insert(index, f);
        }

        public void Remove()
        {
            photos.RemoveAt(index);

            // Update the index just in case is the last photo
            index = (index + this.Count) % this.Count;
        }
    }
}
