namespace dev.kaldiroglu.Memento.Gui;

/// <summary>The client: saves the window's state, moves and resizes the window, then undoes.</summary>
public static class Test
{
    public static void Run()
    {
        GuiComponent window = new GuiComponent("window", 0, 0, 20, 10);

        Console.WriteLine(window);

        window.SetMemento(new GuiComponentMemento());
        window.SaveState();

        window.X = 20;
        window.Length = 40;
        Console.WriteLine(window);

        window.Undo();
        Console.WriteLine(window);
    }
}
