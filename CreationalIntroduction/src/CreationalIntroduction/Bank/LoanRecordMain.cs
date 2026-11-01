namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>A record that checks its arguments and calculates its total.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- bank-loan-record</c>.
/// </remarks>
public static class LoanRecordMain
{
    public static void Run()
    {
        LoanRecord loan = new LoanRecord(1000, 0.01, 12);
        Console.WriteLine(loan);
        Console.WriteLine("Total: " + DoubleText.Of(loan.TotalRepayment));

        try
        {
            new LoanRecord(1000, 0.01, 0);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Rejected: " + e.Message);
        }
    }
}
