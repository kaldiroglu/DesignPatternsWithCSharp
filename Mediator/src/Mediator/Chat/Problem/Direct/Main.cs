using System.Globalization;

namespace dev.kaldiroglu.Mediator.Chat.Problem.Direct;

/// <summary>
/// Four members meet each other, then Can joins and only Elif is told. Burak's message never
/// reaches Can, because Burak's own list of members does not have him.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat-direct</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Member elif = new Member("Elif"), burak = new Member("Burak"),
                mert = new Member("Mert"), selin = new Member("Selin");
        List<Member> team = [elif, burak, mert, selin];
        for (int i = 0; i < team.Count; i++)
        {
            for (int j = i + 1; j < team.Count; j++)
            {
                team[i].Meet(team[j]);
            }
        }
        Console.WriteLine("References held by four members: "
                + team.Sum(m => m.References).ToString(CultureInfo.InvariantCulture));
        elif.Whisper(mert, "Your review is late.");
        Console.WriteLine("Mert's inbox:  " + Show(mert.Inbox));
        Console.WriteLine("Burak's inbox: " + Show(burak.Inbox));
        Member can = new Member("Can");
        elif.Meet(can);                          // only Elif is told about Can
        burak.Say("Lunch at one?");
        elif.Say("Hello");
        Console.WriteLine("Can's inbox:   " + Show(can.Inbox) + "  (Burak's lunch message is missing)");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
