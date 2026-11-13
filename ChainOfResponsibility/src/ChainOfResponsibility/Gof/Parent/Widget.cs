namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Parent;

/// <summary>
/// A control on the screen: the editor, a dialog or a button.
/// <para>
/// Every widget keeps its parent, the widget that holds it. <see cref="Path"/> uses the
/// parent to say where the widget is. <see cref="HandleHelp"/> uses the same parent as the
/// next link of the help chain. No field is added for the chain.
/// </para>
/// </summary>
public sealed class Widget
{
    private readonly Widget? parent;
    private readonly string name;
    private readonly string? topic;

    public Widget(Widget? parent, string name, string? topic = null)
    {
        this.parent = parent;
        this.name = name;
        this.topic = topic;
    }

    public string Path()
    {
        if (parent == null)
            return name;
        return parent.Path() + " > " + name;
    }

    public string HandleHelp()
    {
        if (topic != null)
            return "Help: " + topic;
        if (parent != null)
            return parent.HandleHelp();
        return "No help is available.";
    }
}
