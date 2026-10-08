namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>A <b>ConcreteState</b>: no drinks are left. A coin is returned at once.</summary>
internal sealed class SoldOut : IVendingState
{
    public IVendingState InsertCoin(VendingMachine machine)
    {
        machine.Record("coin returned: sold out");
        return this;
    }

    public IVendingState PressButton(VendingMachine machine)
    {
        machine.Record("sold out");
        return this;
    }

    public IVendingState Refilled(VendingMachine machine) =>
        machine.Stock > 0 ? new WaitingForCoin() : this;
}
