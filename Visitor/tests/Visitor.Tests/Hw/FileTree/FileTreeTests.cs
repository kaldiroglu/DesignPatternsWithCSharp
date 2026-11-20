using Xunit;
using F = global::dev.kaldiroglu.Visitor.Hw.FileTree;

namespace dev.kaldiroglu.Visitor.Tests.Hw.FileTree;

/// <summary>Homework two: the folder tree walks itself, so the visitors have no loop.</summary>
public class FileTreeTests
{
    private static F.Folder Project() =>
        new("project",
            new F.FileEntry("README.md", 2),
            new F.Folder("src",
                new F.FileEntry("Main.java", 4),
                new F.FileEntry("Order.java", 6)),
            new F.FileEntry("build.xml", 3));

    [Fact]
    public void TheListing()
    {
        var listing = new F.ListingVisitor();
        Project().Accept(listing);
        Assert.Equal(new[] { "project/", "  README.md (2)", "  src/", "    Main.java (4)",
            "    Order.java (6)", "  build.xml (3)" }, listing.Lines);
    }

    [Fact]
    public void TheTotalSize()
    {
        var size = new F.SizeVisitor();
        Project().Accept(size);
        Assert.Equal(15, size.Total);
    }

    /// <summary>Folder.Accept does the walking: no visitor has a loop over the children.</summary>
    [Fact]
    public void TheFolderWalks()
    {
        Assert.Contains("foreach (IEntry child in Children)", Printed.CodeOf("Hw/FileTree/Folder.cs"));
        foreach (var visitor in new[] { "ListingVisitor", "SizeVisitor" })
        {
            var code = Printed.CodeOf($"Hw/FileTree/{visitor}.cs");
            Assert.DoesNotContain("foreach", code);
            Assert.DoesNotContain("Children", code);
        }
    }

    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[] { "project/", "  README.md (2)", "  src/", "    Main.java (4)",
            "    Order.java (6)", "  build.xml (3)", "Total size: 15" }, Printed.By(F.Main.Run));
    }
}
