using Xunit;

using dev.kaldiroglu.Adapter.Pluggable.Parameterized;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>Pluggable technique (c): PluggablePowerAdapter is parameterized with Action blocks.</summary>
public class ParameterizedTests
{
    [Fact]
    public void TurnOnAndOff_RunTheInjectedActions()
    {
        int on = 0;
        int off = 0;
        TurkishPowerSource adapter = new PluggablePowerAdapter(() => on++, () => off++);

        adapter.TurnOn();
        adapter.TurnOn();
        adapter.TurnOff();

        Assert.Equal(2, on);
        Assert.Equal(1, off);
    }
}
