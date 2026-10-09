using System.Globalization;

namespace dev.kaldiroglu.Memento.Gui;

/// <summary>
/// The <b>Originator</b>: a window with a position and a size. <see cref="SaveState"/> gives
/// its state to the memento, and <see cref="Undo"/> takes it back.
/// </summary>
public class GuiComponent
{
    private GuiComponentMemento? memento;
    private GuiComponentState state;

    public GuiComponent(string name, int x, int y, int length, int width)
    {
        Name = name;
        X = x;
        Y = y;
        Length = length;
        Width = width;
        // NOTE: the state object is created once, here. The setters below do not update it,
        // so SaveState() always saves the starting values. Move the window to x=20, save,
        // move it to x=50 and undo: x is 0, not 20. The Java has the same behavior.
        state = new GuiComponentState(x, y, length, width);
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

    public void SaveState()
    {
        memento!.State = state;
    }

    public void Undo()
    {
        state = memento!.State!;
        X = state.X;
        Y = state.Y;
        Length = state.Length;
        Width = state.Width;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"GuiComponent [name={Name}, x={X}, y={Y}, length={Length}, width={Width}]");
}
