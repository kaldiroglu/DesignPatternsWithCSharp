namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>
/// Looks as if its undo is simply "volume down", and it is not. At the top of the range
/// the press changes nothing, and undoing it must change nothing either — so the command
/// remembers whether it actually moved the volume.
/// </summary>
public sealed class VolumeUp : ICommand
{
    private readonly Television _tv;
    private bool _changed;

    public VolumeUp(Television tv) => _tv = tv;

    public void Execute() => _changed = _tv.VolumeUp();

    public void Undo()
    {
        if (_changed)
        {
            _tv.VolumeDown();
        }
    }
}
