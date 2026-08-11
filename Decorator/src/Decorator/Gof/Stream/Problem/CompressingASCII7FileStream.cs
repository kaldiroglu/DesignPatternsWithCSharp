namespace dev.kaldiroglu.Decorator.Gof.Stream.Problem;

/// <summary>Both transformations, in one order, on one destination — a fifth class.</summary>
public class CompressingASCII7FileStream : CompressingFileStream
{
    public CompressingASCII7FileStream()
    {
    }

    public CompressingASCII7FileStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() =>
        Store(Codecs.ToAscii7(Codecs.Compress(TakeBuffer())));
}
