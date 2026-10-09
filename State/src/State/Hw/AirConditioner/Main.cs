namespace dev.kaldiroglu.State.Hw.AirConditioner;

/// <summary>
/// Turns an air conditioner on in a warm room, lets the room cool down, sets a higher
/// target and turns it off. The states choose the next state from the room and the target.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- hw-airconditioner</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var ac = new AirConditioner(26);
        ac.PowerOn();
        Console.WriteLine("On in a 26 degree room, target 22: " + ac.State);
        ac.RoomIs(22);
        Console.WriteLine("The room is now 22:                " + ac.State);
        ac.SetTarget(24);
        Console.WriteLine("The target is now 24:              " + ac.State);
        ac.PowerOff();
        ac.SetTarget(20);
        Console.WriteLine("Off, then the target is set to 20: " + ac.State);
        Console.WriteLine("Log: " + Show(ac.Log));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
