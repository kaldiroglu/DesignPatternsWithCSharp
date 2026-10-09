namespace dev.kaldiroglu.Mediator.Chat.Problem.Bus;

/// <summary>
/// Elif sends a private message to Mert over the bus. The bus gives it to every client;
/// the team clients hide it, and the guest client shows it to Can.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat-bus</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        MessageBus bus = new MessageBus();
        ChatClient elif = new ChatClient("Elif"), burak = new ChatClient("Burak"), mert = new ChatClient("Mert");
        GuestClient can = new GuestClient("Can");
        foreach (ChatClient client in new[] { elif, burak, mert })
        {
            bus.Subscribe(client);
        }
        bus.Subscribe(can);
        bus.Publish(new Message("Elif", "Mert", "Your review is late."));
        Console.WriteLine("Delivered to: " + Show(bus.Deliveries));
        Console.WriteLine("Mert shows:   " + Show(mert.Shown));
        Console.WriteLine("Burak shows:  " + Show(burak.Shown));
        Console.WriteLine("Can shows:    " + Show(can.Shown) + "  (a private message for Mert)");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
