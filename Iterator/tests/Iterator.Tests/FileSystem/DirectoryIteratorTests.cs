namespace dev.kaldiroglu.Iterator.Tests.FileSystem;

// The usings sit inside the namespace, so that Directory and File mean the file system
// example's classes and not System.IO.Directory and System.IO.File.
using System.Reflection;
using dev.kaldiroglu.Iterator.FileSystem;
using Xunit;

/// <summary>
/// The file system example. The output the Part 3 notes quote from <c>FileSystem.Main</c> is
/// asserted here.
/// </summary>
public class DirectoryIteratorTests
{
    /// <summary>Move takes the element out of its directory, sets the parent and adds it to the target.</summary>
    [Fact]
    public void MoveChangesTheParent()
    {
        var dev = new Directory("Dev");
        var reports = new Directory("Reports");
        var file = new File("Readme.txt", dev);
        var archive = new Directory("Archive", dev);

        file.Move(reports);
        archive.Move(reports);

        Assert.Empty(NamesIn(dev));
        Assert.Equal(new[] { "Readme.txt", "Archive" }, NamesIn(reports));
        Assert.Same(reports, file.Parent);
        Assert.Same(reports, archive.Parent);
    }

    /// <summary>Copying a file gives a new file in no directory.</summary>
    [Fact]
    public void CopyAFile()
    {
        var dev = new Directory("Dev");
        var file = new File("Readme.txt", dev);

        var copy = (File)file.Copy();

        Assert.NotSame(file, copy);
        Assert.Equal("Readme.txt", copy.Name);
        Assert.Null(copy.Parent);
        Assert.Equal(new[] { "Readme.txt" }, NamesIn(dev));
    }

    /// <summary>Copying a directory copies its elements, so a change to the copy does not reach the original.</summary>
    [Fact]
    public void CopyADirectory()
    {
        var dev = new Directory("Dev");
        new File("Readme.txt", dev);
        var reports = new Directory("Reports", dev);
        new File("ImportantReport.docs", reports);

        var copy = (Directory)dev.Copy();
        new File("Extra.txt", copy);

        Assert.Equal(new[] { "Readme.txt", "Reports", "Extra.txt" }, NamesIn(copy));
        Assert.Equal(new[] { "Readme.txt", "Reports" }, NamesIn(dev));
        var iterator = copy.GetEnumerator();
        iterator.MoveNext();
        Assert.Same(copy, ((File)iterator.Current).Parent);
    }

    /// <summary>Deleting a directory with no parent changes nothing and does not fail.</summary>
    [Fact]
    public void DeleteARoot()
    {
        new Directory("Root").Delete();
    }

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
        var lines = Printed.By(Main.Run);

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
        // Elements is internal. Its getter must be neither public,
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
        // through reflection. A read-only collection throws NotSupportedException.
        var list = (ICollection<IStorage>)elements.GetValue(dev)!;
        Assert.Throws<NotSupportedException>(() => list.Add(new File("Extra.txt", null!)));
    }

    [Fact(DisplayName = "DirectoryIterator is in the same package as Directory")]
    public void TheIteratorSitsBesideTheDirectory()
    {
        // The iterator is in the directory's namespace.
        Assert.Equal(typeof(Directory).Namespace, typeof(DirectoryIterator<>).Namespace);
    }
}
