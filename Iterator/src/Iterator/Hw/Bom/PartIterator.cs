using System.Collections;

using dev.kaldiroglu.Iterator.Hw.Bom.Composite;

namespace dev.kaldiroglu.Iterator.Hw.Bom;

/// <summary>
/// Homework 1: every part in a bill of materials, from the Composite deck's <c>bom</c>.
/// </summary>
/// <remarks>
/// <para>
/// It walks the tree depth first and returns only <see cref="Part"/>s: assemblies are
/// containers, and services have nothing to pick from a shelf. Each part comes with the
/// quantity the whole product needs, multiplied along the path from the top.
/// </para>
/// <para>
/// The homework question was whether a part used in two places should come out twice. Here
/// it does: one <see cref="PartLine"/> for each place it is used. A purchasing report that
/// wants one line per part number adds them up afterwards. Both answers are defensible; the
/// point is to choose one and say so.
/// </para>
/// </remarks>
public sealed class PartIterator : IEnumerator<PartLine>
{
    private readonly record struct Step(BomComponent Component, int Quantity);

    private readonly BomComponent _root;
    private readonly Stack<Step> _pending = new();
    private PartLine? _current;

    public PartIterator(BomComponent root)
    {
        _root = root;
        _pending.Push(new Step(root, 1));
    }

    public PartLine Current =>
        _current ?? throw new InvalidOperationException("no current part: call MoveNext first");

    object IEnumerator.Current => Current;

    /// <summary>Moves to the next part, opening assemblies on the way and skipping services.</summary>
    public bool MoveNext()
    {
        _current = null;
        while (_current is null && _pending.Count > 0)
        {
            Step step = _pending.Pop();
            if (step.Component is Part part)
            {
                _current = new PartLine(part, step.Quantity);
            }

            IReadOnlyList<BomLine> lines = step.Component.Lines;
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                BomLine line = lines[i];
                _pending.Push(new Step(line.Component, step.Quantity * line.Quantity));
            }
        }

        return _current is not null;
    }

    /// <summary>Starts the walk again from the top of the bill of materials.</summary>
    public void Reset()
    {
        _pending.Clear();
        _pending.Push(new Step(_root, 1));
        _current = null;
    }

    public void Dispose()
    {
    }

    /// <summary>So that a bill of materials can be used in a <c>foreach</c> loop.</summary>
    public static IEnumerable<PartLine> PartsOf(BomComponent root) =>
        new LambdaEnumerable<PartLine>(() => new PartIterator(root));
}
