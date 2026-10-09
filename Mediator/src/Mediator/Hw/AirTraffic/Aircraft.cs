namespace dev.kaldiroglu.Mediator.Hw.AirTraffic;

/// <summary>A <b>Colleague</b>: an aircraft that wants to land or take off. It knows only the tower.</summary>
public sealed class Aircraft
{
    private readonly ControlTower tower;

    public Aircraft(string callSign, string intent, ControlTower tower)
    {
        CallSign = callSign;
        Intent = intent;
        this.tower = tower;
    }

    public void Request()
    {
        tower.Request(this);
    }

    public void Clear()
    {
        tower.RunwayClear(this);
    }

    public string CallSign { get; }

    public string Intent { get; }
}
