namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>Answers a <c>MORE_SPECIFIC</c> request; for any other, asks its successor and adds its own help.</summary>
public class ConcreteHandler1 : AbstractHandler
{
    public ConcreteHandler1(IHandler? successor) : base(successor)
    {
    }

    protected override IHelp NewHelp() => new Help1();

    public override IHelp HandleRequest(Context context)
    {
        if (context == Context.MORE_SPECIFIC)
        {
            return NewHelp();
        }
        else
        {
            IHelp successorHelp = successor!.HandleRequest(context);
            successorHelp.AddHelp(NewHelp());
            return successorHelp;
        }
    }
}
