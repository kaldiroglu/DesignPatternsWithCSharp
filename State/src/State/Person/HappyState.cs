namespace dev.kaldiroglu.State.Person;

/// <summary>A <b>ConcreteState</b>: a happy person.</summary>
public class HappyState : IEmotionalState
{
    public string SayGoodbye() => "Bye, friend!";

    public string SayHello() => "Hello, friend!";
}
