namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

/// <summary>A <b>ConcreteHandler</b>: a button, with or without its own help topic.</summary>
public sealed class Button : Widget
{
    public Button(Widget parent, string? topic) : base(parent, topic)
    {
    }

    public Button(Widget parent) : this(parent, null)
    {
    }
}
