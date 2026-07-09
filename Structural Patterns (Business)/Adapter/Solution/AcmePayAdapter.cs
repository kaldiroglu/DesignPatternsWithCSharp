namespace DevKaldiroglu.DP.Structural.Adapter.Solution;

public class AcmePayAdapter : IPaymentProcessor
{
    private readonly AcmeGatewayClient _client;
    public AcmePayAdapter(AcmeGatewayClient client) => _client = client;

    public PaymentResult Charge(PaymentRequest req)
    {
        var minorUnits = req.Amount.ToMinorUnits();
        var currency = req.Amount.Currency;
        var requestUid = req.IdempotencyKey.ToString();

        string authToken;
        try
        {
            authToken = _client.Authorize(requestUid, req.CustomerRef, minorUnits, currency);
        }
        catch (AcmeGatewayException e)
        {
            return new PaymentResult.Failure(Translate(e.Code), e.Message);
        }

        try
        {
            return new PaymentResult.Success(_client.Capture(authToken, minorUnits));
        }
        catch (AcmeGatewayException e)
        {
            _client.Release(authToken);
            return new PaymentResult.Failure(FailureReason.CaptureFailed, e.Message);
        }
    }

    private static FailureReason Translate(int code) => code switch
    {
        4001 => FailureReason.InvalidAmount,
        4030 => FailureReason.CustomerBlocked,
        _    => FailureReason.GatewayError
    };
}

public class CheckoutService
{
    private readonly IPaymentProcessor _processor;
    public CheckoutService(IPaymentProcessor processor) => _processor = processor;
    public PaymentResult Checkout(PaymentRequest req) => _processor.Charge(req);
}

public static class SolutionDemo
{
    public static void Run()
    {
        IPaymentProcessor processor = new AcmePayAdapter(new AcmeGatewayClient());
        var checkout = new CheckoutService(processor);

        Print(checkout.Checkout(new PaymentRequest(Guid.NewGuid(), "cust-1", new Money(49.95m, "USD"))));
        Print(checkout.Checkout(new PaymentRequest(Guid.NewGuid(), "cust-jp", new Money(1500m,  "JPY"))));
        Print(checkout.Checkout(new PaymentRequest(Guid.NewGuid(), "BAD",     new Money(10.00m, "USD"))));
    }

    private static void Print(PaymentResult r) =>
        Console.WriteLine(r switch
        {
            PaymentResult.Success s => "PAID:" + s.AuthorizationId,
            PaymentResult.Failure f => $"DECLINED:{f.Reason} ({f.Detail})",
            _ => "?"
        });
}
