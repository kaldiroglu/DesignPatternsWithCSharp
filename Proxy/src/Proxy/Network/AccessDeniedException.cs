namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// Raised by the protection proxy (<see cref="ProxyServer"/>) when a client is
/// not allowed to make the requested connection.
/// </summary>
public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message) : base(message)
    {
    }
}
