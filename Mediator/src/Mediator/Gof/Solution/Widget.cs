namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>
/// The <b>Colleague</b>: a widget knows its director and nothing else. When something
/// happens to it, it says so: <c>Changed()</c>.
/// </summary>
public abstract class Widget
{
    private readonly DialogDirector director;

    protected Widget(DialogDirector director)
    {
        this.director = director;
    }

    protected void Changed()
    {
        director.WidgetChanged(this);
    }
}
