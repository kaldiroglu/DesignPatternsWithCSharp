namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>Prints the answer a customer receives.</summary>
public abstract class AbstractCustomer : ICustomer
{
    /// <summary>
    /// The Java abstract class leaves <c>askAQuestion</c> to its subclasses without naming it.
    /// A C# abstract class must declare every interface member, so it is declared abstract here.
    /// </summary>
    public abstract void AskAQuestion();

    public void ReceiveAnswer(string answer)
    {
        Console.WriteLine("Answer: " + answer);
    }
}
