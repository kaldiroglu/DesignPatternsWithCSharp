namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// Hands out the PM as an <see cref="IPM"/> — but always wrapped in a
/// <see cref="ProxyPM"/>. The citizen only ever gets the proxy, never a direct
/// reference to the <see cref="RealPM"/>.
/// </summary>
public class PMSecretary
{
    private readonly IPM _pm = new ProxyPM(new RealPM());

    public IPM GetMePM() => _pm;
}
