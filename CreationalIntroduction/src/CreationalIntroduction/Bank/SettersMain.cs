namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>A wrong total is stored, because setters check nothing.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- bank-setters</c>.
/// </remarks>
public static class SettersMain
{
    public static void Run()
    {
        LoanWithSetters loan = new LoanWithSetters();
        loan.Principal = 1000;
        loan.Rate = 0.01;
        loan.Months = 12;
        loan.TotalRepayment = 1100;   // the client's arithmetic is wrong

        Console.WriteLine("Total stored : " + DoubleText.Of(loan.TotalRepayment));
        Console.WriteLine("Correct total: " + DoubleText.Of(loan.CorrectTotal()));
    }
}
