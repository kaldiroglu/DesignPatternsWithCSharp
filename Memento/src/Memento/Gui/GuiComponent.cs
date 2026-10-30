using System.Globalization;

namespace dev.kaldiroglu.Memento.Gui;

/// <summary>
/// The <b>Originator</b>: a window with a position and a size. <see cref="CreateMemento"/>
/// puts its state in a memento, and <see cref="Restore"/> takes it back.
/// </summary>
public class GuiComponent
{
    public GuiComponent(string name, int x, int y, int length, int width)
    {
        Name = name;
        X = x;
        Y = y;
        Length = length;
        Width = width;
    }

    public string Name { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Length { get; set; }

    public int Width { get; set; }

    /// <summary>Puts the current values in a new memento, so later changes cannot reach it.</summary>
    public GuiComponentMemento CreateMemento() => new(new GuiComponentState(X, Y, Length, Width));

    public void Restore(GuiComponentMemento memento)
    {
        GuiComponentState state = memento.State;
        X = state.X;
        Y = state.Y;
        Length = state.Length;
        Width = state.Width;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"GuiComponent [name={Name}, x={X}, y={Y}, length={Length}, width={Width}]");
}
