namespace dev.kaldiroglu.Decorator.Gof.Stream.Solution;

/// <summary>A <b>ConcreteDecorator</b>: compresses on the way out, to any destination.</summary>
public sealed class CompressingStream : StreamDecorator
{
    public CompressingStream(Stream component) : this(component, DefaultBufferSize)
    {
    }

    public CompressingStream(Stream component, int bufferSize) : base(component, bufferSize)
    {
    }

    protected override void HandleBufferFull() => Forward(Codecs.Compress(TakeBuffer()));
}
