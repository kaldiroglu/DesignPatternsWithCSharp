namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>
/// The button whose undo has to remember something: the channel that was on before.
/// The request names the new channel; only the receiver knew the old one.
/// </summary>
public sealed class SelectChannel : ICommand
{
    private readonly Television _tv;
    private readonly int _channel;
    private int _previous;

    public SelectChannel(Television tv, int channel)
    {
        _tv = tv;
        _channel = channel;
    }

    public void Execute()
    {
        _previous = _tv.Channel;
        _tv.SelectChannel(_channel);
    }

    public void Undo() => _tv.SelectChannel(_previous);
}
