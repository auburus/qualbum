using System;
using System.IO;
using System.Collections.Generic;


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

    /*public IEnumerable<DirectoryInfo> Subfolders {
        get {
            return this.baseFolder
                .EnumerateDirectories("*", SearchOption.AllDirectories)
                .Prepend(directory)
                .Where ( d => (d.Attributes & FileAttributes.Hidden) == 0);
        }
    }*/

    public DirectoryInfo BaseFolder { get { return this.baseFolder; } }
}
