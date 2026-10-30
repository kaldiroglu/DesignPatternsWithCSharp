namespace dev.kaldiroglu.Memento.Gui;

/// <summary>The state as one object. A record, so a saved state cannot be changed.</summary>
public sealed record GuiComponentState(int X, int Y, int Length, int Width);
