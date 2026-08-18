namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>
/// The Abstraction: a kind of payment, over whatever provider it was handed.
/// <para>
/// Four payment kinds and three providers are seven classes, not twelve. Nothing below this
/// line mentions a bank, a wallet or a drawer.
/// </para>
/// </summary>
public abstract class Payment
{
    protected readonly IPaymentProvider Provider;

    protected Payment(IPaymentProvider provider) =>
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));

    /// <summary>Takes the money, however this kind of payment takes it.</summary>
    public abstract IReadOnlyList<Receipt> Collect(decimal amount);
}
