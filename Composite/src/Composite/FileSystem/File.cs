namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>A Leaf: a file has bytes and no children.</summary>
public class File : StorageElement
{
    private readonly long _bytes;

    public File(string name, Directory? parent) : this(name, parent, 1024)
    {
    }

    public File(string name, Directory? parent, long bytes) : base(name, parent)
    {
        _bytes = bytes;
        Attach();
    }

    /// <summary>A leaf answers for itself. That is the whole of a leaf's job.</summary>
    public override long Size() => _bytes;

    public override IStorage Copy() => new File(GetName(), null, _bytes);

    public override string Render(string indent) => $"{indent}{GetName()}  ({_bytes} bytes)";
}
