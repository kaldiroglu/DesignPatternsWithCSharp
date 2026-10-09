namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

/// <summary>
/// The <b>Handler</b>: GoF's <c>HelpHandler</c>. It has a help topic or not, and a
/// successor.
/// <para>
/// If it has a topic, it shows the help; if not, it passes the request to its successor.
/// GoF implementation issue 1 (implementing the successor chain): here the chain is not a
/// new set of links — each widget's parent is its successor, so the chain is the window's
/// own containment.
/// </para>
/// </summary>
public abstract class HelpHandler
{
    private readonly HelpHandler? successor;
    private readonly string? topic;

    protected HelpHandler(HelpHandler? successor, string? topic)
    {
        this.successor = successor;
        this.topic = topic;
    }

    public bool HasHelp() => topic != null;

    public string HandleHelp()
    {
        if (HasHelp())
        {
            return "Help: " + topic;
        }
        if (successor != null)
        {
            return successor.HandleHelp();
        }
        return "No help is available.";
    }
}
