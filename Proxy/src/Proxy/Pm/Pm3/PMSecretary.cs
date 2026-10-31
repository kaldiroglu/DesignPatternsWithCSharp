namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// How a citizen gets an <see cref="IPM"/>. The secretary hands out a <see cref="ProxyPM"/>, and
/// no caller is given a reference to the <see cref="RealPM"/>. The Prime Minister is created the
/// first time <see cref="GetPM"/> is called, and <see cref="Citizen"/>'s constructor calls it.
/// So she exists from the first citizen, before any call. This is not yet a virtual proxy; Pm4
/// creates her on the first forwarded call.
/// </summary>
public class PMSecretary
{
    private IPM? _pm;

    public IPM GetPM() => _pm ??= new ProxyPM(new RealPM());

    /// <summary>For the demo and the tests: has the office been set up yet?</summary>
    public bool HasBeenAskedForThePM => _pm != null;
}
