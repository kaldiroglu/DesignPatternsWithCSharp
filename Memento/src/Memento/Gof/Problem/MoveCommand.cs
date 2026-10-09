namespace dev.kaldiroglu.Memento.Gof.Problem;

/// <summary>
/// Undo by doing the opposite: move the box back and solve again.
/// <para>
/// The box returns to its place, but the solver's bend does not: the solver kept the bend it
/// chose during the move. To put it back, the command would need the solver's private
/// <c>bendX</c> — a getter and a setter that expose the solver's inside to every caller.
/// </para>
/// </summary>
public sealed class MoveCommand
{
    private readonly ConstraintSolver solver;
    private readonly Graphic target;
    private readonly int dx;

    public MoveCommand(ConstraintSolver solver, Graphic target, int dx)
    {
        this.solver = solver;
        this.target = target;
        this.dx = dx;
    }

    public void Execute()
    {
        target.Move(dx);
        solver.Solve();
    }

    public void Unexecute()
    {
        target.Move(-dx);
        solver.Solve();
    }
}
