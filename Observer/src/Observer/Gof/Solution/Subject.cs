namespace dev.kaldiroglu.Observer.Gof.Solution;

/// <summary>
/// The <b>Subject</b>: GoF's <c>Subject</c>. It keeps a list of observers and tells them
/// all when something changes.
/// <para>
/// It knows them only as <see cref="IObserver"/>s. That is the whole decoupling: a subject
/// and its observers can be in different layers, and each can change without the other.
/// </para>
/// </summary>
public abstract class Subject
{
    private readonly List<IObserver> observers = [];

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    /// <summary>Called by the subject itself after its state has changed.</summary>
    protected void NotifyObservers()
    {
        foreach (var observer in observers.ToList())
        {
            observer.Update(this);
        }
    }

    public int ObserverCount => observers.Count;
}
