namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>Holds the successor.</summary>
public abstract class AbstractHandler : IHandler
{
    protected IHandler? successor;

    public AbstractHandler(IHandler? successor)
    {
        this.successor = successor;
    }

    /// <summary>Each request gets new help objects, so one request cannot change the help of another.</summary>
    protected abstract IHelp NewHelp();

    /// <summary>
    /// The Java abstract class leaves <c>handleRequest</c> to its subclasses without naming
    /// it. A C# abstract class must declare every interface member, so it is declared
    /// abstract here.
    /// </summary>
    public abstract IHelp HandleRequest(Context context);
}
