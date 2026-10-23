namespace dev.kaldiroglu.State.Person;

/// <summary>
/// The <b>Context</b>. It implements the same interface as its states and answers with the
/// words of its current emotional state.
/// </summary>
public class Person(IEmotionalState emotionalState) : IEmotionalState
{
    // Internal: other classes in this assembly may set it.
    internal IEmotionalState emotionalState = emotionalState;

    public void SetEmotionalState(IEmotionalState emotionalState)
    {
        this.emotionalState = emotionalState;
    }

    public string SayGoodbye() => emotionalState.SayGoodbye();

    public string SayHello() => emotionalState.SayHello();
}
