using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.Problem1;

public class Test
{
    public static void Main(string[] args)
    {
        TurkishPowerSource turkishPowerSource = new TurkishPowerProvider();

        // Can't have a reference of type Appliance anymore
        TurkishHomeAppliance turkishMixer = new TurkishHomeAppliance("Mixer");
        turkishMixer.SetPowerSource(turkishPowerSource);
        turkishMixer.Start();
        turkishMixer.Stop();

        Console.WriteLine();

        USPowerSource usPowerSource = new USPowerProvider();

        turkishMixer.SetUsPowerSource(usPowerSource);
        turkishMixer.Start();
        turkishMixer.Stop();
    }
}
