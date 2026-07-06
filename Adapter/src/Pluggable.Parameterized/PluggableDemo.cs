namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;

/// <summary>
/// Runnable demo for both pluggable-adapter domains. In each, ONE adapter class serves TWO
/// differently-shaped adaptees, with the adaptation injected as lambdas (GoF technique (c)).
/// </summary>
public sealed class PluggableDemo
{
    private PluggableDemo()
    {
    }

    public static void Run()
    {
        USPowerSource us = new USPowerSource();
        TurkishPowerSource fromUS = new PluggablePowerAdapter(us.PushSwitch, us.PushSwitch);

        UKPowerSource uk = new UKPowerSource();
        TurkishPowerSource fromUK = new PluggablePowerAdapter(uk.FlipToggle, uk.FlipToggle);

        KenyaPowerSource kenya = new KenyaPowerSource();
        TurkishPowerSource fromKenya = new PluggablePowerAdapter(kenya.HakunaMatata, kenya.HakunaMatata);

        foreach (TurkishPowerSource source in new[] { fromUS, fromUK, fromKenya })
        {
            source.TurnOn();
            source.TurnOff();
            Console.WriteLine();
        }
    }
}
