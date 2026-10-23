using dev.kaldiroglu.Mediator.Chat.Problem.Bus;

namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// Sends the same private message from Elif to Mert two ways: through the message bus of
/// stage three, and through the chat room.
/// <para>
/// The promise: a private message is seen only by the person it is sent to. Elif, Burak and
/// Mert are on the team; Can joins as a guest. Then Mert blocks Burak, and Burak says
/// something to everyone.
/// </para>
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat-two-ways</c>.
/// </remarks>
public static class TwoWaysMain
{
    public static void Run()
    {
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
        Member elif = new Member("Elif", room);
        Member burak = new Member("Burak", room);
        Member mert = new Member("Mert", room);
        Guest can = new Guest("Can", room);
        elif.Whisper("Mert", "Your review is late.");
        Console.WriteLine("Chat room: Elif sends Mert a private message.");
        Console.WriteLine("  delivered to: " + Show(room.Deliveries));
        Console.WriteLine("  Mert shows:   " + Show(mert.Shown));
        Console.WriteLine("  Can shows:    " + Show(can.Shown));

        mert.Block("Burak");
        burak.Say("Lunch at one?");
        Console.WriteLine("Mert blocks Burak; Burak says something to everyone.");
        Console.WriteLine("  Elif shows:   " + Show(elif.Shown));
        Console.WriteLine("  Mert shows:   " + Show(mert.Shown));
        Console.WriteLine("  Can shows:    " + Show(can.Shown));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
