using System.Globalization;

namespace dev.kaldiroglu.Memento.Gui;

/// <summary>
/// The <b>Originator</b>: a window with a position and a size. <see cref="CreateMemento"/>
/// puts its state in a memento, and <see cref="Restore"/> takes it back.
/// </summary>
public class GuiComponent
{
    private int x;
    private int y;
    private int length;
    private int width;

    public GuiComponent(string name, int x, int y, int length, int width)
    {
        Name = name;
        this.x = x;
        this.y = y;
        this.length = length;
        this.width = width;
    }

    public string Name { get; }

    /// <summary>Moves the window. The only way to change its position.</summary>
    public void MoveTo(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>Changes the window's size. The only way to change its length and width.</summary>
    public void Resize(int length, int width)
    {
        this.length = length;
        this.width = width;
    }

    /// <summary>Puts the current values in a new memento, so later changes cannot reach it.</summary>
    public GuiComponentMemento CreateMemento() => new(new GuiComponentState(x, y, length, width));

    public void Restore(GuiComponentMemento memento)
    {
        GuiComponentState state = memento.State;
        x = state.X;
        y = state.Y;
        length = state.Length;
        width = state.Width;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"GuiComponent [name={Name}, x={x}, y={y}, length={length}, width={width}]");
}
