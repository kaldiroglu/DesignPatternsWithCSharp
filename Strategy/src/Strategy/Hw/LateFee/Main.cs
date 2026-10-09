namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>Charges the same late book under three fee rules, changed on one returns desk.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- hw-latefee</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var tenDaysLate = new Loan("Design Patterns", 10, 50);
        var desk = new ReturnsDesk(new StandardFee());

        IFeeRule[] rules = [new StandardFee(), new CappedFee(300), new GraceThenDouble(3)];
        Console.WriteLine("'" + tenDaysLate.Title + "', " + tenDaysLate.DaysLate
            + " days late, " + tenDaysLate.DailyRate + " a day:");
        foreach (var rule in rules)
        {
            desk.SetRule(rule);
            Console.WriteLine("  " + desk.RuleName + ": " + desk.Charge(tenDaysLate));
        }

        var twoDaysLate = new Loan("Design Patterns", 2, 50);
        Console.WriteLine("Two days late: STANDARD " + new StandardFee().Charge(twoDaysLate)
            + ", GRACE_THEN_DOUBLE " + new GraceThenDouble(3).Charge(twoDaysLate));
    }
}
