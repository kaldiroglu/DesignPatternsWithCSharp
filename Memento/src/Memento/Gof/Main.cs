namespace dev.kaldiroglu.Memento.Gof;

/// <summary>
/// Box A at 0, box B at 100, the bend at 50. Move B 60 to the left — past the bend — and undo.
/// </summary>
public static class Main
{
    public static void Run()
    {
        var a1 = new Problem.Graphic("A", 0);
        var b1 = new Problem.Graphic("B", 100);
        var solver1 = new Problem.ConstraintSolver(a1, b1);
        var move1 = new Problem.MoveCommand(solver1, b1, -60);
        Console.WriteLine("Before the pattern");
        Console.WriteLine("  at the start: " + solver1.Line());
        move1.Execute();
        Console.WriteLine("  after move:   " + solver1.Line());
        move1.Unexecute();
        Console.WriteLine("  after undo:   " + solver1.Line());

        var a2 = new Solution.Graphic("A", 0);
        var b2 = new Solution.Graphic("B", 100);
        var solver2 = new Solution.ConstraintSolver(a2, b2);
        var move2 = new Solution.MoveCommand(solver2, b2, -60);
        Console.WriteLine("With a memento");
        Console.WriteLine("  at the start: " + solver2.Line());
        move2.Execute();
        Console.WriteLine("  after move:   " + solver2.Line());
        move2.Unexecute();
        Console.WriteLine("  after undo:   " + solver2.Line());
    }
}
