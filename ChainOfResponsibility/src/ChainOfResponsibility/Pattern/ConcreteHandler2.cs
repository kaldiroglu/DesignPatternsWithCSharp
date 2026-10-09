namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>Answers a <c>SPECIFIC</c> request; for any other, asks its successor and adds its own help.</summary>
public class ConcreteHandler2 : AbstractHandler
{
    public ConcreteHandler2(IHandler? successor, IHandler? predecessor) : base(successor, predecessor)
    {
        help = new Help2();
    }

    public override IHelp HandleRequest(Context context)
    {
        if (context == Context.SPECIFIC)
        {
            return help;
        }
        else
        {
            IHelp successorHelp = successor!.HandleRequest(context);
            successorHelp.OtherHelp = help;
            return successorHelp;
        }
    }
}
