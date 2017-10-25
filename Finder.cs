using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Finder
{
    /**
     * Find all images in the current directory subtree.
     * It doesn't find images in hidden directories, nor hidden images,
     * and it does that on purpose.
     *
     * The reason why is because if it's hidden, we don't want to "see" it.
     */
    public static IEnumerable<FileSystemInfo> FindImages(DirectoryInfo directory)
    {
        return directory.EnumerateDirectories("*", SearchOption.AllDirectories)
            .Where( d => (d.Attributes & FileAttributes.Hidden) == 0)
            .Select( d => d.EnumerateFiles())
            .Aggregate(
                Enumerable.Empty<FileSystemInfo>(),
                (list, newlist) => list.Concat(newlist)
            )
            .Where( f => (f.Attributes & FileAttributes.Hidden) == 0)
            .Where( f => Regex.IsMatch(f.Extension, @"\.[jpg|jpeg|png|gif]"));
    }
}
