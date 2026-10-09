namespace dev.kaldiroglu.Observer.Hw.Inbox;

/// <summary>
/// Three views of one inbox, each a lambda. The inbox pushes the new message; the badge
/// also pulls the unread count, which is not in the message.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- hw-inbox</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var inbox = new Inbox();
        var subjects = new List<string>();
        inbox.OnNewMessage(message => Console.WriteLine("Badge: " + inbox.Unread + " unread"));
        inbox.OnNewMessage(message => subjects.Add(message.Subject));
        inbox.OnNewMessage(message => Console.WriteLine("Pop-up: new message from " + message.From));
        inbox.Receive(new Inbox.Message("Ayse", "Meeting at ten"));
        inbox.Receive(new Inbox.Message("Deniz", "October sales"));
        Console.WriteLine("List: " + Show(subjects));
        inbox.ReadAll();
        Console.WriteLine("After reading all: " + inbox.Unread + " unread");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
