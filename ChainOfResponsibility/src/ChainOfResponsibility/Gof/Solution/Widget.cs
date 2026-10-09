namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

/// <summary>
/// A control on the screen. Its successor is usually its parent widget, but any
/// <see cref="HelpHandler"/> can be the next link.
/// </summary>
public abstract class Widget : HelpHandler
{
    protected Widget(HelpHandler? successor, string? topic) : base(successor, topic)
    {
    }
}
