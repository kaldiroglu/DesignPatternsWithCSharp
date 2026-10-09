namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>
/// Interpreter: a small rule language for a shop, such as
/// "category is books and price below 50".
/// <para>
/// Each grammar rule is one class, and a sentence is a tree of them: a Composite. Every class
/// has <see cref="Interpret"/>, which evaluates its part of the sentence against a product —
/// the context. GoF's own sample code interprets Boolean expressions in the same way.
/// </para>
/// <para>
/// The operations are inside the classes. That suits a small language with few operations.
/// When the operations grow — printing, checking, optimizing — GoF suggest moving them into
/// visitors, so the rule classes stay small.
/// </para>
/// <para>
/// The Java interface is <c>sealed</c> and lists its five classes. C# cannot close an
/// interface to a fixed list of classes, so this one is open; each record is sealed.
/// </para>
/// </summary>
public interface IRule
{
    bool Interpret(Product product);

    string Describe();
}
