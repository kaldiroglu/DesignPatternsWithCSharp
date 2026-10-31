namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// The Subject: what a citizen may ask of the Prime Minister's office. <see cref="RealPM"/> and
/// <see cref="ProxyPM"/> both implement it, so a citizen cannot tell which she holds and is not
/// rewritten when the answer changes.
/// </summary>
public interface IPM
{
    void Listen(string problem);

    void FindJob(string name);
}
