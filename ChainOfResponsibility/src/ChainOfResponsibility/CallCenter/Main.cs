namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>
/// Builds the chain standard, then gold, then VIP, and sends one customer of each kind to
/// the standard desk. Each customer is answered by the first desk that serves them.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/ChainOfResponsibility.Demo -- callcenter</c>.
/// </remarks>
public class Main
{
    public static void Run()
    {
        ICallTaker first = CreateCallTakers();
        foreach (ICustomer customer in Customers())
        {
            first.Answer(customer);
            Console.WriteLine();
        }
    }

    public static ICallTaker CreateCallTakers()
    {
        VipCallTaker vipCT = new VipCallTaker(null);
        GoldCallTaker goldCT = new GoldCallTaker(vipCT);
        StandardCallTaker standardCT = new StandardCallTaker(goldCT);
        return standardCT;
    }

    public static IReadOnlyList<ICustomer> Customers() =>
        [new StandardCustomer(), new GoldCustomer(), new VipCustomer()];
}
