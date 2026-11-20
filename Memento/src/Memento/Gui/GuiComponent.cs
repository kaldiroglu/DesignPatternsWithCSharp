using System.Globalization;

namespace dev.kaldiroglu.Memento.Gui;

/// <summary>
/// The <b>Originator</b>: a window with a position and a size. Its state is one object,
/// <see cref="GuiComponentState"/>. <see cref="CreateMemento"/> puts the current state in a
/// memento, and <see cref="Restore"/> reads it back. Only this class can do either.
/// </summary>
public class GuiComponent
{
    /// <summary>
    /// The <b>Memento</b>, as the caretaker sees it. It has no members: the caretaker can keep
    /// it and pass it back, but cannot read it, change it or make one.
    /// </summary>
    /// <remarks>
    /// An enclosing class cannot read a nested class's private members. So the caretaker gets
    /// this empty interface, and the class that holds the state, <see cref="Memento"/>, is
    /// private to the window.
    /// </remarks>
    public interface IMemento
    {
    }

    /// <summary>The state inside the memento. The class is private, so only the window can create it and read it.</summary>
    private sealed class Memento(GuiComponentState state) : IMemento
    {
        public GuiComponentState State { get; } = state;
    }

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
    public IMemento CreateMemento() => new Memento(new GuiComponentState(x, y, length, width));

    /// <summary>
    /// Takes the state back from a memento this window's class made. A memento of any other
    /// class throws <see cref="InvalidCastException"/>.
    /// </summary>
    public void Restore(IMemento memento)
    {
        GuiComponentState state = ((Memento)memento).State;
        x = state.X;
        y = state.Y;
        length = state.Length;
        width = state.Width;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"GuiComponent [name={Name}, x={x}, y={y}, length={length}, width={width}]");
}
