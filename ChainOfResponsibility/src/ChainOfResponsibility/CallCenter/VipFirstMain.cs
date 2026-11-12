namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>
/// The same desks and customers as <see cref="Main"/>, with the VIP desk first. The VIP desk
/// serves every customer, so it answers all three calls and the other desks receive none.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/ChainOfResponsibility.Demo -- callcenter-vip-first</c>.
/// </remarks>
public class VipFirstMain
{
    public static void Run()
    {
        ICallTaker first = new VipCallTaker(new GoldCallTaker(new StandardCallTaker(null)));
        foreach (ICustomer customer in Main.Customers())
        {
            first.Answer(customer);
            Console.WriteLine();
        }
    }
}
