namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>
/// The <b>Mediator</b>: GoF's <c>DialogDirector</c>. Every widget reports its changes here,
/// and only here.
/// </summary>
public abstract class DialogDirector
{
    public abstract void WidgetChanged(Widget widget);
}
