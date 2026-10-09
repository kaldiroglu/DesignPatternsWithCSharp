namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>A customer who calls: the request that goes along the chain.</summary>
public interface ICustomer
{
    void AskAQuestion();

    void ReceiveAnswer(string answer);
}
