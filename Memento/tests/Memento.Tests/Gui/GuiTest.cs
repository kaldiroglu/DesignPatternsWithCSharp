namespace dev.kaldiroglu.Memento.Tests.Gui;

using dev.kaldiroglu.Memento.Gui;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;
using static dev.kaldiroglu.Memento.Tests.Printed;

/// <summary>The window whose state is one object, kept by a memento.</summary>
public class GuiTest
{
    /// <summary>Restore puts the window back where it was saved, although x and length changed after.</summary>
    [Fact]
    public void RestoreGoesBackToTheSavedState()
    {
        GuiComponent window = new GuiComponent("window", 0, 0, 20, 10);
        GuiComponentMemento saved = window.CreateMemento();
        window.MoveTo(20, 0);
        window.Resize(40, 10);
        window.Restore(saved);
        Assert.Equal("GuiComponent [name=window, x=0, y=0, length=20, width=10]", window.ToString());
    }

    /// <summary>The caretaker can keep two mementos, and the window can go back to either.</summary>
    [Fact]
    public void TwoMementos()
    {
        GuiComponent window = new GuiComponent("window", 0, 0, 20, 10);
        GuiComponentMemento first = window.CreateMemento();
        window.MoveTo(20, 0);
        GuiComponentMemento second = window.CreateMemento();
        window.MoveTo(40, 0);

        window.Restore(first);
        Assert.Equal("GuiComponent [name=window, x=0, y=0, length=20, width=10]", window.ToString());
        window.Restore(second);
        Assert.Equal("GuiComponent [name=window, x=20, y=0, length=20, width=10]", window.ToString());
    }

    /// <summary>The window has no public setters: it changes only through MoveTo and Resize.</summary>
    [Fact]
    public void NoPublicSetters()
    {
        Assert.Empty(PublicSettersOf(typeof(GuiComponent)));
        Assert.Equal(new[] { "CreateMemento", "MoveTo", "Resize", "Restore", "ToString", "get_Name" },
            PublicMethodsOf(typeof(GuiComponent)).Order(StringComparer.Ordinal));
    }

    /// <summary>The window holds no memento: the caretaker keeps it.</summary>
    [Fact]
    public void TheWindowHoldsNoMemento()
    {
        var fields = typeof(GuiComponent).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        Assert.DoesNotContain(fields, f => f.FieldType == typeof(GuiComponentMemento));
    }

    /// <summary>
    /// A saved state cannot be changed, but anyone can read it. The accessor of the property
    /// State is the method get_State.
    /// </summary>
    [Fact]
    public void TheMementoIsReadable()
    {
        Assert.Equal(new[] { "get_State" }, PublicMethodsOf(typeof(GuiComponentMemento)));
    }

    /// <summary>Main.Run prints the window before, after the change, and after undo.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "GuiComponent [name=window, x=0, y=0, length=20, width=10]",
            "GuiComponent [name=window, x=20, y=0, length=40, width=10]",
            "GuiComponent [name=window, x=0, y=0, length=20, width=10]"
        }, By(global::dev.kaldiroglu.Memento.Gui.Main.Run));
    }
}
