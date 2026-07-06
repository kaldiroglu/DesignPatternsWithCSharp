namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;

public class ApplianceDemo
{
    public static void Run()
    {
        HomeAppliance shaver = new TurkishHomeAppliance("Shaver");

        USPowerSource uSPowerSource = new USPowerSource();
        PluggablePowerAdapter adapter1 = new PluggablePowerAdapter(uSPowerSource.PushSwitch, uSPowerSource.PushSwitch);
        shaver.SetPowerSource(adapter1);
        RunAppliance(shaver);

        KenyaPowerSource kenyaPowerSource = new KenyaPowerSource();
        PluggablePowerAdapter adapter2 = new PluggablePowerAdapter(kenyaPowerSource.HakunaMatata, kenyaPowerSource.HakunaMatata);
        shaver.SetPowerSource(adapter2);
        RunAppliance(shaver);
    }

    static void RunAppliance(HomeAppliance turkishHomeAppliance)
    {
        Console.WriteLine("\n*** Starting ***");
        turkishHomeAppliance.Start();
        Console.WriteLine("\n*** Stopping ***");
        turkishHomeAppliance.Stop();
    }
}
