namespace DevKaldiroglu.DP.Structural.Adapter.Solution;

public readonly record struct Money(decimal Amount, string Currency)
{
    private static readonly Dictionary<string, int> MinorUnits = new()
    {
        ["USD"] = 2, ["EUR"] = 2, ["GBP"] = 2, ["TRY"] = 2,
        ["JPY"] = 0, ["KRW"] = 0,
        ["BHD"] = 3, ["KWD"] = 3
    };

    public long ToMinorUnits()
    {
        var scale = MinorUnits.GetValueOrDefault(Currency, 2);
        return (long)Math.Round(Amount * (decimal)Math.Pow(10, scale), MidpointRounding.AwayFromZero);
    }
}

public sealed record PaymentRequest(Guid IdempotencyKey, string CustomerRef, Money Amount);

public abstract record PaymentResult
{
    private PaymentResult() { }
    public sealed record Success(string AuthorizationId) : PaymentResult;
    public sealed record Failure(FailureReason Reason, string Detail) : PaymentResult;
}

public enum FailureReason
{
    CustomerBlocked,
    InvalidAmount,
    CaptureFailed,
    GatewayError
}

public interface IPaymentProcessor
{
    PaymentResult Charge(PaymentRequest request);
}
