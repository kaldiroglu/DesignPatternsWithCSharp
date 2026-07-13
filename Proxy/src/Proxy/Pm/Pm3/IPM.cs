namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// Stage 3 — the Subject. Now both the real PM and the proxy implement a common
/// interface, so the <see cref="Citizen"/> depends only on <see cref="IPM"/> and
/// cannot tell whether it holds the real subject or its proxy. This is the Proxy
/// pattern in its canonical form.
/// </summary>
public interface IPM
{
    void Listen(string problem);

    void FindJob(string name);
}
