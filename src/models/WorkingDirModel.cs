using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class WorkingDirModel
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

    public int CurrentPhotoIndex { get { return this.photoIndex; } }

    public int TotalNumberPhotos { get { return this.photos.Count; } }

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

    public List<Color> GetHashFromCurrent()
    {
        return this.GetHash(this.CurrentPhotoPath);
    }

    /**
      * It firsts resizes the image for performance
      */
    public List<Color> GetHash(string path) {
        Image image = Image.FromFile(path);
        Bitmap currentImage = ResizeImage(image, 6, 6);

        List<Color> hash = new List<Color>();

        for (int x = 0; x < currentImage.Width; x++) {
            for (int y = 0; y < currentImage.Height; y++) {
                hash.Add(currentImage.GetPixel(x, y));
            }
        }

        return hash;
    }

    public static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        //destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }

    /**
      * Both list MUST have the same length
      */
    public int HashDifference(List<Color> hash1, List<Color> hash2)
    {
        int diff = 0;

        for (int i = 0; i < Math.Min(hash1.Count, hash2.Count); i++)
        {
            diff += Math.Abs(hash1[i].R - hash2[i].R);
            diff += Math.Abs(hash1[i].G - hash2[i].G);
            diff += Math.Abs(hash1[i].B - hash2[i].B);
        }

        return diff;
    }
}

public class DirectoryChangedEventArgs : System.EventArgs
{
    public DirectoryInfo NewDirectory { get; set; }
}
