namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>A ConcreteImplementor: a card gateway, genuinely two-phase.</summary>
public sealed class BankGateway : IPaymentProvider
{
    private int _counter;

    public string Name => "bank";

    public Authorization Authorize(decimal amount) =>
        new($"BANK-{++_counter}", amount, false);

    public Receipt Capture(Authorization authorization) =>
        new(authorization.Reference, authorization.Amount, Name);

    public Receipt Refund(decimal amount, string reference) =>
        new($"{reference}-R", -amount, Name);
}
