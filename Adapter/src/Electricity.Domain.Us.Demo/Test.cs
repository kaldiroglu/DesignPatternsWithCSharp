using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.Domain.Us;

public class Test
{
    public static void Main(string[] args)
    {
        USPowerSource usPowerSource = new USPowerProvider();

        USHomeApplicance usMixer = new USHomeApplicance("Mixer");
        usMixer.SetPowerSource(usPowerSource);
        usMixer.Start();
        usMixer.Stop();
    }
}
