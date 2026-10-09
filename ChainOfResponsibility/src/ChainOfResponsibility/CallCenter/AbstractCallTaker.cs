namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>Holds the next desk in the chain.</summary>
public abstract class AbstractCallTaker : ICallTaker
{
    protected ICallTaker? next;

    public AbstractCallTaker(ICallTaker? next)
    {
        this.next = next;
    }

    public ICallTaker? Next
    {
        get => next;
        set => next = value;
    }

    /// <summary>
    /// The Java abstract class leaves <c>answer</c> to its subclasses without naming it. A C#
    /// abstract class must declare every interface member, so it is declared abstract here.
    /// </summary>
    public abstract void Answer(ICustomer customer);
}
