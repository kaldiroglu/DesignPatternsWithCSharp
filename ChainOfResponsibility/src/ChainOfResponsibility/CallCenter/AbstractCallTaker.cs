namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>Holds the next desk in the chain.</summary>
public abstract class AbstractCallTaker : ICallTaker
{
    protected ICallTaker? next;

    public AbstractCallTaker(ICallTaker? next)
    {
        this.next = next;
    }

    public ICallTaker? Next => next;

    /// <summary>Gives the customer to the next desk. A desk with no next desk tells the customer so.</summary>
    protected void PassOn(ICustomer customer)
    {
        if (next != null)
            next.Answer(customer);
        else
            customer.ReceiveAnswer("No desk can answer you.");
    }

    /// <summary>
    /// Left to the subclasses. A C# abstract class must declare every interface member, so
    /// it is declared abstract here.
    /// </summary>
    public abstract void Answer(ICustomer customer);
}
