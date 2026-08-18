namespace dev.kaldiroglu.Bridge.Violation;

/// <summary>
/// A supertype with a published contract: <i>calling <c>DoIt</c> prints something.</i>
/// <para>
/// This namespace is not a Bridge. It is the argument <i>for</i> one — the thing that goes
/// wrong when an implementation is supplied by subclassing instead of by delegation. Read it
/// before <c>Bridge.Basic.Problem</c> or straight after it.
/// </para>
/// </summary>
public class AType
{
    protected int AnIntVariable;
    protected bool ABoolVariable;

    public AType(int anIntVariable, bool aBoolVariable)
    {
        AnIntVariable = anIntVariable;
        ABoolVariable = aBoolVariable;
    }

    public int IntVariable
    {
        get => AnIntVariable;
        set => AnIntVariable = value;
    }

    public bool BoolVariable
    {
        get => ABoolVariable;
        set => ABoolVariable = value;
    }

    /// <summary>
    /// Prints. That is the contract every caller of this type is entitled to rely on.
    /// <para>
    /// C# makes the author write <c>virtual</c> before anyone can override this, which is one
    /// more deliberate step than Java asks for — and still not a defence. Opting in to
    /// overriding is not the same as sanctioning every override.
    /// </para>
    /// </summary>
    public virtual void DoIt()
    {
        Console.WriteLine(ABoolVariable
            ? $"My variable: {AnIntVariable}"
            : "Nothing happened!");
    }
}
