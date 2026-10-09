namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>Answers a <c>MORE_SPECIFIC</c> request; for any other, asks its successor and adds its own help.</summary>
public class ConcreteHandler1 : AbstractHandler
{
    public ConcreteHandler1(IHandler? successor, IHandler? predecessor) : base(successor, predecessor)
    {
        help = new Help1();
    }

    public override IHelp HandleRequest(Context context)
    {
        if (context == Context.MORE_SPECIFIC)
        {
            return help;
        }
        else
        {
            IHelp successorHelp = successor!.HandleRequest(context);
            // NOTE: this replaces the other help that ConcreteHandler2 has just set. For a
            // GENERIC request, Help3's other help is Help1, and Help2 is lost: Show prints
            // only "Help3" and then "Help1". The Java has the same behavior.
            successorHelp.OtherHelp = help;
            return successorHelp;
        }
    }
}
