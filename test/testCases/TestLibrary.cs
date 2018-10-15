using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class TestLibrary : Test
{
    public void TestConstructor()
    {
        LibraryModel lib = new LibraryModel("./test/library");
        Assert(lib.BaseFolder.Name == "library");
    }

    public void TestSubdirectories()
    {
        LibraryModel lib = new LibraryModel("./test/library");

        String[] correctDirs = new String[] {"library", "2017", "2018",
            "2017-08-12 Anna a Colorado",
            "2017-09-11 Festa catalans", "2018-03-15 Herman Gulch"};


        AssertEnumerableEqual(lib.Subdirectories.Select( x => x.Name),
                correctDirs);
    }

    public void TestFindDirectories()
    {
        LibraryModel lib = new LibraryModel("./test/library");

        String[] correctDirs = new String[] { "2017-08-12 Anna a Colorado",
            "2017-09-11 Festa catalans", "2018-03-15 Herman Gulch"};
        AssertEnumerableEqual(lib.FindDirectories("an").Select(x => x.Name),
                correctDirs);
    }
}
