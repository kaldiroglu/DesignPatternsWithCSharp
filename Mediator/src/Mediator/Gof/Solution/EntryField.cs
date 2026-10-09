namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>A <b>ConcreteColleague</b>: a text field.</summary>
public sealed class EntryField : Widget
{
    public EntryField(DialogDirector director) : base(director)
    {
    }

    /// <summary>Called by the user typing: reports the change.</summary>
    public void Type(string text)
    {
        Text = text;
        Changed();
    }

    /// <summary>Called by the director: sets the text without reporting it back.</summary>
    public void SetText(string text)
    {
        Text = text;
    }

    public string Text { get; private set; } = "";
}
