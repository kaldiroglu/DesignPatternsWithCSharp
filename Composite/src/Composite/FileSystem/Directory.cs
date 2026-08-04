using dev.kaldiroglu.Composite.FileSystem.Iterator;

namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// The Composite: a directory is storage, and it holds storage.
/// </summary>
/// <remarks>
/// Every operation here is the same two lines — do this directory's part, then ask the
/// children to do theirs. The recursion is the pattern; there is nothing else to it.
/// </remarks>
public class Directory : StorageElement, IStorageContainer
{
    private const long DirectoryBytes = 256;
    private const long Uncached = -1;

    private readonly List<IStorage> _elements = [];

    /// <summary>
    /// The cached total, and the number of times any directory has had to work one out.
    /// </summary>
    /// <remarks>
    /// <para>
    /// GoF's implementation issue 7: "the Composite class can cache traversal or search
    /// information about its children". Without it, <c>Size()</c> walks the whole subtree on
    /// every call, and a report asking four questions walks it four times.
    /// </para>
    /// <para>
    /// The cache is what finally makes the parent reference load-bearing. A change anywhere
    /// invalidates every ancestor's total, and the only way to reach them is upward — see
    /// <see cref="Invalidate"/>.
    /// </para>
    /// </remarks>
    private long _cachedSize = Uncached;

    private static int _recomputations;

    public Directory(string name) : this(name, null)
    {
    }

    public Directory(string name, Directory? parent) : base(name, parent) => Attach();

    /// <summary>
    /// The payoff: one call, one number, any depth — and no client ever writes a loop or asks
    /// what kind of element it is holding.
    /// </summary>
    public override long Size()
    {
        if (_cachedSize == Uncached)
        {
            _recomputations++;
            _cachedSize = DirectoryBytes + _elements.Sum(e => e.Size());
        }

        return _cachedSize;
    }

    /// <summary>
    /// Throws this directory's total away, and every ancestor's with it.
    /// </summary>
    /// <remarks>
    /// Upward, because a child's change invalidates the totals of everything above it and
    /// nothing below. This is the half of caching people forget, and the half that makes it
    /// wrong when they do.
    /// </remarks>
    internal void Invalidate()
    {
        if (_cachedSize == Uncached)
        {
            return;                       // already dirty, so the ancestors are too
        }

        _cachedSize = Uncached;
        GetParent()?.Invalidate();
    }

    /// <summary>How many times any directory has actually computed a total. For the tests.</summary>
    public static int Recomputations() => _recomputations;

    public static void ResetRecomputations() => _recomputations = 0;

    /// <summary>A deep copy: the directory and everything under it, detached from any parent.</summary>
    public override IStorage Copy()
    {
        var copy = new Directory(GetName(), null);
        foreach (var element in _elements)
        {
            copy.Add(element.Copy());
        }

        return copy;
    }

    /// <summary>
    /// Renders the subtree. Each element renders itself, and this method neither knows nor
    /// asks what kind it is — a type test here would defeat the one thing the pattern is for.
    /// </summary>
    public override string Render(string indent)
    {
        var output = new System.Text.StringBuilder($"{indent}{GetName()}/");
        foreach (var element in _elements)
        {
            output.AppendLine().Append(element.Render(indent + "    "));
        }

        return output.ToString();
    }

    /// <summary>The newest modification in the subtree — a maximum, not a sum.</summary>
    public override DateTimeOffset LastModified()
    {
        var own = base.LastModified();
        if (_elements.Count == 0)
        {
            return own;
        }

        var newest = _elements.Max(e => e.LastModified());
        return newest > own ? newest : own;
    }

    /// <summary>This directory, plus everything beneath it.</summary>
    public override int Count() => 1 + _elements.Sum(e => e.Count());

    /// <summary>
    /// The biggest leaf under here — a reduction that returns an element rather than a
    /// number, and one a directory cannot answer for itself. It has to ask, and take the best
    /// answer it gets back.
    /// </summary>
    public override IStorage? Largest()
    {
        IStorage? best = null;
        foreach (var candidate in _elements.Select(e => e.Largest()).OfType<IStorage>())
        {
            if (best is null || candidate.Size() > best.Size())
            {
                best = candidate;
            }
        }

        return best;
    }

    public override IStorage? Find(string name)
    {
        if (GetName() == name)
        {
            return this;
        }

        return _elements.Select(e => e.Find(name)).FirstOrDefault(hit => hit is not null);
    }

    public override IReadOnlyList<IStorage> FindAll(Func<IStorage, bool> test)
    {
        var found = new List<IStorage>();
        if (test(this))
        {
            found.Add(this);
        }

        foreach (var element in _elements)
        {
            found.AddRange(element.FindAll(test));
        }

        return found.AsReadOnly();
    }

    public void List() => Console.WriteLine(Render(""));

    public void Add(IStorage element)
    {
        if (ReferenceEquals(element, this))
        {
            throw new ArgumentException("a directory cannot contain itself", nameof(element));
        }

        if (!_elements.Contains(element))
        {
            _elements.Add(element);
            if (element is StorageElement child)
            {
                child.SetParent(this);
            }

            Invalidate();
        }
    }

    public void Remove(IStorage element)
    {
        if (_elements.Remove(element))
        {
            Invalidate();
        }
    }

    public IReadOnlyList<IStorage> Elements() => _elements.AsReadOnly();

    public IStorageIterator Iterator() => new DirectoryIterator(this);
}
