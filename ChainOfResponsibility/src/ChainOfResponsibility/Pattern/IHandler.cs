namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>The <b>Handler</b>: returns help for a context.</summary>
public interface IHandler
{
    IHelp HandleRequest(Context context);
}
