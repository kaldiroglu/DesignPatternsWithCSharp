namespace dev.kaldiroglu.Command.Hw.Remote;

public sealed class TurnOn : ICommand
{
    private readonly Television _tv;

    public TurnOn(Television tv) => _tv = tv;

    public void Execute() => _tv.TurnOn();

    public void Undo() => _tv.TurnOff();
}
