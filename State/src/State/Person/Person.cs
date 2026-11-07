namespace dev.kaldiroglu.State.Person;

/// <summary>
/// The <b>Context</b>. It answers with the words of its current emotional state. The caller
/// changes the state with <see cref="SetEmotionalState"/>; the states never do.
/// </summary>
public class Person(IEmotionalState emotionalState)
{
    private IEmotionalState emotionalState = emotionalState;

    public void SetEmotionalState(IEmotionalState emotionalState)
    {
        this.emotionalState = emotionalState;
    }

    public string SayGoodbye() => emotionalState.SayGoodbye();

    public string SayHello() => emotionalState.SayHello();
}
