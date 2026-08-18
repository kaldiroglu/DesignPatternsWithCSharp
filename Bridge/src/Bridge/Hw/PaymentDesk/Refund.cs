namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>A RefinedAbstraction: money going the other way.</summary>
public sealed class Refund : Payment
{
    private readonly string _originalReference;

    public Refund(IPaymentProvider provider, string originalReference) : base(provider) =>
        _originalReference = originalReference;

    public override IReadOnlyList<Receipt> Collect(decimal amount) =>
        new[] { Provider.Refund(amount, _originalReference) };
}
