using Xunit;

using dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>Pluggable technique (b): DelegatingPowerAdapter forwards to a swappable delegate.</summary>
public class DelegateObjectTests
{
    private sealed class RecordingDelivery : PowerDelivery
    {
        public int Delivered;
        public int CutCount;

        public void Deliver() => Delivered++;
        public void Cut() => CutCount++;
    }

    [Fact]
    public void TurnOnAndOff_ForwardToTheDelegate()
    {
        var delivery = new RecordingDelivery();
        TurkishPowerSource adapter = new DelegatingPowerAdapter(delivery);

        adapter.TurnOn();
        adapter.TurnOff();

        Assert.Equal(1, delivery.Delivered);
        Assert.Equal(1, delivery.CutCount);
    }
}
