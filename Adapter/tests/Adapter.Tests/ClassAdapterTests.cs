using Xunit;

using dev.kaldiroglu.Adapter.Electricity.ClassAdapter;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>Electricity class adapter: USTurkishPowerAdapter : USPowerSource, TurkishPowerSource.</summary>
public class ClassAdapterTests
{
    [Fact]
    public void TurnOnAndOff_ToggleTheInheritedSource()
    {
        var adapter = new USTurkishPowerAdapter();

        adapter.TurnOn();
        Assert.True(adapter.IsLive());

        adapter.TurnOff();
        Assert.False(adapter.IsLive());
    }

    [Fact]
    public void TurnOn_IsIdempotent()
    {
        var adapter = new USTurkishPowerAdapter();

        adapter.TurnOn();
        adapter.TurnOn();

        Assert.True(adapter.IsLive());
    }

    [Fact]
    public void Adapter_IsA_USPowerSource()
    {
        Assert.IsAssignableFrom<USPowerSource>(new USTurkishPowerAdapter());
    }
}
