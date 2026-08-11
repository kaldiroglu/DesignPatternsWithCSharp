namespace dev.kaldiroglu.Decorator.Gof.Stream.Problem;

/// <summary>
/// The same compression again, because the destination changed. Two axes multiplied:
/// transformations x destinations, one class per cell.
/// </summary>
public class CompressingSocketStream : SocketStream
{
    public CompressingSocketStream()
    {
    }

    public CompressingSocketStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => Store(Codecs.Compress(TakeBuffer()));
}
