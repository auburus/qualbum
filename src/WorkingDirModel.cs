using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class WorkingDirModel
{
    
    private DirectoryInfo dir;
    private DirectoryInfo deletedDir;
    private int photoIndex;
    private List<FileSystemInfo> photos;

    public DirectoryInfo Directory { get { return dir; } }

    public event EventHandler<DirectoryChangedEventArgs> DirectoryChangedEvent;

    public void ChangeDirectory(DirectoryInfo newDir)
    {
        dir = newDir;
        photos = Finder.FindImages(newDir).ToList();
        this.GoFirstPhoto();

        createDeletedFolder();

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

    public FileSystemInfo CurrentPhoto {
        get {
            // TODO check it exists
            return photos[photoIndex];
        }
    }

    public void DeleteCurrentPhoto() {
        Console.WriteLine("Deleting photo " + photoIndex);

        FileInfo current = new FileInfo(CurrentPhoto.FullName);
        current.MoveTo(Path.Combine(deletedDir.FullName, CurrentPhoto.Name));

        photos.RemoveAt(photoIndex);
        IncrementPhoto(0);
    }

    private void createDeletedFolder() {
        DirectoryInfo[] dirs = dir.GetDirectories(".deleted");

        // Check if directory exists
        if (dirs.Length == 1) {
            deletedDir = dirs[0];
        } else {
            deletedDir = this.dir.CreateSubdirectory(".deleted");
        }

        deletedDir.Attributes = deletedDir.Attributes | FileAttributes.Hidden;
    }
}

public class DirectoryChangedEventArgs : System.EventArgs
{
    public DirectoryInfo NewDirectory { get; set; }
}
