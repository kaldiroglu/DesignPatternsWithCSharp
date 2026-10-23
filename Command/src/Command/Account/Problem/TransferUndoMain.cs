using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Problem;

/// <summary>Stage three: the teller transfers 300, and one undo takes back only half of it.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- account-problem-transfer-undo</c>.
/// </remarks>
public static class TransferUndoMain
{
    public static void Run()
    {
        var deniz = new Domain.Account("Deniz", Money.Of("1000.00"));
        var emre = new Domain.Account("Emre", Money.Of("0.00"));
        var teller = new Teller();
        teller.Transfer(deniz, emre, Money.Of("300.00"));
        Console.WriteLine("Stage three: transfer 300 from Deniz to Emre. Deniz "
            + deniz.Balance + ", Emre " + emre.Balance);
        teller.Undo();
        Console.WriteLine("Undo pressed once. Deniz " + deniz.Balance + ", Emre " + emre.Balance);
        Console.WriteLine("Undo took back only the deposit. "
            + Money.Of("1000.00").Minus(deniz.Balance.Plus(emre.Balance))
            + " lira left Deniz and arrived nowhere.");
        Console.WriteLine("Journal: " + Show(teller.Journal()));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
