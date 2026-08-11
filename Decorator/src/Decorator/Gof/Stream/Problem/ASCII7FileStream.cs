namespace dev.kaldiroglu.Decorator.Gof.Stream.Problem;

/// <summary>The other transformation, welded to the same destination.</summary>
public class ASCII7FileStream : FileStream
{
    public ASCII7FileStream()
    {
    }

    public ASCII7FileStream(int bufferSize) : base(bufferSize)
    {
    }

    protected override void HandleBufferFull() => Store(Codecs.ToAscii7(TakeBuffer()));
}
