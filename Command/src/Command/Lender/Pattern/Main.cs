namespace dev.kaldiroglu.Command.Lender.Pattern;

/// <summary>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- lender-pattern</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        ICommand command = new Borrower();

        var lender = new Lender();
        lender.Lend(command, 1000);

        command = new TaxOffice();
        lender.Lend(command, 2000);
    }
}
