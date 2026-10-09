using System.Globalization;
using dev.kaldiroglu.Mediator.Chat.Problem.Bus;

namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// Runs the chat through stage one, stage three and the chat room.
/// <para>
/// The promise: a private message is seen only by the person it is sent to. Elif, Burak and
/// Mert are on the team; Can joins as a guest.
/// </para>
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        // stage one: everyone holds everyone
        // Inside this namespace the plain name Member means the room's member, so stage one's
        // member is written Problem.Direct.Member.
        Problem.Direct.Member elif = new("Elif"), burak = new("Burak"), mert = new("Mert"),
                selin = new("Selin");
        List<Problem.Direct.Member> team = [elif, burak, mert, selin];
        for (int i = 0; i < team.Count; i++)
        {
            for (int j = i + 1; j < team.Count; j++)
            {
                team[i].Meet(team[j]);
            }
        }
        int references = team.Sum(m => m.References);
        Console.WriteLine("Stage one: " + Number(team.Count) + " members hold " + Number(references)
                + " references to each other.");

        // stage three: a bus, and clients that filter
        MessageBus bus = new MessageBus();
        ChatClient elifClient = new("Elif"), burakClient = new("Burak"), mertClient = new("Mert");
        GuestClient canClient = new GuestClient("Can");
        foreach (ChatClient client in new[] { elifClient, burakClient, mertClient })
        {
            bus.Subscribe(client);
        }
        bus.Subscribe(canClient);
        bus.Publish(new Message("Elif", "Mert", "Your review is late."));
        Console.WriteLine("Stage three: Elif sends Mert a private message.");
        Console.WriteLine("  delivered to: " + Show(bus.Deliveries));
        Console.WriteLine("  Mert shows:   " + Show(mertClient.Shown));
        Console.WriteLine("  Burak shows:  " + Show(burakClient.Shown));
        Console.WriteLine("  Can shows:    " + Show(canClient.Shown));

        // the mediator
        ChatRoom room = new ChatRoom();
        Member elif2 = new Member("Elif", room);
        Member burak2 = new Member("Burak", room);
        Member mert2 = new Member("Mert", room);
        Guest can = new Guest("Can", room);
        elif2.Whisper("Mert", "Your review is late.");
        Console.WriteLine("Chat room: Elif sends Mert a private message.");
        Console.WriteLine("  delivered to: " + Show(room.Deliveries));
        Console.WriteLine("  Mert shows:   " + Show(mert2.Shown));
        Console.WriteLine("  Can shows:    " + Show(can.Shown));

        mert2.Block("Burak");
        burak2.Say("Lunch at one?");
        Console.WriteLine("Mert blocks Burak; Burak says something to everyone.");
        Console.WriteLine("  Elif shows:   " + Show(elif2.Shown));
        Console.WriteLine("  Mert shows:   " + Show(mert2.Shown));
        Console.WriteLine("  Can shows:    " + Show(can.Shown));
    }

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
