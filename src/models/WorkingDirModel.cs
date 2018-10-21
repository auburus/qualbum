using System;
using System.IO;

namespace Qualbum {

    public class WorkingDirModel
    {
        public event EventHandler<DirectoryChangedEventArgs> DirectoryChangedEvent;

        public void ChangeDirectory(DirectoryInfo newDir)
        {
            if (DirectoryChangedEvent != null) {
                DirectoryChangedEventArgs args = new DirectoryChangedEventArgs();
                args.NewDirectory = newDir;
                this.DirectoryChangedEvent(this, args);
            } 
        }
    }

    public class DirectoryChangedEventArgs : System.EventArgs
    {
        public DirectoryInfo NewDirectory { get; set; }
    }
}
