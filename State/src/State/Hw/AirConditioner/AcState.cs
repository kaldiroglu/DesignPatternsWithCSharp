namespace dev.kaldiroglu.State.Hw.AirConditioner;

/// <summary>
/// The <b>State</b>. The homework question was where the "which state now?" rule lives. It
/// lives in <see cref="Running"/>: every running state uses the same rule, so it is written once.
/// </summary>
internal abstract class AcState
{
    internal abstract string Name { get; }

    internal virtual AcState PowerOn(AirConditioner ac) => this;

    internal virtual AcState PowerOff(AirConditioner ac)
    {
        ac.Record("off");
        return OffState();
    }

    /// <summary>The target or the room temperature changed.</summary>
    internal virtual AcState Changed(AirConditioner ac) => Running(ac);

    internal static AcState OffState() => Off.Instance;

    /// <summary>The running state that fits the room and the target.</summary>
    internal static AcState Running(AirConditioner ac)
    {
        AcState next = ac.Room > ac.Target ? Cooling.Instance
            : ac.Room < ac.Target ? Heating.Instance
            : Idle.Instance;
        ac.Record(next.Name + " (room " + ac.Room + ", target " + ac.Target + ")");
        return next;
    }

    internal sealed class Off : AcState
    {
        internal static readonly Off Instance = new();

        internal override string Name => "off";

        internal override AcState PowerOn(AirConditioner ac) => Running(ac);

        internal override AcState PowerOff(AirConditioner ac) => this;

        internal override AcState Changed(AirConditioner ac) => this;   // off: a new target is kept, nothing runs
    }

    internal sealed class Idle : AcState
    {
        internal static readonly Idle Instance = new();

        internal override string Name => "idle";
    }

    internal sealed class Cooling : AcState
    {
        internal static readonly Cooling Instance = new();

        internal override string Name => "cooling";
    }

    internal sealed class Heating : AcState
    {
        internal static readonly Heating Instance = new();

        internal override string Name => "heating";
    }
}
