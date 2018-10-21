using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;

/// <summary>
/// A library is the base folder where all photos end up organized
/// </sumamry>
public class LibraryModel
{
    private DirectoryInfo baseFolder;

    public LibraryModel(DirectoryInfo dir)
    {
        if (!dir.Exists) {
            throw new Exception(dir.FullName + " doesn't exist!");
        }
        this.baseFolder = dir;
        this.checkOrCreateQualbumFolder();

    }

    public LibraryModel(String path) : this(new DirectoryInfo(path)) 
    {
    }

    public DirectoryInfo BaseFolder { get { return this.baseFolder; } }

    public DirectoryInfo QualbumFolder {
        get {
            return this.baseFolder.GetDirectories(".qualbum")[0];
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

    private void checkOrCreateQualbumFolder()
    {
        DirectoryInfo qualbumFolder = new DirectoryInfo(
            Path.Combine(this.BaseFolder.FullName, ".qualbum")
        );
        qualbumFolder.Create(); // It does nothing if it alredy exists

        if ( ! qualbumFolder.Attributes.HasFlag(FileAttributes.Hidden)) {
            qualbumFolder.Attributes |= FileAttributes.Hidden;
        }
    }
}
