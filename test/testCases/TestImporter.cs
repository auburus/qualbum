using Mono.Data.Sqlite;
using System;
using System.IO;

namespace Qualbum
{
    public class TestImporter : Test
    {
        Library library;
        Importer importer;


        public override void Setup()
        {
            library = new Library(
                Qualbum.BaseFolder.GetDirectories("test")[0].GetDirectories("library")[0]
            );
            importer = new Importer(this.library);

            importer.RemoveAllDeleted();
        }

        public void TestDeleteAndRestore()
        {
            FileInfo file = new FileInfo(
                    Path.Combine(library.BaseFolder.FullName, "test.img"));

            using ( var f = file.Create())
            {
                Assert(file.Exists, "File should exist");
            }

            importer.Delete(file);

            file = new FileInfo(Path.Combine(library.BaseFolder.FullName, "test.img"));
            Assert(!file.Exists, "File still exists and it should not");


            FileInfo file2 = importer.RestoreLast();

            Assert(file2 != null, "Restore should return a valid FileInfo");

            file = new FileInfo(Path.Combine(library.BaseFolder.FullName, "test.img"));
            Assert(file.Exists, "File should be back here");
            AssertEqual(file.FullName, file2.FullName);
        }


        public void TestRestoreWithoutDelete()
        {
            // Make sure it doesn't crash
            FileInfo file = importer.RestoreLast();

            Assert(file == null, "File should be null");
        }
    }
}
