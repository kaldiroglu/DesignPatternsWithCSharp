using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.TwoWayAdapter;

public class Test
{
    public static void Main(string[] args)
    {
        // In US with Turkish home appliance
        USPowerSource usPowerSource = new USPowerProvider();
        TwoWayUSTurkishPowerAdapter twoWayUSTurkishPowerAdapter1 = new TwoWayUSTurkishPowerAdapter(usPowerSource);

        Console.WriteLine();

        Appliance turkishMixer = new TurkishHomeAppliance("Mixer");
        turkishMixer.SetPowerSource(twoWayUSTurkishPowerAdapter1);
        turkishMixer.Start();
        turkishMixer.Stop();

        Console.WriteLine();

        // In Turkey with US home appliance
        TurkishPowerSource turkishPowerSource = new TurkishPowerProvider();
        TwoWayUSTurkishPowerAdapter twoWayUSTurkishPowerAdapter2 = new TwoWayUSTurkishPowerAdapter(turkishPowerSource);

        Console.WriteLine();

        USHomeApplicance usBroom = new USHomeApplicance("Broom");
        usBroom.SetPowerSource(twoWayUSTurkishPowerAdapter2);
        usBroom.Start();
        usBroom.Stop();
    }
}
