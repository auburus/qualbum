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
                    c.GetMethod("Setup").Invoke(obj, null);
                    m.Invoke(obj, null);
                    c.GetMethod("Teardown").Invoke(obj, null);
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

    public virtual void Setup() {}
    public virtual void Teardown() {}

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

    public void AssertEqual(String str1, String str2) {
        if (!str1.Equals(str2)) {
            throw new AssertException("Failed asserting that \"" +
                    str1 + "\" is equal to \"" + str2 + "\"");
        }
    }

    public void AssertEqual(object obj1, object obj2) {
        if (obj1 is string && obj2 is string) {
            AssertEqual(obj1.ToString(), obj2.ToString());
        }
        else if (obj1 != obj2) {
            throw new AssertException("Failed asserting that \"" +
                    obj1.ToString() + "\" is equal to \"" + obj2.ToString() + "\"");
        }
    }

    public void AssertEnumerableEqual(IEnumerable<object> list1,
            IEnumerable<object> list2)
    {
        Assert(list1.Count()==list2.Count(), "Failed asserting that list1 ("
                + list1.Count() + " items) equals list2 (" +
                list2.Count() + " items)\n" +
                "List 1: [" + String.Join(",", list1.ToArray()) + "]\n" +
                "List 2: [" + String.Join(",", list2.ToArray()) + "]\n");

        var enumerator = list2.GetEnumerator();

        foreach (var x in list1) {
            enumerator.MoveNext();
            var y = enumerator.Current;

            AssertEqual(x, y);
        }
    }

    class AssertException : Exception {
        public AssertException() : base() { }
        public AssertException(String message) : base(message) { }
    }
}

