namespace dev.kaldiroglu.Adapter.Electricity.ClassAdapter;

/// <summary>
/// Runnable demo for the electricity <b>class adapter</b>. The appliance drives a US source through
/// the Turkish interface without ever knowing the adaptee's type.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        // The class adapter presents a US source as a Turkish one.
        TurkishPowerSource adapter = new USTurkishPowerAdapter();

        TurkishHomeAppliance mixer = new TurkishHomeAppliance("Mixer", adapter);
        mixer.Start();
        mixer.Stop();

        Console.WriteLine();

        // Because the class adapter IS-A USPowerSource, it can also be used as one directly.
        USTurkishPowerAdapter asUsSource = new USTurkishPowerAdapter();
        Console.WriteLine("Used as a raw US source; isLive=" + asUsSource.IsLive());
        asUsSource.PushSwitch();
        Console.WriteLine("after pushSwitch(); isLive=" + asUsSource.IsLive());
    }
}
