namespace dev.kaldiroglu.Adapter.Electricity.Domain.Tr;

public class TurkishPowerProvider : TurkishPowerSource
{
    public TurkishPowerProvider()
    {
        Console.WriteLine("TurkishPowerProvider is up and running.");
    }

    public void ProvidePowerAt220V()
    {
        Console.WriteLine("I provide electricity at 220V. Be careful, there may be some casual interruptions!");
    }

    public void TurnOn()
    {
        Console.WriteLine("TurkishPowerProvider started to provide electricity.");
    }

    public void TurnOff()
    {
        Console.WriteLine("TurkishPowerProvider stopped to provide electricity.");
    }
}
