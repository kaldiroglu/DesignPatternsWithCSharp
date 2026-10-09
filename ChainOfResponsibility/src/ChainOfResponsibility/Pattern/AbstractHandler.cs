namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>Holds the successor, the predecessor and this handler's own help.</summary>
public abstract class AbstractHandler : IHandler
{
    protected IHandler? successor;

    // NOTE: the predecessor is always null. Test builds each handler before its predecessor
    // exists, so it passes null. Nothing reads this field. The Java has the same behavior.
    protected IHandler? predecessor;

    // Every concrete handler sets its help in its constructor.
    protected IHelp help = null!;

    public AbstractHandler(IHandler? successor, IHandler? predecessor)
    {
        this.successor = successor;
        this.predecessor = predecessor;
    }

    /// <summary>
    /// The Java abstract class leaves <c>handleRequest</c> to its subclasses without naming
    /// it. A C# abstract class must declare every interface member, so it is declared
    /// abstract here.
    /// </summary>
    public abstract IHelp HandleRequest(Context context);
}
