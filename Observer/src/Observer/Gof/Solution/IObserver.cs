namespace dev.kaldiroglu.Observer.Gof.Solution;

/// <summary>
/// The <b>Observer</b>: GoF's <c>Observer</c>, with one operation.
/// <para>
/// The subject passes itself, so an observer that watches several subjects knows which one
/// changed. That is GoF implementation issue 2 (observing more than one subject).
/// </para>
/// </summary>
public interface IObserver
{
    void Update(Subject changed);
}
