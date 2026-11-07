namespace dev.kaldiroglu.Memento.Pattern2;

/// <summary>
/// The caretaker saves twice, the state changes again, and two undos go back one save at a
/// time: first to state-3, then to state-1.
/// </summary>
public static class Main
{
    public static void Run()
    {
        Originator originator = new Originator("state-0");
        Caretaker caretaker = new Caretaker(originator);

        originator.SetState("state-1");
        caretaker.Save();
        originator.SetState("state-2");
        originator.SetState("state-3");
        caretaker.Save();
        originator.SetState("state-4");

        caretaker.Undo();
        caretaker.Undo();
        caretaker.Undo();
        Console.WriteLine("Final: " + originator);
    }
}
