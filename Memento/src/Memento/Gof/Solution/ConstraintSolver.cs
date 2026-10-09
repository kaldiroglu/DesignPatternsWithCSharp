using System.Globalization;

namespace dev.kaldiroglu.Memento.Gof.Solution;

/// <summary>
/// The <b>Originator</b>: GoF's <c>ConstraintSolver</c>. It can write its state into a
/// memento and take it back, without showing that state to anyone else.
/// </summary>
public sealed class ConstraintSolver
{
    /// <summary>
    /// The <b>Memento</b>, as a command sees it: GoF's <c>ConstraintSolverMemento</c>. It has
    /// no members, so a command can keep it but not read it. GoF implementation issue 1
    /// (language support): C++ makes the solver a friend; Java nests the class.
    /// </summary>
    /// <remarks>
    /// C# has no friend classes, and an enclosing class cannot read a nested class's private
    /// members. So the command gets this empty interface, and the class that holds the bend,
    /// <see cref="Memento"/>, is private to the solver.
    /// </remarks>
    public interface IMemento
    {
    }

    /// <summary>
    /// The state inside the memento: the bend. The class is private, so only the solver can
    /// name it, create it and read it.
    /// </summary>
    private sealed class Memento : IMemento
    {
        public int BendX { get; }

        public Memento(int bendX)
        {
            BendX = bendX;
        }
    }

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

    public IMemento CreateMemento()
    {
        return new Memento(bendX);
    }

    /// <summary>
    /// Takes the bend back from a memento this solver's class made. A memento of any other
    /// class throws <see cref="InvalidCastException"/>.
    /// </summary>
    public void SetMemento(IMemento memento)
    {
        bendX = ((Memento)memento).BendX;
    }

    public string Line() =>
        string.Create(CultureInfo.InvariantCulture, $"{from.Name} {from.X} -> bend {bendX} -> {to.Name} {to.X}");
}
