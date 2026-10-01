namespace dev.kaldiroglu.Command.Lender.Problem1;

/// <summary>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- lender-problem1</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        var borrower = new Borrower();
        var lender = new Lender();
        lender.Lend(borrower, 1000);
    }
}
