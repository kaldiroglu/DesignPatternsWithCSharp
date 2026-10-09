namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>The end of the chain: answers every request that reaches it, the generic ones.</summary>
public class ConcreteHandler3 : AbstractHandler
{
    public ConcreteHandler3(IHandler? successor) : base(successor)
    {
    }

    protected override IHelp NewHelp() => new Help3();

    public override IHelp HandleRequest(Context context)
    {
        // The last handler: it answers every context that reaches it, Context.GENERIC included.
        return NewHelp();
    }
}
