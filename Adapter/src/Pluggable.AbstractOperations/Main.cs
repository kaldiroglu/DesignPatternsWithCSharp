namespace dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

public class Program
{
    public static void Main(string[] args)
    {
        HomeAppliance turkishHomeAppliance = new TurkishHomeAppliance("Iron");

        USPowerSource uSPowerSource = new USPowerSource();
        PowerAdapter adapter1 = new USTurkishPowerAdapter(uSPowerSource);
        turkishHomeAppliance.SetPowerSource(adapter1);
        RunAppliance(turkishHomeAppliance);

        UKPowerSource ukPowerSource = new UKPowerSource();
        PowerAdapter adapter2 = new UKTurkishPowerAdapter(ukPowerSource);
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
