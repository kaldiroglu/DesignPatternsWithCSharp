namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// A home directory with files, a nested directory and an alias — six questions asked of a
/// tree of unknown depth, with no loop at the call site.
/// </summary>
public static class FileSystemDemo
{
    public static void Run()
    {
        var home = new Directory("akin");
        var dev = new Directory("Dev", home);
        var readme = new File("Readme.txt", dev, 2_048);
        var report = new File("Report.docx", dev, 45_000);
        var cs = new File("Selam.cs", dev, 3_100);

        var reports = new Directory("Reports", dev);
        var important = new File("ImportantReport.docx", reports, 120_000);
        _ = new Alias("Latest report", reports, report);

        readme.Touch(DateTimeOffset.Parse("2026-01-04T09:00:00Z"));
        report.Touch(DateTimeOffset.Parse("2026-03-19T14:30:00Z"));
        cs.Touch(DateTimeOffset.Parse("2026-02-11T08:15:00Z"));
        important.Touch(DateTimeOffset.Parse("2026-07-21T17:45:00Z"));

        home.List();

        Console.WriteLine();
        Console.WriteLine("-- six questions, one call each --");
        Console.WriteLine($"size of the whole tree : {home.Size()} bytes");
        Console.WriteLine($"size of Reports only   : {reports.Size()} bytes");
        Console.WriteLine($"elements in the tree   : {home.Count()}");
        Console.WriteLine($"newest anywhere        : {home.LastModified():yyyy-MM-ddTHH:mm:ssZ}");
        Console.WriteLine($"biggest leaf           : {home.Largest()?.GetName() ?? "none"}");
        Console.WriteLine($"find Selam.cs          : {home.Find("Selam.cs")?.GetName() ?? "not found"}");
        Console.WriteLine("over 40 KB, any depth  : "
            + string.Join(", ", home.FindAll(s => s.Size() > 40_000).Select(s => s.GetName())));
        Console.WriteLine("  Directories are in that list because a directory over 40 KB");
        Console.WriteLine("  is over 40 KB. The predicate never asked what kind it was.");
        Console.WriteLine("  Any depth, and not one loop at the call site.");

        Console.WriteLine();
        Console.WriteLine("-- the same questions asked of a single file --");
        var leafReport = new DiskReport(readme);
        Console.Write(leafReport.Summary());
        Console.WriteLine("  A leaf answers all five. The client cannot tell the difference.");

        Console.WriteLine();
        Console.WriteLine("-- the cache, and why the parent reference exists --");
        Directory.ResetRecomputations();
        home.Size();
        home.Size();
        home.Size();
        Console.WriteLine($"three calls, nothing changed : {Directory.Recomputations()}"
            + " totals computed — the tree is not walked at all");

        _ = new File("Notes.md", reports, 900);      // three levels down
        home.Size();
        Console.WriteLine($"after one file is added      : {Directory.Recomputations()}"
            + " — exactly Reports, Dev and akin");
        Console.WriteLine("  Invalidation runs upward, which is why an element keeps a");
        Console.WriteLine("  reference to its parent. Nothing below the change is touched.");

        Console.WriteLine();
        Console.WriteLine("-- move Report.docx into Reports --");
        report.Move(reports);
        home.List();
        Console.WriteLine($"Report.docx now lives at: {report.Path()}");
        Console.WriteLine("  It left Dev and arrived in Reports — both halves, in one call.");

        Console.WriteLine();
        Console.WriteLine("-- copy the Reports directory --");
        var duplicate = reports.Copy();
        duplicate.Rename("Reports (copy)");
        Console.WriteLine($"copy size             : {duplicate.Size()} bytes");
        Console.WriteLine("  A deep copy: the directory and everything under it, detached");
        Console.WriteLine("  from any parent, with the same total.");

        Console.WriteLine();
        Console.WriteLine("-- walk the tree depth-first --");
        var walker = home.Iterator();
        while (walker.MoveNext())
        {
            Console.WriteLine($"  {walker.Current.GetName()}");
        }
    }
}
