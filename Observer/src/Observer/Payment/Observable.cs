namespace dev.kaldiroglu.Observer.Payment;

/// <summary>
/// A stand-in for the JDK's <c>java.util.Observable</c>, which .NET does not have. It
/// behaves like the JDK class in the ways this example can see.
/// <list type="bullet">
///   <item><see cref="AddObserver"/> ignores an observer that is already in the list.</item>
///   <item><see cref="NotifyObservers()"/> does nothing unless <see cref="SetChanged"/> was
///   called first, and it clears the flag before it notifies.</item>
///   <item>Observers are told in the reverse order of addition. The JDK documentation says
///   the order is unspecified; its code walks the list from the end. This class does the
///   same, so the output matches the Java.</item>
/// </list>
/// <para>
/// The JDK deprecated <c>Observable</c> and <c>Observer</c> in Java 9. Its note says the
/// event model is quite limited, the order of notifications is unspecified, and state changes
/// are not in one-for-one correspondence with notifications. <c>Observable</c> is also a
/// class, so a subject that extends it cannot extend anything else.
/// </para>
/// </summary>
public class Observable
{
    private readonly List<IObserver> observers = [];
    private bool changed;
    private readonly object gate = new();

    public void AddObserver(IObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        lock (gate)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }
    }

    public void DeleteObserver(IObserver observer)
    {
        lock (gate)
        {
            observers.Remove(observer);
        }
    }

    public void DeleteObservers()
    {
        lock (gate)
        {
            observers.Clear();
        }
    }

    public int CountObservers()
    {
        lock (gate)
        {
            return observers.Count;
        }
    }

    public void NotifyObservers()
    {
        NotifyObservers(null);
    }

    /// <summary>
    /// Tells every observer, last added first, if this object has changed, and then marks it
    /// unchanged. The list is copied first, as the JDK does, so the observers are called
    /// outside the lock.
    /// </summary>
    public void NotifyObservers(object? arg)
    {
        IObserver[] snapshot;
        lock (gate)
        {
            if (!changed)
            {
                return;
            }
            snapshot = observers.ToArray();
            ClearChanged();
        }

        for (int i = snapshot.Length - 1; i >= 0; i--)
        {
            snapshot[i].Update(this, arg);
        }
    }

    protected void SetChanged()
    {
        lock (gate)
        {
            changed = true;
        }
    }

    protected void ClearChanged()
    {
        lock (gate)
        {
            changed = false;
        }
    }

    public bool HasChanged()
    {
        lock (gate)
        {
            return changed;
        }
    }
}
