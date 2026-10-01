namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>The <b>Receiver</b>. It keeps its channel and volume while it is off.</summary>
public sealed class Television
{
    public const int MaxVolume = 10;

    public void TurnOn() => IsOn = true;

    public void TurnOff() => IsOn = false;

    public bool IsOn { get; private set; }

    public int Channel { get; private set; } = 1;

    public void SelectChannel(int channel) => Channel = channel;

    public int Volume { get; private set; } = 5;

    /// <summary>Answers whether the volume actually changed: at the top it does not.</summary>
    public bool VolumeUp()
    {
        if (Volume == MaxVolume)
        {
            return false;
        }

        Volume++;
        return true;
    }

    /// <summary>Answers whether the volume actually changed: at zero it does not.</summary>
    public bool VolumeDown()
    {
        if (Volume == 0)
        {
            return false;
        }

        Volume--;
        return true;
    }
}
