namespace DevKaldiroglu.DP.Structural.Adapter.Problem;

public class AcmeGatewayException : Exception
{
    public int Code { get; }
    public AcmeGatewayException(int code, string message) : base(message) { Code = code; }
}

public class AcmeGatewayClient
{
    public string Authorize(string requestUid, string customerRef, long amountCents, string currency)
    {
        if (amountCents <= 0)         throw new AcmeGatewayException(4001, "amount must be positive");
        if (customerRef == "BAD")     throw new AcmeGatewayException(4030, "customer blocked");
        return "AUTH-" + requestUid;
    }

    public string Capture(string authToken, long amountCents)
    {
        if (string.IsNullOrEmpty(authToken) || !authToken.StartsWith("AUTH-"))
            throw new AcmeGatewayException(4002, "invalid auth token");
        return "CAP-" + authToken.Substring(5);
    }

    public void Release(string authToken) { /* best-effort */ }
}
