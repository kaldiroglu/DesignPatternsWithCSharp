using Xunit;

using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;
using dev.kaldiroglu.Adapter.Electricity.TwoWayAdapter;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>Two-way adapter: usable from the US side and the Turkish side.</summary>
public class TwoWayAdapterTests
{
    private sealed class RecordingUsSource : USPowerSource
    {
        public int Pushes;
        public void ProvidePowerAt110V() { }
        public void PushSwitch() => Pushes++;
    }

    private sealed class RecordingTrSource : TurkishPowerSource
    {
        public int Ons;
        public int Offs;
        public void ProvidePowerAt220V() { }
        public void TurnOn() => Ons++;
        public void TurnOff() => Offs++;
    }

    [Fact]
    public void UsSide_TurnOn_PushesTheUsSwitch()
    {
        var us = new RecordingUsSource();
        var adapter = new TwoWayUSTurkishPowerAdapter(us);   // built from a US source

        ((TurkishPowerSource)adapter).TurnOn();

        Assert.Equal(1, us.Pushes);
    }

    [Fact]
    public void TrSide_PushSwitch_TurnsOnTheTurkishSource()
    {
        var tr = new RecordingTrSource();
        var adapter = new TwoWayUSTurkishPowerAdapter(tr);   // built from a Turkish source

        ((USPowerSource)adapter).PushSwitch();

        Assert.Equal(1, tr.Ons);
    }
}
