namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// How a citizen obtains an <see cref="IPM"/> — and the reason she cannot obtain the real one.
/// <para>
/// The secretary hands out a <see cref="ProxyPM"/>. Nothing in the returned type says so, and
/// no caller can reach past it to the <see cref="RealPM"/>, because no caller is ever given a
/// reference to it.
/// </para>
/// <para>
/// The Prime Minister is also created lazily: she is not brought into existence until
/// somebody actually needs her. That is a second kind of proxy — GoF's virtual proxy — present
/// in the same object.
/// </para>
/// </summary>
public class PMSecretary
{
    private IPM? pm;

    public IPM GetPM()
    {
        pm ??= new ProxyPM(new RealPM());
        return pm;
    }

    /// <summary>For the demo and the tests: has the office been set up yet?</summary>
    public bool HasBeenAskedForThePM() => pm != null;
}
