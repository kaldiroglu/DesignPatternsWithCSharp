namespace dev.kaldiroglu.Decorator.Gof.Stream.Solution;

/// <summary>A <b>ConcreteDecorator</b>: folds to 7-bit ASCII, to any destination.</summary>
public sealed class ASCII7Stream : StreamDecorator
{
    public ASCII7Stream(Stream component) : this(component, DefaultBufferSize)
    {
    }

    public ASCII7Stream(Stream component, int bufferSize) : base(component, bufferSize)
    {
    }

    protected override void HandleBufferFull() => Forward(Codecs.ToAscii7(TakeBuffer()));
}
