namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>Undo needs nothing remembered: the television keeps its own channel and volume while off.</summary>
public sealed class TurnOff : ICommand
{
    private readonly Television _tv;

    public TurnOff(Television tv) => _tv = tv;

    public void Execute() => _tv.TurnOff();

    public void Undo() => _tv.TurnOn();
}
