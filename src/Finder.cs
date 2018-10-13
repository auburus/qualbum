using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

static class Finder
{
    /**
     * Find all images in the current directory subtree.
     * It doesn't find images in hidden directories, nor hidden images,
     * and it does that on purpose.
     *
     * The reason why is because if it's hidden, we don't want to "see" it.
     */
    public static IEnumerable<FileInfo> FindImages(DirectoryInfo directory)
    {
        return directory.EnumerateDirectories("*", SearchOption.AllDirectories)
            .Prepend(directory)
            .Where( d => (d.Attributes & FileAttributes.Hidden) == 0)
            .Select( d => d.EnumerateFiles())
            .Aggregate(
                Enumerable.Empty<FileInfo>(),
                (list, newlist) => list.Concat(newlist)
            )
            .Where( f => (f.Attributes & FileAttributes.Hidden) == 0)
            .Where( f => Regex.IsMatch(
                        f.Extension,
                        @"\.[jpg|jpeg|png]",
                        RegexOptions.IgnoreCase
            ))
            .OrderBy( f => GuessOrder(f) );
    }


    public static String GuessOrder(FileInfo f)
    {
        DateTime guessedDate = GuessDate(f);
        if (guessedDate == f.CreationTime) {
            return f.Name;
        }

        return guessedDate.ToString("ddMMyyyy");
    }

    /**
      * Tries to guess the date from the file name. If not, it defaults
      * to the creationTime
      */
    public static DateTime GuessDate(FileInfo f)
    {
        Regex rx = new Regex(@"[^\d]*(\d{8})[^\d]{1}.*",
                RegexOptions.Compiled);

        MatchCollection matches = rx.Matches(f.Name);

        if (matches.Count == 1 ) {
            String date = matches[0].Groups[1].Value;
            return new DateTime(Int32.Parse(date.Substring(0,4)),
                    Int32.Parse(date.Substring(4,2)),
                    Int32.Parse(date.Substring(6,2)));
        }

        return f.CreationTime;
    }
}
