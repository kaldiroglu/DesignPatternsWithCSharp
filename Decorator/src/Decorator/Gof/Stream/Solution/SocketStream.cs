namespace dev.kaldiroglu.Decorator.Gof.Stream.Solution;

/// <summary>A second <b>ConcreteComponent</b> — and it costs no new transformations.</summary>
public sealed class SocketStream : Stream
{
    private readonly List<string> _packets = [];

    public SocketStream() : this(DefaultBufferSize)
    {
    }

    public SocketStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => _packets.Add(TakeBuffer());

    public IReadOnlyList<string> Packets() => _packets.AsReadOnly();

    public string Contents() => string.Concat(_packets);
}
