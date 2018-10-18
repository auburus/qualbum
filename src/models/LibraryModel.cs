using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A library is the base folder where all photos end up organized
/// </sumamry>
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

    /// Gets all the subdirectories in the tree given by the current directory
    /// It dynamically computes them so if a new one is added, it will get
    /// catched up.
    public IEnumerable<DirectoryInfo> Subdirectories {
        get {
            return this.baseFolder
                .EnumerateDirectories("*", SearchOption.AllDirectories)
                .Prepend(this.baseFolder)
                .Where ( d => (d.Attributes & FileAttributes.Hidden) == 0);
        }
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
}
