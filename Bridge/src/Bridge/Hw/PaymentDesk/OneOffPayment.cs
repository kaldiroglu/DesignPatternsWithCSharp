namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>A RefinedAbstraction: authorize, then capture. Once.</summary>
public sealed class OneOffPayment : Payment
{
    public OneOffPayment(IPaymentProvider provider) : base(provider)
    {
    }

    public override IReadOnlyList<Receipt> Collect(decimal amount)
    {
        var hold = Provider.Authorize(amount);
        return new[] { Provider.Capture(hold) };
    }
}
