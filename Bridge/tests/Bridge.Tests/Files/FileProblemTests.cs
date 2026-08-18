using dev.kaldiroglu.Bridge.Files.Problem;
using Xunit;
using Solution = dev.kaldiroglu.Bridge.Files.Solution;

namespace dev.kaldiroglu.Bridge.Tests.Files;

/// <summary>
/// The three ways the client's system could have been written instead, and the breach each one
/// leaves behind.
/// </summary>
public class FileProblemTests
{
    private const string Path = "claims/2026/case-4021";

    private static string Source(string file) =>
        File.ReadAllText(System.IO.Path.Combine(
            TypeCensus.SourceRoot, "Files", "Problem", file));

    private readonly VendorStores _stores = new();

    // ------------------------------------------- design 1: a switch on each axis

    [Fact(DisplayName = "switch: it works, and both departments get their own rule on Evernote")]
    public void TheSwitchWorks()
    {
        var manager = new SwitchingFileManager(_stores);
        for (var i = 1; i <= 8; i++)
        {
            manager.Save(Department.Finance, Store.Evernote, Path, $"draft {i}");
            manager.Save(Department.Insurance, Store.Evernote, Path, $"draft {i}");
        }

        Assert.Equal(5, _stores.VersionsHeld("Evernote", $"finance/{Path}"));
        Assert.Equal(2, _stores.VersionsHeld("Evernote", $"insurance/{Path}"));
        Assert.Equal("draft 8", _stores.LatestContent("Evernote", $"finance/{Path}"));
    }

    [Fact(DisplayName = "switch: insurance keeps two versions on Evernote and all eight on FileNet")]
    public void TheForgottenRetentionRule()
    {
        var manager = new SwitchingFileManager(_stores);
        for (var i = 1; i <= 8; i++)
        {
            manager.Save(Department.Insurance, Store.Evernote, Path, $"draft {i}");
            manager.Save(Department.Insurance, Store.FileNet, Path, $"draft {i}");
        }

        Assert.Equal(2, _stores.VersionsHeld("Evernote", $"insurance/{Path}"));

        // The same department, the same rule, a different store — and the rule is simply not
        // there. Nothing threw. Insurance is now holding six versions it is not allowed to keep,
        // and only an auditor or this assertion can tell.
        Assert.Equal(8, _stores.VersionsHeld("FileNet", $"insurance!{Path}"));
        Assert.Equal(2, Department.Insurance.RetainedVersions());
    }

    [Fact(DisplayName = "switch: six branches by hand, and the retention rule written in five")]
    public void TheRulesLeak()
    {
        var source = Source("SwitchingFileManager.cs");
        var body = source[source.IndexOf("public sealed class", StringComparison.Ordinal)..];

        var leaves = TypeCensus.CountOf(body, "case Store.Evernote:")
                     + TypeCensus.CountOf(body, "case Store.SharePoint:")
                     + TypeCensus.CountOf(body, "case Store.FileNet:");
        Assert.Equal(6, leaves);   // branches, one per pair, written by hand

        // Six branches store something; only five of them then trim.
        Assert.Equal(5, TypeCensus.CountOf(body, "kept.Count -"));
    }

    // ------------------------------------------- design 2: a class per pair

    [Fact(DisplayName = "class per pair: it works, and the class name has to state both axes")]
    public void ClassPerPairWorks()
    {
        var finance = new FinanceEvernoteManager(_stores);
        var insurance = new InsuranceEvernoteManager(_stores);
        for (var i = 1; i <= 8; i++)
        {
            finance.Save(Path, $"draft {i}");
            insurance.Save(Path, $"draft {i}");
        }

        Assert.Equal(5, _stores.VersionsHeld("Evernote", $"finance/{Path}"));
        Assert.Equal(2, _stores.VersionsHeld("Evernote", $"insurance/{Path}"));

        foreach (var pair in new[]
                 {
                     typeof(FinanceEvernoteManager), typeof(InsuranceEvernoteManager),
                     typeof(FinanceSharePointManager)
                 })
        {
            var name = pair.Name;
            Assert.True(name.StartsWith("Finance") || name.StartsWith("Insurance"),
                $"{name} states its department");
            Assert.NotEqual("Manager", name.Replace("Finance", "").Replace("Insurance", ""));
        }
    }

    [Fact(DisplayName = "class per pair: one rule, written once per store; one store, once per rule")]
    public void TheSameThingTwice()
    {
        var financeEvernote = Source("FinanceEvernoteManager.cs");
        var insuranceEvernote = Source("InsuranceEvernoteManager.cs");
        var financeSharePoint = Source("FinanceSharePointManager.cs");

        // Same store, two departments: the vendor call is duplicated.
        Assert.Contains("EvernoteCreateNote", financeEvernote);
        Assert.Contains("EvernoteCreateNote", insuranceEvernote);

        // Same department, two stores: the retention number is duplicated.
        Assert.Contains("Keep = 5", financeEvernote);
        Assert.Contains("Keep = 5", financeSharePoint);

        // 2 departments x 3 stores. Three of the six are written out in this namespace.
        Assert.Equal(6, Enum.GetValues<Department>().Length * Enum.GetValues<Store>().Length);
    }

    // ------------------------------------------- design 3: inherit the store

    [Fact(DisplayName = "inherit: the store is the base class, so it cannot be changed at all")]
    public void TheStoreIsWeldedOn()
    {
        var manager = new EvernoteBoundFinanceManager(_stores);
        for (var i = 1; i <= 8; i++)
        {
            manager.Save(Path, $"draft {i}");
        }

        Assert.Equal(5, _stores.VersionsHeld("Evernote", $"finance/{Path}"));

        // There is no SetStore, and there cannot be: a base class is chosen when the code is
        // compiled. When the Evernote contract ends, this object cannot follow the documents.
        Assert.DoesNotContain(typeof(EvernoteBoundFinanceManager).GetMethods(),
            m => m.Name.ToLowerInvariant().Contains("setstore")
                 || m.Name.ToLowerInvariant().Contains("setprovider"));

        // And the vendor is in the type itself, so every caller that names this class names the
        // vendor too.
        Assert.Contains("Evernote", typeof(EvernoteBoundFinanceManager).Name);
    }

    // ------------------------------------------- and what the solution costs instead

    [Fact(DisplayName = "the bridge does what none of the three can: move a live manager to another store")]
    public void TheBridgeAnswer()
    {
        Solution.IFileProvider evernote = new Solution.EvernoteProvider();
        Solution.IFileProvider sharePoint = new Solution.SharePointProvider();

        Solution.FileManager finance = new Solution.FinanceFileManager(evernote);
        finance.Save(Path, "draft 1");

        // The same object, a different store, decided while the program runs. None of the three
        // designs above can express this line.
        finance.SetProvider(sharePoint);
        finance.Save(Path, "draft 2");

        Assert.Equal("draft 1",
            System.Text.Encoding.UTF8.GetString(evernote.Read(evernote.Open(Path))));
        Assert.Equal("draft 2",
            System.Text.Encoding.UTF8.GetString(sharePoint.Read(sharePoint.Open(Path))));
        Assert.Equal(5, finance.RetainedVersions);
    }
}
