namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>A ConcreteImplementor: a stored-value wallet. Also two-phase.</summary>
public sealed class Wallet : IPaymentProvider
{
    private int _counter;

    public string Name => "wallet";

    public Authorization Authorize(decimal amount) =>
        new($"WLT-{++_counter}", amount, false);

    public Receipt Capture(Authorization authorization) =>
        new(authorization.Reference, authorization.Amount, Name);

    public Receipt Refund(decimal amount, string reference) =>
        new($"{reference}-R", -amount, Name);
}
