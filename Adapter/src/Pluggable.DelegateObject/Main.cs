namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

public class Program
{
    public static void Main(string[] args)
    {
        HomeAppliance turkishHomeAppliance = new TurkishHomeAppliance("Iron");

        USPowerSource uSPowerSource = new USPowerSource();
        USPowerDelivery uSPowerDelivery = new USPowerDelivery(uSPowerSource);
        DelegatingPowerAdapter adapter1 = new DelegatingPowerAdapter(uSPowerDelivery);
        turkishHomeAppliance.SetPowerSource(adapter1);
        RunAppliance(turkishHomeAppliance);

        UKPowerSource uKPowerSource = new UKPowerSource();
        UKPowerDelivery uKPowerDelivery = new UKPowerDelivery(uKPowerSource);
        DelegatingPowerAdapter adapter2 = new DelegatingPowerAdapter(uKPowerDelivery);
        turkishHomeAppliance.SetPowerSource(adapter2);

        RunAppliance(turkishHomeAppliance);
    }

    static void RunAppliance(HomeAppliance turkishHomeAppliance)
    {
        Console.WriteLine("\n*** Starting ***");
        turkishHomeAppliance.Start();
        Console.WriteLine("\n*** Stopping ***");
        turkishHomeAppliance.Stop();
    }
}
