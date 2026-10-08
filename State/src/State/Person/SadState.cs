namespace dev.kaldiroglu.State.Person;

/// <summary>A <b>ConcreteState</b>: a sad person.</summary>
public class SadState : IEmotionalState
{
    public string SayGoodbye() => "Bye. Sniff, sniff.";

    public string SayHello() => "Hello. Sniff, sniff.";
}
