namespace dev.kaldiroglu.Memento.Gui;

/// <summary>
/// Holds one saved state. Its <see cref="State"/> is public, so any class can read the
/// window's state through it; <c>Game.Solution.Player.Checkpoint</c> closes that.
/// </summary>
public class GuiComponentMemento(GuiComponentState state)
{
    public GuiComponentState State { get; } = state;
}
