namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>The mirror of <see cref="VolumeUp"/>: it remembers whether the press did anything.</summary>
public sealed class VolumeDown : ICommand
{
    private readonly Television _tv;
    private bool _changed;

    public VolumeDown(Television tv) => _tv = tv;

    public void Execute() => _changed = _tv.VolumeDown();

    public void Undo()
    {
        if (_changed)
        {
            _tv.VolumeUp();
        }
    }
}
