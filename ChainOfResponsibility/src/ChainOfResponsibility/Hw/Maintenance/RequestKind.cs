namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>
/// The kind of a maintenance request.
/// <para>
/// A C# record cannot hold both a property <c>Kind</c> and a nested type <c>Kind</c>, so the
/// enum is a type of its own.
/// </para>
/// </summary>
public enum RequestKind
{
    BUG,
    UI_CHANGE,
    IMPROVEMENT,
    PROJECT
}
