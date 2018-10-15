using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


/**
  * A library is the base folder where all photos end up organized
  */
public class LibraryModel
{
    private DirectoryInfo baseFolder;

    public LibraryModel(DirectoryInfo dir)
    {
        this.baseFolder = dir;
    }

    public LibraryModel(String path) : this(new DirectoryInfo(path)) 
    {
    }

    public DirectoryInfo BaseFolder { get { return this.baseFolder; } }

    public IEnumerable<DirectoryInfo> Subdirectories {
        get {
            return this.baseFolder
                .EnumerateDirectories("*", SearchOption.AllDirectories)
                .Prepend(this.baseFolder)
                .Where ( d => (d.Attributes & FileAttributes.Hidden) == 0);
        }
    }

    public IEnumerable<DirectoryInfo> FindDirectories(String partialName)
    {
        foreach (DirectoryInfo dir in this.Subdirectories) {
            if (dir.Name.ToLower().Contains(partialName)) {
                yield return dir;
            }
        }
    }
}
