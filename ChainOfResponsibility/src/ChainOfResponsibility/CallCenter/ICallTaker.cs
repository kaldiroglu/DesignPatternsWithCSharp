namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>The <b>Handler</b>: a desk in the call center that answers a customer or passes the call on.</summary>
public interface ICallTaker
{
    void Answer(ICustomer customer);
}
