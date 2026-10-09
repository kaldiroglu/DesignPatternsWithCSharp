namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

/// <summary>A <b>ConcreteHandler</b> at the end of every chain: the application's general help.</summary>
public sealed class Application : HelpHandler
{
    public Application(string topic) : base(null, topic)
    {
    }
}
