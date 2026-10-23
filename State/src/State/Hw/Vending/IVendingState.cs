namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>
/// The <b>State</b>. Each state answers the three requests and returns the next state.
/// <para>
/// C# has no sealed interface. Here the interface is <c>internal</c>, so no code outside this assembly can
/// implement it, and the three states are <c>sealed</c> classes. <see cref="Refilled"/> is a
/// default interface method.
/// </para>
/// </summary>
internal interface IVendingState
{
    IVendingState InsertCoin(VendingMachine machine);

    IVendingState PressButton(VendingMachine machine);

    IVendingState Refilled(VendingMachine machine) => this;
}
