using Xunit;

using dev.kaldiroglu.Adapter.Electricity.Domain.Us;
using dev.kaldiroglu.Adapter.Electricity.PowerAdapter1;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>Object adapter: PowerAdapter1.USTurkishPowerAdapter wraps a USPowerSource.</summary>
public class PowerAdapter1Tests
{
    /// <summary>A test double that counts how often the adaptee's PushSwitch is called.</summary>
    private sealed class RecordingUsSource : USPowerSource
    {
        public int Pushes;

        public void ProvidePowerAt110V() { }

        public void PushSwitch() => Pushes++;
    }

    [Fact]
    public void TurnOnOff_ForwardToPushSwitch_AndTurnOnIsGuarded()
    {
        var us = new RecordingUsSource();
        var adapter = new USTurkishPowerAdapter(us);

        adapter.TurnOn();   // pushes -> 1
        adapter.TurnOn();   // already on: no-op
        adapter.TurnOff();  // pushes -> 2

        Assert.Equal(2, us.Pushes);
    }
}
