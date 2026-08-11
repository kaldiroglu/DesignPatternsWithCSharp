namespace dev.kaldiroglu.Decorator.Gof.Stream.Solution;

/// <summary>A <b>ConcreteComponent</b>: a destination, and nothing else.</summary>
public sealed class FileStream : Stream
{
    private readonly List<string> _blocks = [];

    public FileStream() : this(DefaultBufferSize)
    {
    }

    public FileStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => _blocks.Add(TakeBuffer());

    public IReadOnlyList<string> Blocks() => _blocks.AsReadOnly();

    public string Contents() => string.Concat(_blocks);
}
