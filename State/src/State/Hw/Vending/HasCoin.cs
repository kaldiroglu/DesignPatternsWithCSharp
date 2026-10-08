namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>A <b>ConcreteState</b>: a coin is in the machine. Pressing the button gives a drink.</summary>
internal sealed class HasCoin : IVendingState
{
    public IVendingState InsertCoin(VendingMachine machine)
    {
        machine.Record("coin returned: one coin is enough");
        return this;
    }

    public IVendingState PressButton(VendingMachine machine)
    {
        machine.TakeOneDrink();
        machine.Record("drink given");
        return machine.Stock > 0 ? new WaitingForCoin() : new SoldOut();
    }
}
