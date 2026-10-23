namespace dev.kaldiroglu.Observer.Payment;

/// <summary>
/// An observer interface, written for this example: .NET does not have one in this shape.
/// <para>
/// One method, called by an <see cref="Observable"/> when it has changed. The first
/// argument is the observable itself; the second is whatever it passed to
/// <see cref="Observable.NotifyObservers(object?)"/>, or <c>null</c>.
/// </para>
/// <para>
/// This is not .NET's <see cref="IObserver{T}"/>. That interface belongs to a different
/// design, in which the subject pushes values and also reports errors and completion.
/// </para>
/// </summary>
public interface IObserver
{
    void Update(Observable observable, object? arg);
}
