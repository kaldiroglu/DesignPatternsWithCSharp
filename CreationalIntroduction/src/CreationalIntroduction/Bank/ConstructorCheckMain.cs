namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>A constructor that checks the total the client calculated.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- bank-constructor-check</c>.
/// </remarks>
public static class ConstructorCheckMain
{
    public static void Run()
    {
        BankLoan good = new BankLoan(1000, 0.01, 12, 1120);
        Console.WriteLine("Accepted: " + DoubleText.Of(good.TotalRepayment));

        try
        {
            new BankLoan(1000, 0.01, 12, 1100);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Rejected: " + e.Message);
        }
    }
}
