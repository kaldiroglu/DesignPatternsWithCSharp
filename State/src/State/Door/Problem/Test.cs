namespace dev.kaldiroglu.State.Door.Problem;

/// <summary>
/// Opens and closes the door eight times and prints whether it is open after each step.
/// <para>
/// The Java original's <c>main</c>. C# prints a <c>bool</c> as <c>True</c> or <c>False</c>,
/// so <see cref="Show"/> prints it in lower case, as Java does.
/// </para>
/// </summary>
public static class Test
{
    public static void Run()
    {
        Door door = new Door(true);
        Console.WriteLine("Initial: " + Show(door.IsOpen));
        door.Close();
        Console.WriteLine("After close(): " + Show(door.IsOpen));
        door.Open();
        Console.WriteLine("After open(): " + Show(door.IsOpen));
        door.Open();
        Console.WriteLine("After open(): " + Show(door.IsOpen));
        door.Close();
        Console.WriteLine("After close(): " + Show(door.IsOpen));
        door.Open();
        Console.WriteLine("After open(): " + Show(door.IsOpen));
        door.Close();
        Console.WriteLine("After close(): " + Show(door.IsOpen));
        door.Close();
        Console.WriteLine("After close(): " + Show(door.IsOpen));
    }

    private static string Show(bool value) => value ? "true" : "false";
}
