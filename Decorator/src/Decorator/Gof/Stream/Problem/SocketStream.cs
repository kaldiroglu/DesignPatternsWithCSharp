namespace dev.kaldiroglu.Decorator.Gof.Stream.Problem;

/// <summary>A second destination — and therefore a second copy of every transformation.</summary>
public class SocketStream : Stream
{
    private readonly List<string> _packets = [];

    public SocketStream() : this(DefaultBufferSize)
    {
    }

    public SocketStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => Store(TakeBuffer());

    protected void Store(string packet) => _packets.Add(packet);

    public IReadOnlyList<string> Packets() => _packets.AsReadOnly();

    public string Contents() => string.Concat(_packets);
}
