using System;
using System.IO;

namespace Qualbum
{

    public class TestFinder : Test
    {
        public void TestFindRelativePath()
        {
            FileInfo f = new FileInfo(@"/home/hello/how/are/you\ doing/today/question.txt");
            DirectoryInfo d = new DirectoryInfo(@"/home/hello/how");

            String relPath = Finder.FindRelativePath(d, f);

            AssertEqual(relPath, @"are/you\ doing/today/question.txt");


        }

        public void TestUnvalidRelPath()
        {
            FileInfo f = new FileInfo(@"/home/hello/how/are/you\ doing/today/question.txt");
            DirectoryInfo d = new DirectoryInfo(@"/home/hello/this/is/amazing");

            String relPath = Finder.FindRelativePath(d, f);

            AssertEqual(relPath, "");
        }


    }
}
