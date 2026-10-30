namespace dev.kaldiroglu.Memento.Gui;

/// <summary>The caretaker: it keeps the memento and never looks inside it.</summary>
public static class Main
{
    public static void Run()
    {
        GuiComponent window = new GuiComponent("window", 0, 0, 20, 10);
        Console.WriteLine(window);

        GuiComponentMemento saved = window.CreateMemento();

        window.X = 20;
        window.Length = 40;
        Console.WriteLine(window);

        window.Restore(saved);
        Console.WriteLine(window);
    }
}
