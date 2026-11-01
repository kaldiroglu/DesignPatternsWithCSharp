namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>A loan that calculates its own total.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- bank-secure-loan</c>.
/// </remarks>
public static class SecureLoanMain
{
    public static void Run()
    {
        SecureLoan loan = SecureLoan.Create(1000, 0.01, 12);
        Console.WriteLine("Total: " + DoubleText.Of(loan.TotalRepayment));

        try
        {
            SecureLoan.Create(-1000, 0.01, 12);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Rejected: " + e.Message);
        }
    }
}
