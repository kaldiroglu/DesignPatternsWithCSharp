namespace dev.kaldiroglu.Memento.Gof.Problem;

/// <summary>
/// Box A at 0, box B at 100, the bend at 50. Move B 60 to the left, past the bend, and undo
/// by moving it back: B returns to 100, but the bend stays at 20.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Graphic a = new Graphic("A", 0);
        Graphic b = new Graphic("B", 100);
        ConstraintSolver solver = new ConstraintSolver(a, b);
        MoveCommand move = new MoveCommand(solver, b, -60);

        Console.WriteLine("At the start: " + solver.Line());
        move.Execute();
        Console.WriteLine("After move:   " + solver.Line());
        move.Unexecute();
        Console.WriteLine("After undo:   " + solver.Line());
    }
}
