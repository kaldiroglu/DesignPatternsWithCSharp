namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>
/// Builds the chain standard, gold, VIP, creates five random customers and lets every one of
/// them call the standard desk.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. It chooses customers at random, so the output changes
/// from run to run. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- callcenter</c>.
/// </remarks>
public class Test
{
    private ICallTaker? first;
    private List<ICustomer> customers = [];

    public static void Run()
    {
        Test test = new Test();
        test.CreateCallTakers();
        test.CreateCustomers(5);
        test.StartTakingCalls();
    }

    public void CreateCallTakers()
    {
        VipCallTaker vipCT = new VipCallTaker(null);
        GoldCallTaker goldCT = new GoldCallTaker(vipCT);
        StandardCallTaker standardCT = new StandardCallTaker(goldCT);
        first = standardCT;
    }

    public void CreateCustomers(int count)
    {
        customers = [];
        for (int i = 0; i < count; i++)
        {
            double random = Random.Shared.NextDouble();
            if (random < 0.33)
            {
                StandardCustomer customer = new StandardCustomer();
                customers.Add(customer);
            }
            else if (random < 0.66)
            {
                GoldCustomer customer = new GoldCustomer();
                customers.Add(customer);
            }
            else
            {
                VipCustomer customer = new VipCustomer();
                customers.Add(customer);
            }
        }
    }

    private void StartTakingCalls()
    {
        foreach (ICustomer customer in customers)
        {
            first!.Answer(customer);
        }
    }
}
