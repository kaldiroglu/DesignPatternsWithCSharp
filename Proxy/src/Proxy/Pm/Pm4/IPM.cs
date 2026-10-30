namespace dev.kaldiroglu.Proxy.Pm.Pm4;

/// <summary>The Subject. <see cref="Citizen"/> depends on it and on nothing else.</summary>
public interface IPM
{
    void Listen(string problem);

    void FindJob(string name);
}
