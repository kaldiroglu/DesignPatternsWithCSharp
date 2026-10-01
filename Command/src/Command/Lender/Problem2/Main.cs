namespace dev.kaldiroglu.Command.Lender.Problem2;

/// <summary>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- lender-problem2</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        IBorrower borrower = new ConcreteBorrower1();

        var lender = new Lender();
        lender.Lend(borrower, 1000);

        borrower = new ConcreteBorrower2();
        lender.Lend(borrower, 2000);
    }
}
