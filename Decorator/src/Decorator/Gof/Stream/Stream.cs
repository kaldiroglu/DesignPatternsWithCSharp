namespace dev.kaldiroglu.Decorator.Gof.Stream;

/// <summary>
/// GoF's second Sample Code example (pp. 182–184): a stream that buffers what is put into
/// it and hands the buffer on when it is full.
/// </summary>
public abstract class Stream
{
    public const int DefaultBufferSize = 64;

    private readonly System.Text.StringBuilder _buffer = new();
    private readonly int _bufferSize;

    protected Stream(int bufferSize)
    {
        if (bufferSize <= 0)
        {
            throw new ArgumentException("buffer size must be positive");
        }

        _bufferSize = bufferSize;
    }

    public void PutInt(int value) => PutString(value.ToString());

    public void PutString(string value)
    {
        _buffer.Append(value);
        if (_buffer.Length >= _bufferSize)
        {
            HandleBufferFull();
        }
    }

    public virtual void Close()
    {
        if (_buffer.Length > 0)
        {
            HandleBufferFull();
        }
    }

    protected abstract void HandleBufferFull();

    protected string TakeBuffer()
    {
        var contents = _buffer.ToString();
        _buffer.Clear();
        return contents;
    }
}
