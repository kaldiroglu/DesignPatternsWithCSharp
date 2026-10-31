using dev.kaldiroglu.Composite.FileSystem;
using Xunit;
using Directory = dev.kaldiroglu.Composite.FileSystem.Directory;
using File = dev.kaldiroglu.Composite.FileSystem.File;

namespace dev.kaldiroglu.Composite.Tests.FileSystem;

/// <summary>What keeps the file system a tree: one parent for each element, and no cycle.</summary>
public class DirectoryTests
{
    private readonly Directory _home = new("akin");
    private readonly Directory _dev;
    private readonly Directory _reports;
    private readonly File _report;

    public DirectoryTests()
    {
        _dev = new Directory("Dev", _home);
        _ = new File("Readme.txt", _dev, 2_048);
        _report = new File("Report.docx", _dev, 45_000);
        _reports = new Directory("Reports", _dev);
        _ = new File("Important.docx", _reports, 120_000);
    }

    [Fact(DisplayName = "a directory cannot move into its own descendant, and nothing changes")]
    public void ADirectoryCannotMoveBelowItself()
    {
        Assert.Throws<ArgumentException>(() => _dev.Move(_reports));

        Assert.Same(_home, _dev.GetParent());
        Assert.Contains(_dev, _home.Elements());
        Assert.DoesNotContain(_dev, _reports.Elements());
    }

    [Fact(DisplayName = "a directory cannot add its own ancestor")]
    public void ADirectoryCannotAddItsAncestor()
    {
        Assert.Throws<ArgumentException>(() => _reports.Add(_home));

        Assert.DoesNotContain(_home, _reports.Elements());
        Assert.Null(_home.GetParent());
    }

    [Fact(DisplayName = "add moves the child: it leaves its old directory")]
    public void AddTakesTheChildFromItsOldParent()
    {
        var totalBefore = _home.Size();

        _reports.Add(_report);

        Assert.Same(_reports, _report.GetParent());
        Assert.DoesNotContain(_report, _dev.Elements());
        Assert.Single(_reports.Elements(), e => ReferenceEquals(e, _report));
        Assert.Equal(totalBefore, _home.Size());
    }

    [Fact(DisplayName = "the disk report demo prints the two totals")]
    public void DiskReportDemoPrintsTheTotals()
    {
        TextWriter original = Console.Out;
        var buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            FileSystemDemo.DiskReportTotals();
        }
        finally
        {
            Console.SetOut(original);
        }

        Assert.Equal(167_816, new DiskReport(_home).TotalBytes());
        Assert.Contains("167,816 bytes", buffer.ToString());
        Assert.Contains("45,000 bytes", buffer.ToString());
    }
}
