namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>A <b>ConcreteState</b>: the machine has drinks and waits for a coin.</summary>
internal sealed class WaitingForCoin : IVendingState
{
    public IVendingState InsertCoin(VendingMachine machine)
    {
        machine.Record("coin accepted");
        return new HasCoin();
    }

    public IVendingState PressButton(VendingMachine machine)
    {
        machine.Record("insert a coin first");
        return this;
    }
}
