namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

/// <summary>
/// The <b>narrow interface</b> reified as a first-class object (GoF technique (b), p. 143). A
/// <see cref="DelegatingPowerAdapter"/> forwards its Target operations to whichever <c>PowerDelivery</c>
/// it is given &ndash; pluggability through <b>composition</b>.
/// </summary>
public interface PowerDelivery
{
    void Deliver();

    void Cut();
}
