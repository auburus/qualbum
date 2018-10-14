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
        int totalTests = 0;
        int passedTests = 0;

        IEnumerable<Type> classes = Assembly.GetExecutingAssembly().GetTypes()
            .Where( t =>
                t.IsClass &&
                t.Name.Length >= 4 &&
                t.Name.Substring(0,4) == "Test");

        foreach (Type c in classes) {
            IEnumerable<MethodInfo> methods =
                c.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where( m =>
                    m.Name.Length >= 4 &&
                    m.Name.Substring(0,4) == "Test");

            foreach (MethodInfo m in methods) {
                object obj = Activator.CreateInstance(c);

                try {
                    totalTests++;
                    m.Invoke(obj, null);
                    passedTests++;
                } catch (TargetInvocationException e) {
                    Console.WriteLine("Assertion failed in <" + c.Name +
                        "." + m.Name + "()>: " + e.InnerException.Message);
                }
            }
        }

        Console.WriteLine("\n\nResult: " + passedTests.ToString() + " of " +
            totalTests.ToString() + " passed");
    }

    public void Assert(bool condition)
    {
        if (!condition) {
            throw new AssertException("Asserting that false was true");
        }
    }
    public void Assert(bool condition, String message)
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
    public void TestConstructor()
    {
        LibraryModel lib = new LibraryModel("./test/library");
        Assert(lib.BaseFolder.Name == "library");
    }
}
