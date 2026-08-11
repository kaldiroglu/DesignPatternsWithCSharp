namespace dev.kaldiroglu.Decorator.Gof.Stream.Problem;

/// <summary>BEFORE: a destination, and the class every transformation has to subclass.</summary>
public class FileStream : Stream
{
    private readonly List<string> _blocks = [];

    public FileStream() : this(DefaultBufferSize)
    {
    }

    public FileStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => Store(TakeBuffer());

    protected void Store(string block) => _blocks.Add(block);

    public IReadOnlyList<string> Blocks() => _blocks.AsReadOnly();

    public string Contents() => string.Concat(_blocks);
}
