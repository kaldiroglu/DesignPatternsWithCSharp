namespace dev.kaldiroglu.Visitor.Hw.FileTree;

/// <summary>
/// Two visitors over one folder tree. The folders do the walking, so neither visitor
/// contains a loop over the children.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- hw-filetree</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var root = new Folder("project",
                new FileEntry("README.md", 2),
                new Folder("src",
                        new FileEntry("Main.java", 4),
                        new FileEntry("Order.java", 6)),
                new FileEntry("build.xml", 3));
        var listing = new ListingVisitor();
        root.Accept(listing);
        foreach (string line in listing.Lines)
        {
            Console.WriteLine(line);
        }
        var size = new SizeVisitor();
        root.Accept(size);
        Console.WriteLine("Total size: " + size.Total);
    }
}
