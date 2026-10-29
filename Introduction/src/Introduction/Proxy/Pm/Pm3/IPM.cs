namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// The Subject: what a citizen is entitled to ask of the Prime Minister's office.
/// <para>
/// This one interface is the whole difference between <c>Pm2</c> and the solution. Because
/// <see cref="RealPM"/> and <see cref="ProxyPM"/> both implement it, a citizen holding an
/// <c>IPM</c> cannot tell which she has — and does not need to be rewritten when the answer
/// changes.
/// </para>
/// </summary>
public interface IPM
{
    void Listen(string problem);

    void FindJob(string name);
}
