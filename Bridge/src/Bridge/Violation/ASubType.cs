namespace dev.kaldiroglu.Bridge.Violation;

/// <summary>
/// The violation.
/// <para>
/// This subclass overrides <see cref="AType.DoIt"/> to <b>store</b> a string instead of
/// printing one, and adds <see cref="WriteIt"/> to print it later. Nothing here fails to
/// compile, and every test written against <c>ASubType</c> passes.
/// </para>
/// <para>
/// The damage is done to callers that hold an <see cref="AType"/>. They were promised output;
/// given one of these, they get silence, and no exception tells them so. That is a breach of
/// the Liskov Substitution Principle: a subtype must be usable wherever its supertype is, and
/// this one is not.
/// </para>
/// <para>
/// <b>Why it belongs in a Bridge namespace.</b> Subclassing was used here to change <i>how</i>
/// something is done, which is exactly what an implementation is for — and changing behavior
/// by overriding can break a contract the supertype made, silently. Delegating to an
/// implementor cannot: <c>ASubAbstraction</c> in <c>Bridge.Basic.Pattern</c> calls
/// <c>implementation.DoingIt()</c> and remains responsible for its own contract no matter
/// which implementation it holds. That difference is the whole argument for putting the
/// implementation behind a reference rather than above it in a hierarchy.
/// </para>
/// <para>
/// Note also <c>_aStringVariable</c>: it is deliberately left unassigned, so a caller that
/// reaches <see cref="WriteIt"/> without having called <c>DoIt</c> first prints nothing at
/// all. A second broken promise, produced by the same move.
/// </para>
/// </summary>
public class ASubType : AType
{
    private string? _aStringVariable;

    public ASubType(int anIntVariable, bool aBoolVariable)
        : base(anIntVariable, aBoolVariable)
    {
    }

    /// <summary>Stores instead of printing — and the supertype said this method prints.</summary>
    public override void DoIt()
    {
        _aStringVariable = ABoolVariable
            ? $"My variable: {AnIntVariable}"
            : "Nothing happened!";
    }

    public void WriteIt() => Console.WriteLine($"aStringVariable : {_aStringVariable}");

    /// <summary>Exposed so a test can show the string was stored rather than printed.</summary>
    public string? AStringVariable => _aStringVariable;
}
