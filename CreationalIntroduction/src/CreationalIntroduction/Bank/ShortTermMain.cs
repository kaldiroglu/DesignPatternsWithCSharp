namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>A check that runs before the base constructor.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- bank-short-term</c>.
/// </remarks>
public static class ShortTermMain
{
    public static void Run()
    {
        Console.WriteLine("Months: " + new ShortTerm(6).Months);

        try
        {
            new ShortTerm(36);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Rejected: " + e.Message);
        }
    }
}
