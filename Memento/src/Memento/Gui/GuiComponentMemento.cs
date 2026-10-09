namespace dev.kaldiroglu.Memento.Gui;

/// <summary>The <b>Memento</b>: holds a <see cref="GuiComponentState"/>.</summary>
public class GuiComponentMemento
{
    // NOTE: the memento keeps the state object it is given, not a copy of it. The Java has the
    // same behavior.
    public GuiComponentState? State { get; set; }
}
