namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// A Leaf that points at another element.
/// </summary>
/// <remarks>
/// <para>
/// A link names what it points at, which makes it a real element of the tree and raises a
/// good question: <b>how big is it?</b>
/// </para>
/// <para>
/// The answer taken here is that a link costs its own few bytes and <em>not</em> the size of
/// what it points at, because the target is counted where it actually lives. Any other answer
/// makes <c>Size()</c> on a root double-count, and a tree that lies about its own size is
/// worse than one that cannot be asked.
/// </para>
/// </remarks>
public abstract class Link : StorageElement
{
    private const long LinkBytes = 64;

    protected Link(string name, Directory? parent, IStorage target) : base(name, parent) =>
        Target = target ?? throw new ArgumentNullException(nameof(target),
            "a link must point at something");

    public IStorage Target { get; }

    public sealed override long Size() => LinkBytes;

    public override string Render(string indent) => $"{indent}{GetName()}  -> {Target.GetName()}";
}

/// <summary>A Leaf: the Windows name for the same idea, resolved by path rather than identity.</summary>
public class ShortCut : Link
{
    public ShortCut(string name, Directory? parent, IStorage target) : base(name, parent, target) =>
        Attach();

    public override IStorage Copy() => new ShortCut(GetName(), null, Target);
}

/// <summary>A Leaf: the macOS name for a link that survives its target being moved.</summary>
public class Alias : Link
{
    public Alias(string name, Directory? parent, IStorage target) : base(name, parent, target) =>
        Attach();

    public override IStorage Copy() => new Alias(GetName(), null, Target);
}
