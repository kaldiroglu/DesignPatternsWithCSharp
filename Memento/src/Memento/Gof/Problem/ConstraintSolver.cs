using System.Globalization;

namespace dev.kaldiroglu.Memento.Gof.Problem;

/// <summary>
/// GoF's motivation, before the pattern: a solver that keeps the line between two boxes.
/// <para>
/// The line has a bend at <c>bendX</c>. After a move, the solver keeps the bend where it is
/// if it still lies between the two boxes; otherwise it puts it in the middle. So the line
/// depends on the history of moves, not only on where the boxes are now — and moving a box
/// back does not always give the old line back.
/// </para>
/// </summary>
public sealed class ConstraintSolver
{
    private readonly Graphic from;
    private readonly Graphic to;
    private int bendX;

    public ConstraintSolver(Graphic from, Graphic to)
    {
        this.from = from;
        this.to = to;
        bendX = (from.X + to.X) / 2;
    }

    public void Solve()
    {
        int low = Math.Min(from.X, to.X);
        int high = Math.Max(from.X, to.X);
        if (bendX <= low || bendX >= high)
        {
            bendX = (low + high) / 2;
        }
    }

    public string Line() =>
        string.Create(CultureInfo.InvariantCulture, $"{from.Name} {from.X} -> bend {bendX} -> {to.Name} {to.X}");
}
