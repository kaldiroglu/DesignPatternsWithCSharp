namespace dev.kaldiroglu.Proxy.Network;

/// <summary>Thrown by the proxy when a request is not permitted.</summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
