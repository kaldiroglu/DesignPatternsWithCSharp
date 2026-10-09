namespace dev.kaldiroglu.Memento.Gof.Solution;

/// <summary>
/// The <b>Caretaker</b>: GoF's <c>MoveCommand</c>. Before it moves the box, it asks the
/// solver for a memento; to undo, it moves the box back and gives the memento back.
/// </summary>
public sealed class MoveCommand
{
    private readonly ConstraintSolver solver;
    private readonly Graphic target;
    private readonly int dx;
    private ConstraintSolver.IMemento? state;

    public MoveCommand(ConstraintSolver solver, Graphic target, int dx)
    {
        this.solver = solver;
        this.target = target;
        this.dx = dx;
    }

    public void Execute()
    {
        state = solver.CreateMemento();
        target.Move(dx);
        solver.Solve();
    }

    public void Unexecute()
    {
        target.Move(-dx);
        solver.SetMemento(state!);
        solver.Solve();
    }
}
