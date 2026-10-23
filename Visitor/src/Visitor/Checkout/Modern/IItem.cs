namespace dev.kaldiroglu.Visitor.Checkout.Modern;

/// <summary>
/// The same items as an interface with record implementations, and one switch for the tax.
/// <para>
/// C# has no sealed interface. Any class may implement this interface, so the compiler cannot
/// know that the four records are all there are. A switch over <c>IItem</c> therefore needs a
/// last arm for "anything else", and a fifth item kind is not a compile error: it reaches
/// that arm at run time. In C#, the visitor's interface is the way to get the check.
/// </para>
/// </summary>
public interface IItem
{
    string Name { get; }

    int Price { get; }
}
