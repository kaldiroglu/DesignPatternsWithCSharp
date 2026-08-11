namespace dev.kaldiroglu.Decorator.Gof.Stream.Solution;

/// <summary>
/// The <b>Decorator</b> for streams: it is a Stream and it has a Stream, and by default it
/// changes nothing.
/// </summary>
public abstract class StreamDecorator : Stream
{
    private readonly Stream _component;

    protected StreamDecorator(Stream component, int bufferSize) : base(bufferSize) =>
        _component = component ?? throw new ArgumentNullException(
            nameof(component), "a decorator must decorate something");

    protected void Forward(string data) => _component.PutString(data);

    protected override void HandleBufferFull() =>
        Forward(TakeBuffer()); // the default decorator changes nothing

    public override void Close()
    {
        base.Close();
        _component.Close();
    }
}
