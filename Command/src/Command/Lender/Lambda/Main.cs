namespace dev.kaldiroglu.Command.Lender.Lambda;

/// <summary>
/// The same two loans as <c>Lender.Pattern.Main</c>, with lambdas instead of command classes.
/// Run it with <c>dotnet run --project src/Command.Demo -- lender-lambda</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        Action<int> borrower = money =>
            Console.WriteLine("Borrowing " + money.ToString(System.Globalization.CultureInfo.InvariantCulture)
                              + " and spending for family!");
        Action<int> taxOffice = _ => Console.WriteLine("Receiving for the tax payment!");

        var lender = new Lender();
        lender.Lend(borrower, 1000);
        lender.Lend(taxOffice, 2000);
    }
}
