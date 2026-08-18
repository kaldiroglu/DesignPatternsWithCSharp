using dev.kaldiroglu.Bridge.Violation;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Violation;

/// <summary>
/// The violation is silent by nature, so it can only be caught by capturing what a caller
/// holding the supertype actually sees. That is what these tests do.
/// </summary>
public class ViolationTests
{
    private static string OutputOf(Action<AType> action, AType subject)
    {
        var original = Console.Out;
        var captured = new StringWriter();
        Console.SetOut(captured);
        try
        {
            action(subject);
        }
        finally
        {
            Console.SetOut(original);
        }

        return captured.ToString();
    }

    [Fact(DisplayName = "the supertype prints, which is the contract")]
    public void TheSupertypeKeepsItsPromise() =>
        Assert.Equal("My variable: 42" + Environment.NewLine,
            OutputOf(s => s.DoIt(), new AType(42, true)));

    [Fact(DisplayName = "the subtype prints nothing, and says nothing about it")]
    public void TheSubtypeBreaksItSilently()
    {
        var output = OutputOf(s => s.DoIt(), new ASubType(42, true));

        Assert.Equal("", output);
        Assert.NotEqual(OutputOf(s => s.DoIt(), new AType(42, true)), output);
    }

    [Fact(DisplayName = "a caller holding AType cannot tell, except by testing the type")]
    public void SubstitutabilityIsBroken()
    {
        AType[] both = [new AType(42, true), new ASubType(42, true)];

        var printed = both.Count(each => OutputOf(s => s.DoIt(), each).Length > 0);

        // Two objects of the declared type, one of them silent. No exception, no signal.
        Assert.Equal(1, printed);
    }

    [Fact(DisplayName = "and the stored string is unset until DoIt has run")]
    public void TheSecondBrokenPromise()
    {
        var early = new ASubType(42, true);
        Assert.Null(early.AStringVariable);

        early.DoIt();
        Assert.Equal("My variable: 42", early.AStringVariable);
    }

    [Fact(DisplayName = "the fix is delegation, not a better override")]
    public void DelegationCannotDoThis()
    {
        // A refinement in Bridge.Basic.Pattern owns its own DoIt() and merely calls the
        // implementation. Whatever the implementation does, the refinement's own contract is
        // still executed — there is no override to break it.
        Assert.True(typeof(global::dev.kaldiroglu.Bridge.Basic.Pattern.IAnAbstraction)
            .IsAssignableFrom(typeof(global::dev.kaldiroglu.Bridge.Basic.Pattern.ASubAbstraction)));
        Assert.Single(typeof(global::dev.kaldiroglu.Bridge.Basic.Pattern.ASubAbstraction).GetFields(
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic));
    }

    [Fact(DisplayName = "C# made the author write virtual, and it still did not help")]
    public void OptingInIsNotSanctioning()
    {
        // Java lets any method be overridden. C# requires `virtual` on the supertype first —
        // one more deliberate step, and no protection at all: the contract is still broken by
        // an override the author of AType explicitly permitted.
        var doIt = typeof(AType).GetMethod(nameof(AType.DoIt))!;
        Assert.True(doIt.IsVirtual);
        Assert.NotEqual(typeof(AType), typeof(ASubType).GetMethod(nameof(AType.DoIt))!.DeclaringType);
    }
}
