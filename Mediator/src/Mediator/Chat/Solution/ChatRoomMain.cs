namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// The pattern on its own. Elif sends Mert a private message, then Mert blocks Burak and
/// Burak says something to everyone. The room decides who receives each message, and this
/// class prints what every participant has after each step.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat-room</c>.
/// </remarks>
public static class ChatRoomMain
{
    public static void Run()
    {
        ChatRoom room = new ChatRoom();
        Member elif = new Member("Elif", room);
        Member burak = new Member("Burak", room);
        Member mert = new Member("Mert", room);
        Guest can = new Guest("Can", room);

        elif.Whisper("Mert", "Your review is late.");
        Console.WriteLine("Elif sends Mert a private message:");
        Print(room, elif, burak, mert, can);

        mert.Block("Burak");
        burak.Say("Lunch at one?");
        Console.WriteLine("Mert blocks Burak; Burak says something to everyone:");
        Print(room, elif, burak, mert, can);
    }

    private static void Print(ChatRoom room, Member elif, Member burak, Member mert, Guest can)
    {
        Console.WriteLine("delivered to " + Show(room.Deliveries) + "\nElif shows " + Show(elif.Shown)
                + "\nBurak shows " + Show(burak.Shown) + "\nMert shows " + Show(mert.Shown)
                + "\nCan shows " + Show(can.Shown) + "\n");
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
