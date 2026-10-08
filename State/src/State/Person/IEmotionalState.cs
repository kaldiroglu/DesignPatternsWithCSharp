namespace dev.kaldiroglu.State.Person;

/// <summary>The <b>State</b>: how a person says hello and goodbye.</summary>
public interface IEmotionalState
{
    string SayHello();

    string SayGoodbye();
}
