namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>
/// The <b>Memento</b>: the originator's state at one moment. It only holds the state.
/// <para>
/// In this namespace the memento is a separate class with a public <see cref="State"/>, so any
/// class can read the state inside it. <c>Pattern2</c> nests the memento in the originator, so
/// only the originator can read it.
/// </para>
/// </summary>
public class Memento
{
    public Memento(string state)
    {
        State = state;
    }

    public string State { get; }
}
