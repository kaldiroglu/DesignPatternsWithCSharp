using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;

namespace dev.kaldiroglu.Adapter.Electricity.Domain.Tr;

public class Test
{
    public static void Main(string[] args)
    {
        TurkishPowerSource turkishPowerSource = new TurkishPowerProvider();

        Appliance turkishMixer = new TurkishHomeAppliance("Mixer");
        turkishMixer.SetPowerSource(turkishPowerSource);
        turkishMixer.Start();
        turkishMixer.Stop();

        Console.WriteLine();

        Appliance turkishBroom = new TurkishHomeAppliance("Broom");
        turkishBroom.SetPowerSource(turkishPowerSource);
        turkishBroom.Start();
        turkishBroom.Stop();
    }
}
