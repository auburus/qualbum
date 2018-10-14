using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

/**
  For some reason, I decided not to use a framework
  (yet). Lets see how this goes
  */
public class Test
{
    public static void Main()
    {

        IEnumerable<Type> classes = Assembly.GetExecutingAssembly().GetTypes()
            .Where( t => t.IsClass );

        foreach (Type c in classes) {
            if (c.Name.Length >= 4 && c.Name.Substring(0,4) == "Test") {
                foreach (MethodInfo m in
                        c.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    if (m.Name.Length >= 4 && m.Name.Substring(0,4) == "Test") {
                        try {
                            m.Invoke(null, null);
                        } catch (TargetInvocationException e) {
                            Console.WriteLine("Assertion failed in <" + c.Name +
                                "." + m.Name + "()>: " + e.InnerException.Message);
                        }
                    }
                }
            }
        }
        //TestLibrary.TestConstructor();
    }

    public static void Assert(bool condition)
    {
        if (!condition) {
            throw new AssertException("Asserting that false was true");
        }
    }
    public static void Assert(bool condition, String message)
    {
        if (!condition) {
            throw new AssertException(message);
        }
    }
}

class AssertException : Exception {
    public AssertException() : base() { }
    public AssertException(String message) : base(message) { }
}

class TestLibrary : Test
{
    public static void TestConstructor()
    {
        LibraryModel lib = new LibraryModel("./test/library");
        Assert(lib.BaseFolder.Name == "library");
    }
}
