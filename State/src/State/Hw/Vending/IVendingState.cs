namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>
/// The <b>State</b>. Each state answers the three requests and returns the next state.
/// <para>
/// In Java the interface is <c>sealed</c> and permits only the three states. C# has no sealed
/// interface. Here the interface is <c>internal</c>, so no code outside this assembly can
/// implement it, and the three states are <c>sealed</c> classes. <see cref="Refilled"/> is a
/// default interface method, as in the Java.
/// </para>
/// </summary>
internal interface IVendingState
{
    IVendingState InsertCoin(VendingMachine machine);

    IVendingState PressButton(VendingMachine machine);

    IVendingState Refilled(VendingMachine machine) => this;
}
