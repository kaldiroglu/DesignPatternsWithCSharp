namespace dev.kaldiroglu.Command.Ac;

/// <summary>A person presses the buttons of the wall switch.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- ac</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        new Person(new ACSwitch()).Action();
    }
}
