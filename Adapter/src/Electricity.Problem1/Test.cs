using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.Problem1;

public class Test
{
    public static void Main(string[] args)
    {
        TurkishPowerSource turkishPowerSource = new TurkishPowerProvider();

        // Can't have a reference of type Appliance anymore
        TurkishHomeAppliance turkishShaver = new TurkishHomeAppliance("Shaver");
        turkishShaver.SetPowerSource(turkishPowerSource);
        turkishShaver.Start();
        turkishShaver.Stop();

        Console.WriteLine();

        USPowerSource usPowerSource = new USPowerProvider();

        turkishShaver.SetUsPowerSource(usPowerSource);
        turkishShaver.Start();
        turkishShaver.Stop();
    }
}
