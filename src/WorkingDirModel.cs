using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

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

    public String CurrentPhotoPath {
        get {
            if (photoIndex < 0 || photoIndex >= photos.Count) {
                return null;
            }
            return photos[photoIndex].FullName;
        }
    }

    public void DeleteCurrentPhoto() {
        FileInfo current = new FileInfo(this.CurrentPhotoPath);
        current.MoveTo(Path.Combine(deletedDir.FullName, current.Name));

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

    public Dictionary<Color, float> GetHistogramFromCurrent()
    {
        return this.GetHistogram(this.CurrentPhotoPath);
    }

    public Dictionary<Color, float> GetHistogram(string path) {
        Bitmap currentImage = new Bitmap(path, false);
        Dictionary<Color, float> histogram = new Dictionary<Color, float>();

        for (int x = 0; x < currentImage.Width; x++) {
            for (int y = 0; y < currentImage.Height; y++) {
                Color color = currentImage.GetPixel(x, y);

                float count = 0;
                if (histogram.TryGetValue(color, out count)) {
                    histogram[color] = count + 1;
                } else {
                    histogram[color] = 1;
                }
            }
        }

        Dictionary<Color, float> hist = new Dictionary<Color, float>();
        //float max = histogram.Values.Max();
        float max = currentImage.Width * currentImage.Height;

        foreach (Color key in histogram.Keys)
        {
            hist[key] = histogram[key] / max;
        }


        return hist;
    }

    public float HistogramDifference(Dictionary<Color, float> histogram1, Dictionary<Color, float> histogram2)
    {
        Dictionary<Color, float> hist1 = new Dictionary<Color, float>(histogram1);
        Dictionary<Color, float> hist2 = new Dictionary<Color, float>(histogram2);

        float diff = 0;
        float count2;

        foreach (Color key in hist1.Keys) {
            if (hist2.TryGetValue(key, out count2)) {
                diff += Math.Abs(hist1[key] - count2);
                hist2[key] = 0;
            } else {
                diff += hist1[key];
            }
        }

        foreach (Color key in hist2.Keys) {
            diff += hist2[key]; // We zeroed all the previously added ones
        }

        return diff;
    }
}

public class DirectoryChangedEventArgs : System.EventArgs
{
    public DirectoryInfo NewDirectory { get; set; }
}
