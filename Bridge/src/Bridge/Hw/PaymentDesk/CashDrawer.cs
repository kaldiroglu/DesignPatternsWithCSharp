namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>
/// A ConcreteImplementor: notes and coins, at the desk.
/// <para>
/// There is no hold and no later settlement. <c>Authorize</c> takes the money and says so by
/// returning an authorization already marked settled; <c>Capture</c> therefore has nothing to do
/// and returns the receipt for what already happened. The abstraction above calls both, in
/// order, exactly as it does for the bank — and never learns that one of the two calls did
/// nothing.
/// </para>
/// </summary>
public sealed class CashDrawer : IPaymentProvider
{
    private int _counter;

    public string Name => "cash";

    public Authorization Authorize(decimal amount) =>
        new($"CASH-{++_counter}", amount, true);

    public Receipt Capture(Authorization authorization) =>
        new(authorization.Reference, authorization.Amount, Name);

    public Receipt Refund(decimal amount, string reference) =>
        new($"{reference}-R", -amount, Name);
}
