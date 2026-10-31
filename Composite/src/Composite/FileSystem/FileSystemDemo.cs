namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// A home directory with files, a nested directory and an alias — six questions asked of a
/// tree of unknown depth, with no loop at the call site. Each method prints one slide's
/// output.
/// </summary>
public static class FileSystemDemo
{
    public static void SixQuestions()
    {
        var tree = new SampleTree();
        var home = tree.Home;

        home.List();

        Console.WriteLine();
        Console.WriteLine("-- six questions, one call each --");
        Console.WriteLine($"size of the whole tree : {home.Size()} bytes");
        Console.WriteLine($"size of Reports only   : {tree.Reports.Size()} bytes");
        Console.WriteLine($"elements in the tree   : {home.Count()}");
        Console.WriteLine($"newest anywhere        : {home.LastModified():yyyy-MM-ddTHH:mm:ssZ}");
        Console.WriteLine($"biggest leaf           : {home.Largest()?.GetName() ?? "none"}");
        Console.WriteLine($"find Selam.cs          : {home.Find("Selam.cs")?.GetName() ?? "not found"}");
        Console.WriteLine("over 40 KB, any depth  : "
            + string.Join(", ", home.FindAll(s => s.Size() > 40_000).Select(s => s.GetName())));
        Console.WriteLine("  Directories are in that list because a directory over 40 KB");
        Console.WriteLine("  is over 40 KB. The predicate never asked what kind it was.");
        Console.WriteLine("  Any depth, and not one loop at the call site.");
    }

    public static void DiskReportTotals()
    {
        var home = new Directory("akin");
        var dev = new Directory("Dev", home);
        _ = new File("Readme.txt", dev, 2_048);
        var report = new File("Report.docx", dev, 45_000);
        var reports = new Directory("Reports", dev);
        _ = new File("Important.docx", reports, 120_000);

        Console.WriteLine($"whole tree : {Bytes(new DiskReport(home))} bytes");
        Console.WriteLine($"one file   : {Bytes(new DiskReport(report))} bytes");
        Console.WriteLine("  The same client, and the same five questions. It cannot tell the");
        Console.WriteLine("  difference, because it holds an IStorage.");
    }

    private static string Bytes(DiskReport report) =>
        report.TotalBytes().ToString("N0", System.Globalization.CultureInfo.InvariantCulture);

    public static void Cache()
    {
        var tree = new SampleTree();
        var home = tree.Home;
        home.Size();   // fills the caches, so the three calls below find them full

        Console.WriteLine("-- the cache, and why the parent reference exists --");
        Directory.ResetRecomputations();
        home.Size();
        home.Size();
        home.Size();
        Console.WriteLine($"three calls, nothing changed : {Directory.Recomputations()}"
            + " totals computed — the tree is not walked at all");

        tree.AddNotes();                             // three levels down
        home.Size();
        Console.WriteLine($"after one file is added      : {Directory.Recomputations()}"
            + " — exactly Reports, Dev and akin");
        Console.WriteLine("  Invalidation runs upward, which is why an element keeps a");
        Console.WriteLine("  reference to its parent. Nothing below the change is touched.");
    }

    public static void Move()
    {
        var tree = new SampleTree();
        tree.AddNotes();

        Console.WriteLine("-- move Report.docx into Reports --");
        tree.Report.Move(tree.Reports);
        tree.Home.List();
        Console.WriteLine($"Report.docx now lives at: {tree.Report.Path()}");
        Console.WriteLine("  It left Dev and arrived in Reports — both halves, in one call.");
    }

    public static void Copy()
    {
        var tree = SampleTree.AfterChanges();

        Console.WriteLine("-- copy the Reports directory --");
        var duplicate = tree.Reports.Copy();
        duplicate.Rename("Reports (copy)");
        Console.WriteLine($"copy size             : {duplicate.Size()} bytes");
        Console.WriteLine("  A deep copy: the directory and everything under it, detached");
        Console.WriteLine("  from any parent, with the same total.");
    }

    public static void Walk()
    {
        var tree = SampleTree.AfterChanges();

        Console.WriteLine("-- walk the tree depth-first --");
        var walker = tree.Home.Iterator();
        while (walker.MoveNext())
        {
            Console.WriteLine($"  {walker.Current.GetName()}");
        }
    }

    /// <summary>The small tree every demo here runs on.</summary>
    private sealed class SampleTree
    {
        public Directory Home { get; } = new("akin");
        public Directory Reports { get; }
        public File Readme { get; }
        public File Report { get; }

        public SampleTree()
        {
            var dev = new Directory("Dev", Home);
            Readme = new File("Readme.txt", dev, 2_048);
            Report = new File("Report.docx", dev, 45_000);
            var cs = new File("Selam.cs", dev, 3_100);

            Reports = new Directory("Reports", dev);
            var important = new File("ImportantReport.docx", Reports, 120_000);
            _ = new Alias("Latest report", Reports, Report);

            Readme.Touch(DateTimeOffset.Parse("2026-01-04T09:00:00Z"));
            Report.Touch(DateTimeOffset.Parse("2026-03-19T14:30:00Z"));
            cs.Touch(DateTimeOffset.Parse("2026-02-11T08:15:00Z"));
            important.Touch(DateTimeOffset.Parse("2026-07-21T17:45:00Z"));
        }

        /// <summary>Adds Notes.md three levels down, the change <see cref="Cache"/> measures.</summary>
        public void AddNotes() => _ = new File("Notes.md", Reports, 900);

        /// <summary>The tree as it stands after the file is added and Report.docx has moved.</summary>
        public static SampleTree AfterChanges()
        {
            var tree = new SampleTree();
            tree.AddNotes();
            tree.Report.Move(tree.Reports);
            return tree;
        }
    }
}
