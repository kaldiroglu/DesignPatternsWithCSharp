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
        adapter.TurnOff();

        Assert.Equal(1, on);
        Assert.Equal(1, off);
    }

    [Fact]
    public void TwoOperationsStayCorrectOnOneToggle()
    {
        int pushes = 0;
        Action push = () => pushes++;
        TurkishPowerSource adapter = new PluggablePowerAdapter(push, push);

        adapter.TurnOn();
        adapter.TurnOn();
        Assert.Equal(1, pushes);

        adapter.TurnOff();
        adapter.TurnOff();
        Assert.Equal(2, pushes);

        adapter.TurnOff();
        Assert.Equal(2, pushes);
    }
}
