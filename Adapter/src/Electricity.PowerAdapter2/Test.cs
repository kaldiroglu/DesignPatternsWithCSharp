using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.PowerAdapter2;

public class Test
{
    public static void Main(string[] args)
    {
        USPowerSource usPowerSource = new USPowerProvider();
        USTurkishPowerAdapter uSTurkishPowerAdapter = new USTurkishPowerAdapter(usPowerSource);

        Console.WriteLine();

        Appliance turkishMixer = new TurkishHomeAppliance("Mixer");
        turkishMixer.SetPowerSource(uSTurkishPowerAdapter);
        turkishMixer.Start();
        turkishMixer.Stop();

        Console.WriteLine();

        Appliance turkishBroom = new TurkishHomeAppliance("Broom");
        turkishBroom.SetPowerSource(uSTurkishPowerAdapter);
        turkishBroom.Start();
        turkishBroom.Stop();
    }
}
