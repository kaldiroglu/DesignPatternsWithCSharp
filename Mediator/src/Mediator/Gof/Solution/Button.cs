namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>A <b>ConcreteColleague</b>: a button.</summary>
public sealed class Button : Widget
{
    public Button(DialogDirector director) : base(director)
    {
    }

    public bool Enabled { get; set; }

    public void Click()
    {
        if (Enabled)
        {
            Changed();
        }
    }
}
