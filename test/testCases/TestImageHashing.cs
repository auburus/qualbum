using System;
using ImageHashing;

namespace Qualbum
{
    class TestImageHashing : Test
    {
        private String path;

        public override void Setup()
        {
            path = Test.BaseFolder.FullName + "/fixtures/imageHashing/";
        }

        public void TestExact()
        {

            String path1 = path + "original.jpg";
            String path2 = path + "exact_copy.jpg";

            AssertEqual(ImageHashing.ImageHashing.Similarity(path1, path2), 100);

            String path3 = path + "exact_copy.png";

            AssertEqual(ImageHashing.ImageHashing.Similarity(path1, path3), 100);
        }

        public void TestSimilar()
        {
            String path1 = path + "original.jpg";
            String path2 = path + "similar.jpg";

            AssertGreater(ImageHashing.ImageHashing.Similarity(path1, path2), 70);

            String path3 = path + "similar.png";

            AssertGreater(ImageHashing.ImageHashing.Similarity(path1, path3), 70);
        }
    }
}
