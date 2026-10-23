namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>Prints the answer a customer receives.</summary>
public abstract class AbstractCustomer : ICustomer
{
    /// <summary>
    /// Left to the subclasses. A C# abstract class must declare every interface member, so
    /// it is declared abstract here.
    /// </summary>
    public abstract void AskAQuestion();

    public void ReceiveAnswer(string answer)
    {
        Console.WriteLine("Answer: " + answer);
    }
}
