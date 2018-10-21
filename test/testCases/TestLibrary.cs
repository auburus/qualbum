using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Qualbum
{
    public class TestLibrary : Test
    {
        public void TestConstructor()
        {
            Library lib = new Library("./test/library");
            Assert(lib.BaseFolder.Name == "library");
        }

        public void TestSubdirectories()
        {
            Library lib = new Library("./test/library");

            String[] correctDirs = new String[] {"library",
                "2017",
                "2017-08-12 Anna a Colorado",
                "2017-09-11 Festa catalans",
                "2018",
                "2018-03-15 Herman Gulch"};


            AssertEnumerableEqual(lib.Subdirectories.Select( x => x.Name),
                    correctDirs);
        }

        public void TestFindDirectories()
        {
            Library lib = new Library("./test/library");

            String[] correctDirs = new String[] { "2017-08-12 Anna a Colorado",
                "2017-09-11 Festa catalans", "2018-03-15 Herman Gulch"};

            AssertEnumerableEqual(lib.SubdirectoriesMatch("an").Select(x => x.Name),
                    correctDirs);


            correctDirs = new String[] { "2018", "2018-03-15 Herman Gulch" };

            AssertEnumerableEqual(lib.SubdirectoriesMatch("18").Select(x => x.Name),
                    correctDirs);
        }
    }
}
