namespace dev.kaldiroglu.Bridge.Notifications.Domain;

/// <summary>
/// Section headings for the demos, so each one stays about the design rather than layout, and
/// the two messages every demo sends.
/// <para>
/// Java calls this class <c>Console</c>. That name is taken in .NET, and shadowing
/// <see cref="System.Console"/> inside the very namespace that writes to it would be a trap
/// rather than a lesson. <c>Demo</c> is no better: the runnable project is
/// <c>dev.kaldiroglu.Bridge.Demo</c>, and inside it a bare <c>Demo</c> binds to the namespace.
/// </para>
/// </summary>
public static class Scenario
{
    public static void Heading(string title)
    {
        Console.WriteLine(new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    public static void Section(string title) =>
        Console.WriteLine($"\n--- {title} " + new string('-', Math.Max(0, 68 - title.Length)));

    /// <summary>The message every demo sends, so their outputs can be compared.</summary>
    public static Message ShortMessage() => new("Order 4021 shipped", "It is on its way.");

    /// <summary>Long enough that the channels disagree about what to do with it.</summary>
    public static Message LongMessage() => new("Payment failed",
        "We could not take payment for order 4021. Please update the card on file. "
        + "If the payment is not completed within 48 hours the order will be released "
        + "and the reserved stock returned to the warehouse for other customers.");
}
