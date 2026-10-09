namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>
/// Shows a remote whose undo puts back exactly what was there, even when a press changed
/// nothing.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- hw-remote</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var tv = new Television();
        var remote = RemoteControl.StandardFor(tv);

        remote.Press("on");
        remote.Press("7");
        Console.WriteLine("On, then channel 7. Channel: " + tv.Channel);
        remote.Undo();
        Console.WriteLine("Undo. Channel: " + tv.Channel
            + " (the command remembered the channel before)");

        for (var i = 0; i < 6; i++)
        {
            remote.Press("volume+");
        }
        Console.WriteLine("Volume up six times from 5. Volume: " + tv.Volume
            + " (the top is " + Television.MaxVolume + ")");
        remote.Undo();
        Console.WriteLine("Undo the last press, which changed nothing. Volume: " + tv.Volume);
        remote.Undo();
        Console.WriteLine("Undo once more. Volume: " + tv.Volume);

        remote.Press("off");
        remote.Undo();
        Console.WriteLine("Off, then undo. The television is on: " + (tv.IsOn ? "true" : "false"));
    }
}
