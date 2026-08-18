namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>
/// A RefinedAbstraction: the same amount, taken in equal parts.
/// <para>
/// The rounding rule — every installment equal, the remainder onto the first — is business
/// logic, written once, and correct on every provider including the cash drawer.
/// </para>
/// </summary>
public sealed class InstallmentPlan : Payment
{
    private readonly int _installments;

    public InstallmentPlan(IPaymentProvider provider, int installments) : base(provider)
    {
        if (installments < 2)
        {
            throw new ArgumentException("an installment plan needs at least two");
        }

        _installments = installments;
    }

    public override IReadOnlyList<Receipt> Collect(decimal amount)
    {
        // Truncate towards zero to two places, exactly as Java's RoundingMode.DOWN does.
        var each = Math.Truncate(amount / _installments * 100m) / 100m;
        var remainder = amount - each * _installments;

        var receipts = new List<Receipt>();
        for (var i = 0; i < _installments; i++)
        {
            var due = i == 0 ? each + remainder : each;
            receipts.Add(Provider.Capture(Provider.Authorize(due)));
        }

        return receipts;
    }
}
