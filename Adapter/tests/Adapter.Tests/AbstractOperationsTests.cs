using Xunit;

using dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>Pluggable technique (a): the abstract PowerAdapter is a Template Method.</summary>
public class AbstractOperationsTests
{
    /// <summary>A subclass plug that records how often the narrow-interface hooks fire.</summary>
    private sealed class RecordingAdapter : PowerAdapter   // internal, reachable via InternalsVisibleTo
    {
        public int Delivered;
        public int CutCount;

        protected override void Deliver() => Delivered++;
        protected override void Cut() => CutCount++;
    }

    [Fact]
    public void TurnOnAndOff_RouteToTheAbstractHooks()
    {
        var adapter = new RecordingAdapter();

        adapter.TurnOn();
        adapter.TurnOff();

        Assert.Equal(1, adapter.Delivered);
        Assert.Equal(1, adapter.CutCount);
    }
}
