using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.PowerAdapter1;

public class Test
{
    public static void Main(string[] args)
    {
        USPowerSource usPowerSource = new USPowerProvider();
        USTurkishPowerAdapter uSTurkishPowerAdapter = new USTurkishPowerAdapter(usPowerSource);

        Console.WriteLine();

        Appliance shaver = new TurkishHomeAppliance("Shaver");
        shaver.SetPowerSource(uSTurkishPowerAdapter);
        shaver.Start();
        shaver.Stop();

        Console.WriteLine();
        //
        //		Appliance turkishBroom = new TurkishHomeAppliance("Broom");
        //		turkishBroom.SetPowerSource(uSTurkishPowerAdapter);
        //		turkishBroom.Start();
        //		turkishBroom.Stop();
    }
}
