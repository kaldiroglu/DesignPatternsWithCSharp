namespace dev.kaldiroglu.Memento.Gui;

/// <summary>The state of a <see cref="GuiComponent"/>, as one object: its position and size.</summary>
public class GuiComponentState
{
    public GuiComponentState(int x, int y, int length, int width)
    {
        X = x;
        Y = y;
        Length = length;
        Width = width;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public int Length { get; set; }

    public int Width { get; set; }
}
