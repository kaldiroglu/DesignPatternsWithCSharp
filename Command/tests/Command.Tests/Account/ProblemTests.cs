namespace dev.kaldiroglu.Command.Tests.Account;

// The usings sit inside the namespace, so that the name Account finds the type before it
// finds the namespace of the same name.
using System.Reflection;
using System.Runtime.CompilerServices;
using dev.kaldiroglu.Command.Account.Domain;
using dev.kaldiroglu.Command.Account.Problem;
using Xunit;

/// <summary>
/// The three naive designs, and what each one costs. All three undo correctly for the case
/// they were built for, so every figure the slides quote about them is measured here.
/// Ported from the Java <c>account.ProblemTest</c>.
/// </summary>
public class ProblemTests
{
    private const string Source = "Account/Problem/";

    private static Money Lira(string amount) => Money.Of(amount);

    /// <summary>The source of a class with every comment removed, so its comments cannot match.</summary>
    internal static string CodeOf(string relativePath) =>
        SourceText.StripComments(SourceText.Read(relativePath));

    /// <summary>
    /// The public operations a class declares. Java counts public methods, and its accessors
    /// such as <c>balance()</c> are methods. In C# they are properties, so this set holds the
    /// declared public methods that are not property accessors, and the declared public
    /// properties.
    /// </summary>
    internal static ISet<string> PublicOperationsOf(Type type)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
                                   | BindingFlags.DeclaredOnly;
        return type.GetMethods(flags).Where(m => !m.IsSpecialName).Select(m => m.Name)
            .Concat(type.GetProperties(flags).Select(p => p.Name))
            .ToHashSet();
    }

    // ------------------------------------------- stage one: the account remembers one move

    [Fact(DisplayName = "stage one: one undo works")]
    public void OneUndoWorks()
    {
        var account = new OneStepAccount("Deniz", Lira("1000.00"));
        account.Withdraw(Lira("200.00"));
        account.Undo();

        Assert.Equal(Lira("1000.00"), account.Balance);
    }

    [Fact(DisplayName = "stage one: a second undo finds nothing, because the first one erased it")]
    public void OnlyOneStep()
    {
        var account = new OneStepAccount("Deniz", Lira("1000.00"));
        account.Deposit(Lira("500.00"));
        account.Withdraw(Lira("200.00"));

        account.Undo();
        account.Undo();

        // The deposit can no longer be undone.
        Assert.Equal(Lira("1500.00"), account.Balance);
    }

    [Fact(DisplayName = "stage one: two of the account's three instance fields are bookkeeping")]
    public void TheAccountCarriesBookkeeping()
    {
        // Java reads the field names owner, balance, lastKind and lastAmount, in that order.
        // In C# the fields are _owner, _lastKind and _lastAmount, and balance is the
        // property Balance with a backing field the compiler writes. The names are read back
        // without the underscore, the backing field under its property's name, and compared
        // as a set, because reflection does not promise the declaration order.
        var fields = typeof(OneStepAccount)
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                       | BindingFlags.DeclaredOnly)
            .Select(FieldName)
            .ToList();

        Assert.Equal(new HashSet<string> { "owner", "balance", "lastKind", "lastAmount" }, fields.ToHashSet());
        Assert.Equal(4, fields.Count);
        Assert.Equal(2, fields.Count(n => n.StartsWith("last", StringComparison.Ordinal)));
    }

    private static string FieldName(FieldInfo field)
    {
        var name = field.Name;
        if (field.IsDefined(typeof(CompilerGeneratedAttribute), false) && name.StartsWith('<'))
        {
            name = name[1..name.IndexOf('>')];          // <Balance>k__BackingField -> Balance
        }
        name = name.TrimStart('_');
        return char.ToLowerInvariant(name[0]) + name[1..];
    }

    // ------------------------------------------------- stage two: the account keeps a history

    [Fact(DisplayName = "stage two: undo as far back as you like, redo, and a journal")]
    public void TheHistoryWorks()
    {
        var account = new HistoryAccount("Deniz", Lira("1000.00"));
        account.Deposit(Lira("500.00"));
        account.Withdraw(Lira("200.00"));

        account.Undo();
        account.Undo();
        Assert.Equal(Lira("1000.00"), account.Balance);

        account.Redo();
        Assert.Equal(Lira("1500.00"), account.Balance);
        Assert.Equal(["deposit 500.00", "withdraw 200.00", "undo withdraw 200.00",
            "undo deposit 500.00", "redo deposit 500.00"], account.Journal());
    }

    [Fact(DisplayName = "stage two: eight public operations, and three of them are banking")]
    public void AFamilyOfHelperMethods()
    {
        var all = PublicOperationsOf(typeof(HistoryAccount));
        var banking = PublicOperationsOf(typeof(dev.kaldiroglu.Command.Account.Domain.Account));

        Assert.Equal(8, all.Count);
        // Balance, Deposit and Withdraw are what an account is for.
        Assert.Equal(3, all.Count(banking.Contains));
        Assert.Equal(new HashSet<string> { "Undo", "Redo", "CanUndo", "CanRedo", "Journal" },
            all.Where(m => !banking.Contains(m)).ToHashSet());
    }

    [Fact(DisplayName = "stage two: every operation lives in three places")]
    public void EveryOperationIsThreeEdits()
    {
        var code = CodeOf(Source + "HistoryAccount.cs");

        // One switch to undo, one to redo, and each kind is a branch in both.
        Assert.Equal(2, SourceText.CountOf(code, "switch ("));
        Assert.Equal(2 * Enum.GetValues<Kind>().Length, SourceText.CountOf(code, "case "));
    }

    // ------------------------------------------------ stage three: the history moves out

    [Fact(DisplayName = "stage three: the account is clean again and the teller undoes")]
    public void TheTellerWorks()
    {
        var deniz = new dev.kaldiroglu.Command.Account.Domain.Account("Deniz", Lira("1000.00"));
        var teller = new Teller();
        teller.Withdraw(deniz, Lira("200.00"));
        teller.Deposit(deniz, Lira("50.00"));

        teller.Undo();
        teller.Undo();
        Assert.Equal(Lira("1000.00"), deniz.Balance);

        teller.Redo();
        Assert.Equal(Lira("800.00"), deniz.Balance);
        // Banking and nothing else.
        Assert.Equal(new HashSet<string> { "Owner", "Balance", "Deposit", "Withdraw" },
            PublicOperationsOf(typeof(dev.kaldiroglu.Command.Account.Domain.Account)));
    }

    [Fact(DisplayName = "stage three: the switches have moved into the teller, and there are still two")]
    public void TheSwitchesMoved()
    {
        var code = CodeOf(Source + "Teller.cs");

        Assert.Equal(2, SourceText.CountOf(code, "switch ("));
        Assert.Equal(2 * Enum.GetValues<Kind>().Length, SourceText.CountOf(code, "case "));
    }

    // ---------------------------------------------------------------------- the reversal

    [Fact(DisplayName = "the reversal: undo takes back half a transfer, and 300 lira vanish")]
    public void TheReversal()
    {
        var deniz = new dev.kaldiroglu.Command.Account.Domain.Account("Deniz", Lira("1000.00"));
        var emre = new dev.kaldiroglu.Command.Account.Domain.Account("Emre", Lira("0.00"));
        var teller = new Teller();

        teller.Transfer(deniz, emre, Lira("300.00"));
        Assert.Equal(Lira("700.00"), deniz.Balance);
        Assert.Equal(Lira("300.00"), emre.Balance);

        // The teller presses Undo once, to take back the transfer.
        teller.Undo();

        Assert.Equal(Lira("700.00"), deniz.Balance);   // the withdrawal is still there
        Assert.Equal(Lira("0.00"), emre.Balance);      // the deposit is gone
        // Money that left one account and arrived nowhere.
        Assert.Equal(Lira("300.00"), Lira("1000.00").Minus(deniz.Balance.Plus(emre.Balance)));

        // One transfer, and the journal cannot tell it was one.
        Assert.Equal(["withdraw 300.00 Deniz", "deposit 300.00 Emre", "undo deposit 300.00 Emre"],
            teller.Journal());
    }

    [Fact(DisplayName = "the reversal: transfer adds no branch, which is exactly why it breaks")]
    public void TheTransferReusesAndRecordsTwice()
    {
        var code = CodeOf(Source + "Teller.cs");
        var start = code.IndexOf("public void Transfer", StringComparison.Ordinal);
        var end = code.IndexOf("public void Undo", StringComparison.Ordinal);
        var transfer = code[start..end];

        Assert.Equal(1, SourceText.CountOf(transfer, "Withdraw("));
        Assert.Equal(1, SourceText.CountOf(transfer, "Deposit("));
        // It records nothing of its own.
        Assert.Equal(0, SourceText.CountOf(transfer, "Record("));
    }
}
