namespace dev.kaldiroglu.Memento.Tests.Gui;

using dev.kaldiroglu.Memento.Gui;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;
using static dev.kaldiroglu.Memento.Tests.Printed;

/// <summary>The window whose state is one object, kept by a memento.</summary>
public class GuiTest
{
    /// <summary>Undo puts the window back where it was saved, although x and length changed after.</summary>
    [Fact]
    public void UndoRestoresTheSavedState()
    {
        GuiComponent window = new GuiComponent("window", 0, 0, 20, 10);
        window.SetMemento(new GuiComponentMemento());
        window.SaveState();
        window.X = 20;
        window.Length = 40;
        window.Undo();
        Assert.Equal("GuiComponent [name=window, x=0, y=0, length=20, width=10]", window.ToString());
    }

    /// <summary>
    /// The memento has a public State with a getter and a setter, so anyone can read the
    /// window's state through it. The Java methods getState and setState are the C# property
    /// State, whose accessors are the methods get_State and set_State.
    /// </summary>
    [Fact]
    public void TheMementoIsOpen()
    {
        Assert.Equal(new[] { "get_State", "set_State" }, PublicMethodsOf(typeof(GuiComponentMemento)));
    }

    /// <summary>Test.Run prints the window before, after the change, and after undo.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "GuiComponent [name=window, x=0, y=0, length=20, width=10]",
            "GuiComponent [name=window, x=20, y=0, length=40, width=10]",
            "GuiComponent [name=window, x=0, y=0, length=20, width=10]"
        }, By(global::dev.kaldiroglu.Memento.Gui.Test.Run));
    }
}
