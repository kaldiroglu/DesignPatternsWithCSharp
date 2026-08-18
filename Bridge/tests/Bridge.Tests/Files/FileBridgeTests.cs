using System.Text;
using dev.kaldiroglu.Bridge.Files.Solution;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Files;

/// <summary>
/// The client's real scenario: two departments with opposite retention rules, over three
/// document stores.
/// </summary>
public class FileBridgeTests
{
    private const string SolutionNs = "dev.kaldiroglu.Bridge.Files.Solution";

    private static List<IFileProvider> Stores() =>
        [new EvernoteProvider(), new SharePointProvider(), new FileNetProvider()];

    [Fact(DisplayName = "one rule, every store, the same answer")]
    public void OneRuleEveryStore()
    {
        foreach (var store in Stores())
        {
            FileManager finance = new FinanceFileManager(store);
            for (var i = 1; i <= 8; i++)
            {
                finance.Save("q3", $"draft {i}");
            }

            Assert.Equal(8, finance.Versions("q3").Count);
            Assert.Equal("draft 8", finance.Read("q3"));
            Assert.Equal(5, finance.RetainedVersions);
        }
    }

    [Fact(DisplayName = "two departments keep different histories of the same store")]
    public void TwoDepartmentsOneStore()
    {
        IFileProvider store = new SharePointProvider();

        Assert.Equal(5, new FinanceFileManager(store).RetainedVersions);
        Assert.Equal(2, new InsuranceFileManager(store).RetainedVersions);
    }

    [Fact(DisplayName = "the store can be changed on a manager that already exists")]
    public void TheStoreCanChangeAtRunTime()
    {
        FileManager finance = new FinanceFileManager(new EvernoteProvider());
        finance.Save("memo", "on Evernote");
        Assert.Equal("on Evernote", finance.Read("memo"));

        finance.SetProvider(new FileNetProvider());
        finance.Save("memo", "on FileNet");
        Assert.Equal("on FileNet", finance.Read("memo"));
    }

    [Fact(DisplayName = "the provider offers storage primitives, not the manager's operations")]
    public void TheInterfacesAreNotTheSameInterfaceTwice()
    {
        var providerMethods = typeof(IFileProvider).GetMethods().Select(m => m.Name).ToList();

        // Read and Versions are genuinely shared vocabulary; Save and SetProvider must not
        // appear, because they are the manager's operations rather than the store's primitives.
        Assert.DoesNotContain("Save", providerMethods);
        Assert.DoesNotContain("SetProvider", providerMethods);

        // And the primitives the manager composes with really are there.
        Assert.All(new[] { "Open", "Write", "DeleteVersion" },
            name => Assert.Contains(name, providerMethods));
    }

    [Fact(DisplayName = "2 departments and 3 stores are 5 classes, not 6")]
    public void MPlusNNotMTimesN()
    {
        // Counted from the namespace, not from two lists written here. Listing the classes and
        // then asserting 2 + 3 == 5 would prove something about integers, and would go on
        // passing the day a fourth store is added and the slide still says five.
        var departments = TypeCensus.ConcreteImplementationsOf(SolutionNs, typeof(FileManager));
        var stores = TypeCensus.ConcreteImplementationsOf(SolutionNs, typeof(IFileProvider));

        Assert.Equal(2, departments);   // refined abstractions
        Assert.Equal(3, stores);        // concrete implementors
        Assert.Equal(5, departments + stores);   // the classes that carry the two axes
        Assert.Equal(6, departments * stores);   // the grid a class-per-pair design writes

        Assert.Equal(typeof(IFileProvider),
            TypeCensus.Field(typeof(FileManager), "Provider").FieldType);
    }

    [Fact(DisplayName = "nothing in this namespace is called an adapter")]
    public void NoAdapterInTheNames()
    {
        foreach (var type in TypeCensus.In(SolutionNs))
        {
            Assert.False(type.Name.ToLowerInvariant().Contains("adapt"),
                $"{type.Name} is an Implementor, not an Adapter — an adapter makes an existing "
                + "incompatible interface fit, after the fact");
        }
    }

    [Fact(DisplayName = "the tombstone keeps version numbers stable when old ones are trimmed")]
    public void TrimmingDoesNotRenumber()
    {
        IFileProvider store = new EvernoteProvider();
        FileManager insurance = new InsuranceFileManager(store);

        for (var i = 1; i <= 8; i++)
        {
            insurance.Save("policy", $"draft {i}");
        }

        // Eight versions were written and six were trimmed, but version 8 is still version 8 —
        // otherwise every retention pass would silently rewrite the audit trail's own numbering.
        Assert.Equal(8, insurance.Versions("policy").Count);
        Assert.Equal("draft 8", insurance.Read("policy"));
        Assert.Equal(2, insurance.RetainedVersions);
        Assert.Equal(Encoding.UTF8.GetBytes("draft 8"), store.Read(store.Open("policy")));
    }
}
