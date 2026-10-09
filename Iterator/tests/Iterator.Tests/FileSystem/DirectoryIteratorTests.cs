namespace dev.kaldiroglu.Iterator.Tests.FileSystem;

// The usings sit inside the namespace, so that Directory and File mean the file system
// example's classes and not System.IO.Directory and System.IO.File.
using System.Reflection;
using dev.kaldiroglu.Iterator.FileSystem;
using Xunit;

/// <summary>
/// The file system example. The output the Part 3 notes quote from <c>FileSystem.Test</c> is
/// asserted here. Ported from the Java <c>fileSystem.DirectoryIteratorTest</c>.
/// </summary>
public class DirectoryIteratorTests
{
    private static List<string> NamesIn(Directory directory)
    {
        var names = new List<string>();
        DirectoryIterator<IStorage> iterator = directory.GetEnumerator();
        while (iterator.MoveNext())
        {
            names.Add(iterator.Current.ToString()!);
        }
        return names;
    }

    [Fact(DisplayName = "fileSystem.Test prints the Dev directory's four elements, then the two files in Reports")]
    public void TheClientPrintsWhatTheNotesQuote()
    {
        var lines = Printed.By(Test.Run);

        var nonBlank = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
        Assert.Equal([
            "List of the directory: /Users/akin",
            "Iterating",
            "Readme.txt", "Report.docs", "Selam.java", "Reports",
            "Iterating",
            "ImportantReport.docs", "SelamTest.java"], nonBlank);
    }

    [Fact(DisplayName = "the iterator lists only the directory's own elements: a folder inside it is one element")]
    public void AFolderIsOneElement()
    {
        var dev = new Directory("Dev");
        new File("Readme.txt", dev);
        var reports = new Directory("Reports", dev);
        new File("ImportantReport.docs", reports);
        new File("SelamTest.java", reports);

        Assert.Equal(["Readme.txt", "Reports"], NamesIn(dev));
        Assert.Equal(["ImportantReport.docs", "SelamTest.java"], NamesIn(reports));
    }

    [Fact(DisplayName = "only the iterator can reach the list: elements() is package-private and cannot be changed")]
    public void TheListIsHiddenAndUnchangeable()
    {
        // Java's elements() is package-private. C# has no package access; the port makes
        // Elements internal, which is the closest choice. Its getter must be neither public,
        // nor protected, nor private.
        var elements = typeof(Directory).GetProperty("Elements", BindingFlags.Instance | BindingFlags.NonPublic)
                       ?? throw new InvalidOperationException("Directory has no Elements property");
        var getter = elements.GetGetMethod(nonPublic: true)!;
        Assert.True(getter.IsAssembly, "internal");
        Assert.False(getter.IsPublic);
        Assert.False(getter.IsFamily || getter.IsFamilyOrAssembly || getter.IsFamilyAndAssembly);
        Assert.False(getter.IsPrivate);

        var dev = new Directory("Dev");
        new File("Readme.txt", dev);
        // The test project cannot call an internal member directly, so it reads the list
        // through reflection. Java's unmodifiable list throws UnsupportedOperationException;
        // a .NET read-only collection throws NotSupportedException.
        var list = (ICollection<IStorage>)elements.GetValue(dev)!;
        Assert.Throws<NotSupportedException>(() => list.Add(new File("Extra.txt", null!)));
    }

    [Fact(DisplayName = "DirectoryIterator is in the same package as Directory")]
    public void TheIteratorSitsBesideTheDirectory()
    {
        // A Java package is a C# namespace here.
        Assert.Equal(typeof(Directory).Namespace, typeof(DirectoryIterator<>).Namespace);
    }
}
