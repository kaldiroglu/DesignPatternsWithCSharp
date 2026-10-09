namespace dev.kaldiroglu.Memento.Gof.Solution;

/// <summary>
/// Box A at 0, box B at 100, the bend at 50. Move B 60 to the left and undo: the command
/// gives the solver its memento back, so the bend returns to 50.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- gof-solution</c>.
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
