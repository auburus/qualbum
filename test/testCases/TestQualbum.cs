using System;
using System.IO;


namespace Qualbum
{

    public class TestQualbum : Test
    {

        public void TestConfigFolder()
        {
            AssertEqual(Qualbum.ConfigFolder.FullName,
                    Qualbum.BaseFolder.GetDirectories("config")[0].FullName);
        }
    }
}
