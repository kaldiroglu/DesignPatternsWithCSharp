namespace dev.kaldiroglu.Command.Tests.Ac;

using dev.kaldiroglu.Command.Ac;
using Xunit;

/// <summary>
/// The air conditioner and its wall switch. The room starts at 22 degrees. The classes print
/// to the console and have no getters, so the tests check what they print.
/// </summary>
public class AirConditionerTests
{
    [Fact(DisplayName = "turning on below the room temperature starts the cooler")]
    public void TurnOnCools()
    {
        var acSwitch = new ACSwitch();
        Assert.Equal(["", "Fan is turned on. Target temperature is: 20",
                "Cooler is turned on. Target temperature is: 20"],
            Printed.By(() => acSwitch.TurnOn(20)));
    }

    [Fact(DisplayName = "turning on above the room temperature starts the heater")]
    public void TurnOnHeats()
    {
        var acSwitch = new ACSwitch();
        Assert.Equal(["", "Fan is turned on. Target temperature is: 25",
                "Heater is turned on. Target temperature is: 25"],
            Printed.By(() => acSwitch.TurnOn(25)));
    }

    [Fact(DisplayName = "turning on at the room temperature starts only the fan")]
    public void TurnOnAtRoomTemperature()
    {
        var acSwitch = new ACSwitch();
        Assert.Equal(["", "Fan is turned on. Target temperature is: 22"],
            Printed.By(() => acSwitch.TurnOn(22)));
    }

    [Fact(DisplayName = "turning on twice, or off twice, says so")]
    public void OnTwiceOffTwice()
    {
        var acSwitch = new ACSwitch();
        Printed.By(() => acSwitch.TurnOn(22));
        Assert.Equal(["", "AirConditioner is already on!"], Printed.By(() => acSwitch.TurnOn(22)));
        Printed.By(acSwitch.TurnOff);
        Assert.Equal(["AirConditioner is already off!", ""], Printed.By(acSwitch.TurnOff));
    }

    [Fact(DisplayName = "turning off prints one line and an empty one")]
    public void TurnOff()
    {
        var acSwitch = new ACSwitch();
        Printed.By(() => acSwitch.TurnOn(22));
        Assert.Equal(["AirConditioner is turned off.", ""], Printed.By(acSwitch.TurnOff));
    }

    [Fact(DisplayName = "turning off keeps the room temperature: back on at 20, only the fan starts")]
    public void TurnOffKeepsTheRoomTemperature()
    {
        var acSwitch = new ACSwitch();
        acSwitch.TurnOn(20);                  // cools the room from 22 to 20
        Printed.By(acSwitch.TurnOff);
        Assert.Equal(["", "Fan is turned on. Target temperature is: 20"],
            Printed.By(() => acSwitch.TurnOn(20)));
    }

    [Fact(DisplayName = "the heater and the cooler need the air conditioner to be on")]
    public void HeaterAndCoolerNeedPower()
    {
        var acSwitch = new ACSwitch();
        var lines = Printed.By(() =>
        {
            acSwitch.TurnOnCooler(18);
            acSwitch.TurnOnHeater(25);
        });
        Assert.Equal(["AirConditioner is off, please first turn it on!",
            "AirConditioner is off, please first turn it on!"], lines);
    }

    [Fact(DisplayName = "the cooler only cools, and the heater only heats")]
    public void EachOnlyGoesOneWay()
    {
        var acSwitch = new ACSwitch();
        Printed.By(() => acSwitch.TurnOn(22));
        var lines = Printed.By(() =>
        {
            acSwitch.TurnOnCooler(25);   // warmer than the room
            acSwitch.TurnOnCooler(18);
            acSwitch.TurnOnHeater(15);   // colder than the room
            acSwitch.TurnOnHeater(23);
        });
        Assert.Equal(["The cooler is not needed: the room is at 22.",
            "Cooler is turned on. Target temperature is: 18",
            "The heater is not needed: the room is at 18.",
            "Heater is turned on. Target temperature is: 23"], lines);
    }

    [Fact(DisplayName = "each command passes its request to the air conditioner")]
    public void CommandsForward()
    {
        var ac = new AirConditioner(new Temperature(22));
        var lines = Printed.By(() =>
        {
            new TurnOnCommand(ac, new Temperature(22)).Execute();
            new HeatCommand(ac, new Temperature(24)).Execute();
            new CoolCommand(ac, new Temperature(19)).Execute();
            new TurnOffCommand(ac).Execute();
        });
        Assert.Equal(["", "Fan is turned on. Target temperature is: 22",
            "Heater is turned on. Target temperature is: 24",
            "Cooler is turned on. Target temperature is: 19",
            "AirConditioner is turned off.", ""], lines);
    }

    [Fact(DisplayName = "execute takes no argument: a command holds what it needs")]
    public void ACommandIsComplete()
    {
        Assert.Empty(typeof(ICommand).GetMethod(nameof(ICommand.Execute))!.GetParameters());
        // Creating the switch prints nothing.
        Assert.Empty(Printed.By(() => new ACSwitch()));
    }

    [Fact(DisplayName = "undo and redo are declared, not yet written: they do nothing")]
    public void UndoAndRedoDoNothingYet()
    {
        var ac = new AirConditioner(new Temperature(22));
        List<ICommand> all =
            [new TurnOnCommand(ac, new Temperature(22)), new TurnOffCommand(ac),
             new HeatCommand(ac, new Temperature(24)), new CoolCommand(ac, new Temperature(19))];
        var lines = Printed.By(() =>
        {
            foreach (var command in all)
            {
                command.Undo();
                command.Redo();
            }
        });
        Assert.Empty(lines);
    }
}
