namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>A <b>ConcreteColleague</b>: a list. It reports a selection and knows no other widget.</summary>
public sealed class ListBox : Widget
{
    public ListBox(DialogDirector director) : base(director)
    {
    }

    public void Select(string item)
    {
        Selection = item;
        Changed();
    }

    public string Selection { get; private set; } = "";
}
