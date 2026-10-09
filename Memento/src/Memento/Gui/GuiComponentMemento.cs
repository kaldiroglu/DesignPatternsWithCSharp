namespace dev.kaldiroglu.Memento.Gui;

/// <summary>The <b>Memento</b>: holds a <see cref="GuiComponentState"/>.</summary>
public class GuiComponentMemento
{
    // GuiComponent.SaveState() gives it a new state object each time, so later changes to the
    // window cannot reach it.
    public GuiComponentState? State { get; set; }
}
