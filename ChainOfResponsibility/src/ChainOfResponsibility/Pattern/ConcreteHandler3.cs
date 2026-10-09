namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>The end of the chain: answers every request that reaches it, the generic ones.</summary>
public class ConcreteHandler3 : AbstractHandler
{
    public ConcreteHandler3(IHandler? successor, IHandler? predecessor) : base(successor, predecessor)
    {
        help = new Help3();
    }

    public override IHelp HandleRequest(Context context)
    {
        // Context.GENERIC
        // NOTE: this returns a new Help3 every time, not the field help. The field is never
        // used. The Java has the same behavior.
        return new Help3();
    }
}
