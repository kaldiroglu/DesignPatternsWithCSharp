namespace dev.kaldiroglu.Decorator.Gof.Stream.Problem;

/// <summary>One transformation, welded to one destination.</summary>
public class CompressingFileStream : FileStream
{
    public CompressingFileStream()
    {
    }

    public CompressingFileStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => Store(Codecs.Compress(TakeBuffer()));
}
