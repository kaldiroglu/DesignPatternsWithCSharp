using System.Globalization;

namespace dev.kaldiroglu.Memento.Gui;

/// <summary>
/// The <b>Originator</b>: a window with a position and a size. <see cref="SaveState"/> gives
/// its state to the memento, and <see cref="Undo"/> takes it back.
/// </summary>
public class GuiComponent
{
    private GuiComponentMemento? memento;

    public GuiComponent(string name, int x, int y, int length, int width)
    {
        Name = name;
        X = x;
        Y = y;
        Length = length;
        Width = width;
    }

    public void SetMemento(GuiComponentMemento memento)
    {
        this.memento = memento;
    }

    public string Name { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Length { get; set; }

    public int Width { get; set; }

    /// <summary>Saves a new state object with the current values, so later changes cannot reach it.</summary>
    public void SaveState()
    {
        memento!.State = new GuiComponentState(X, Y, Length, Width);
    }

    public void Undo()
    {
        GuiComponentState state = memento!.State!;
        X = state.X;
        Y = state.Y;
        Length = state.Length;
        Width = state.Width;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"GuiComponent [name={Name}, x={X}, y={Y}, length={Length}, width={Width}]");
}
