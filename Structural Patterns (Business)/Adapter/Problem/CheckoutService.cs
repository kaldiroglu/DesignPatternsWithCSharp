namespace DevKaldiroglu.DP.Structural.Adapter.Problem;

public class CheckoutService
{
    private readonly AcmeGatewayClient _gateway;
    public CheckoutService(AcmeGatewayClient gateway) => _gateway = gateway;

    public string Checkout(Guid idempotencyKey, string customerRef, decimal amount, string currency)
    {
        // silently wrong for currencies with 0 or 3 minor units
        long cents = (long)Math.Round(amount * 100m, MidpointRounding.AwayFromZero);

        string authToken;
        try
        {
            authToken = _gateway.Authorize(idempotencyKey.ToString(), customerRef, cents, currency);
        }
        catch (AcmeGatewayException e)
        {
            return e.Code switch
            {
                4030 => "DECLINED:CUSTOMER_BLOCKED",
                4001 => "DECLINED:INVALID_AMOUNT",
                _    => $"DECLINED:UNKNOWN({e.Code})"
            };
        }

        try
        {
            return "PAID:" + _gateway.Capture(authToken, cents);
        }
        catch (AcmeGatewayException e)
        {
            _gateway.Release(authToken);
            return $"DECLINED:CAPTURE_FAILED({e.Code})";
        }
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var checkout = new CheckoutService(new AcmeGatewayClient());
        Console.WriteLine(checkout.Checkout(Guid.NewGuid(), "cust-1", 49.95m, "USD"));
        Console.WriteLine(checkout.Checkout(Guid.NewGuid(), "BAD",    10.00m, "USD"));
        Console.WriteLine(checkout.Checkout(Guid.NewGuid(), "cust-2",  0.00m, "USD"));
    }
}
