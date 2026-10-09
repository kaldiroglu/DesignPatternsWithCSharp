using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.TemplateMethod.Tests.Hw;

using dev.kaldiroglu.TemplateMethod.Hw.CallCenter;
using dev.kaldiroglu.TemplateMethod.Hw.Onboarding;
using dev.kaldiroglu.TemplateMethod.Hw.RecordFile;

/// <summary>The worked solutions of the Template Method homework, and what the speaker notes say about them.</summary>
public class HomeworkTests
{
    // ------------------------------------------------------------ 1 · three call centers

    /// <summary>A call center whose audio matches stores every metadata and audio record.</summary>
    [Fact]
    public void IstanbulStoresEverything()
    {
        CallImport istanbul = new IstanbulCallCenter();
        istanbul.Run();

        Assert.Equal(["metadata IST-1", "metadata IST-2", "audio IST-1", "audio IST-2"], istanbul.Stored);
        Assert.Empty(istanbul.Rejected);
    }

    /// <summary>A recording that is too short is rejected by the verification step.</summary>
    [Fact]
    public void AnkaraShortRecordingIsRejected()
    {
        CallImport ankara = new AnkaraCallCenter();
        ankara.Run();

        Assert.Equal(["ANK-2"], ankara.Rejected);
        Assert.Equal(["metadata ANK-1", "metadata ANK-2", "audio ANK-1"], ankara.Stored);
    }

    /// <summary>A call center with no calls stores nothing.</summary>
    [Fact]
    public void IzmirHasNoCalls()
    {
        CallImport izmir = new IzmirCallCenter();
        izmir.Run();

        Assert.Empty(izmir.Stored);
    }

    /// <summary>
    /// Verification is private and the template method is final, so no call center can skip
    /// it. Java's <c>final</c> is "not <c>virtual</c>" in C#.
    /// </summary>
    [Fact]
    public void VerificationCannotBeSkipped()
    {
        var run = typeof(CallImport).GetMethod("Run", Type.EmptyTypes)!;
        var verify = typeof(CallImport)
            .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static
                        | BindingFlags.Public | BindingFlags.NonPublic)
            .First(m => m.Name == "Verify");

        Assert.False(run.IsVirtual);
        Assert.True(verify.IsPrivate);
    }

    // ------------------------------------------------------------ 2 · a first day

    /// <summary>The equipment hook gives an employee a laptop, and a contractor overrides it to give nothing.</summary>
    [Fact]
    public void TheEquipmentHook()
    {
        Assert.Equal(["e-mail for Elif", "payroll for Elif", "laptop for Elif",
                "mentor for Elif", "welcome message to Elif"],
            new EmployeeOnboarding().Start("Elif"));
        Assert.Equal(["e-mail for Mert", "mentor for Mert", "welcome message to Mert"],
            new ContractorOnboarding().Start("Mert"));
    }

    // ------------------------------------------------------------ 3 · a file of records

    /// <summary>The record reader turns each line into a record and skips blank lines.</summary>
    [Fact]
    public void ReadsEveryLine()
    {
        IReadOnlyList<Customer> customers = new CustomerFileReader()
            .ReadAll(new StringReader("Ayse;Istanbul\n\nDeniz ; Izmir\n"));

        Assert.Equal([new Customer("Ayse", "Istanbul"), new Customer("Deniz", "Izmir")], customers);
    }

    /// <summary>The reader is closed even when a line fails to parse.</summary>
    [Fact]
    public void TheReaderIsClosedWhenParsingFails()
    {
        var source = new TrackingReader("Ayse;Istanbul\nnot a customer\n");

        Assert.Throws<ArgumentException>(() => new CustomerFileReader().ReadAll(source));
        Assert.True(source.Closed);
    }

    /// <summary>A reader that remembers whether it was closed. Close and Dispose both end here.</summary>
    private sealed class TrackingReader(string text) : StringReader(text)
    {
        public bool Closed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            Closed = true;
            base.Dispose(disposing);
        }
    }
}
