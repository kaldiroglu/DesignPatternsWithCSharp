namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>
/// The Implementor: the primitives every way of taking money must support.
/// <para>
/// <b>The cash drawer is the whole exercise.</b> A bank gateway and a wallet both authorize
/// first and capture later; cash does neither, because the money is in the drawer or it is not.
/// There are three defensible answers and this file takes the third.
/// </para>
/// <para>
/// One: <i>widen</i> the interface with <c>SupportsTwoPhase</c> and let the abstraction branch
/// on it. Rejected — that is a boolean asking "which implementation are you?", and every branch
/// on it is a piece of the abstraction that now knows about providers.
/// </para>
/// <para>
/// Two: <i>split</i> into two implementor interfaces and let the abstraction discover which it
/// holds. Rejected for the same reason, wearing a type instead of a boolean.
/// </para>
/// <para>
/// Three: <i>keep the two-phase shape and let cash collapse it.</i> Taken here.
/// <see cref="CashDrawer.Authorize"/> returns an authorization that is already settled, and its
/// capture is a no-op that hands back the receipt. Every provider answers every primitive
/// honestly, and no caller branches.
/// </para>
/// <para>
/// The cost of the choice is real and should be said out loud: an authorization from the cash
/// drawer cannot be voided, because the money already moved. That is a property of cash, not a
/// flaw in the design — but it means <c>Void</c> is a primitive we deliberately did not add,
/// since only two of the three providers could implement it.
/// </para>
/// </summary>
public interface IPaymentProvider
{
    string Name { get; }

    Authorization Authorize(decimal amount);

    Receipt Capture(Authorization authorization);

    Receipt Refund(decimal amount, string reference);
}
