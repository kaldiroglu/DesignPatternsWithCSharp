using System.Globalization;

namespace dev.kaldiroglu.Mediator.Chat.Problem.Directory;

/// <summary>
/// Three members share one directory, and Mert blocks Burak. The block works, but only
/// because every sender checks it in its own sending code.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat-directory</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        // Inside this namespace the plain name Directory means the example's class, not
        // System.IO.Directory.
        Directory directory = new Directory();
        Member elif = new Member("Elif", directory);
        Member burak = new Member("Burak", directory);
        Member mert = new Member("Mert", directory);
        mert.Block("Burak");
        burak.Say("Lunch at one?");
        burak.Whisper("Mert", "Are you there?");
        elif.Whisper("Mert", "Your review is late.");
        Console.WriteLine("Members in the directory: "
                + directory.Members.Count.ToString(CultureInfo.InvariantCulture));
        Console.WriteLine("Elif's inbox: " + Show(elif.Inbox));
        Console.WriteLine("Mert's inbox: " + Show(mert.Inbox) + "  (Burak is blocked)");
        Console.WriteLine("The block is checked in Member.say and Member.whisper, by the sender.");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
