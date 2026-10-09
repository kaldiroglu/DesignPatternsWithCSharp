namespace dev.kaldiroglu.Command.Tests.Ac.Lambda;

using System.Reflection;
using dev.kaldiroglu.Command.Ac.Lambda;
using Xunit;

/// <summary>The air conditioner's switch with its requests as functions. Ported from the Java <c>LambdaSwitchTest</c>.</summary>
public class LambdaSwitchTests
{
    [Fact(DisplayName = "the switch holds four functions and no Command")]
    public void FourFunctions()
    {
        var types = typeof(ACSwitch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Select(f => f.FieldType).ToList();
        Assert.Equal(3, types.Count(t => t == typeof(Action<dev.kaldiroglu.Command.Ac.Temperature>)));
        Assert.Equal(1, types.Count(t => t == typeof(Action)));
        Assert.DoesNotContain(typeof(dev.kaldiroglu.Command.Ac.ICommand), types);
    }

    [Fact(DisplayName = "the steps of Person print the same lines through either switch")]
    public void SameOutputAsTheCommandSwitch()
    {
        Assert.Equal(Printed.By(dev.kaldiroglu.Command.Ac.Person.Run), Printed.By(Main.Run));
    }

    [Fact(DisplayName = "turning on below the room temperature starts the cooler")]
    public void TurnOnCools()
    {
        var acSwitch = new ACSwitch();
        Assert.Equal(["", "Fan is turned on. Target temperature is: 20",
            "Cooler is turned on. Target temperature is: 20"], Printed.By(() => acSwitch.TurnOn(20)));
    }
}
