using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class WorkingDirModel
{
    
    private DirectoryInfo dir;
    private int photoIndex;
    private List<FileSystemInfo> photos;

    public DirectoryInfo Directory { get { return dir; } }

    public event EventHandler<DirectoryChangedEventArgs> DirectoryChangedEvent;

    public void ChangeDirectory(DirectoryInfo newDir)
    {
        dir = newDir;
        photos = Finder.FindImages(newDir).ToList();
        this.GoFirstPhoto();

        if (DirectoryChangedEvent != null) {
            DirectoryChangedEventArgs args = new DirectoryChangedEventArgs();
            args.NewDirectory = newDir;
            this.DirectoryChangedEvent(this, args);
        } 
    }

    public void GoFirstPhoto()
    {
        photoIndex = 0;
    }
    
    public void IncrementPhoto(int i)
    {
        photoIndex = (photoIndex + photos.Count + i) % photos.Count;
    }

    public string CurrentPhoto {
        get {
            // TODO check it exists
            return photos[photoIndex].FullName;
        }
    }
}

public class DirectoryChangedEventArgs : System.EventArgs
{
    public DirectoryInfo NewDirectory { get; set; }
}
