namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>
/// The kind of a maintenance request.
/// <para>
/// In the Java this is the enum <c>Request.Kind</c>, nested in the record. A C# record
/// cannot hold both a property <c>Kind</c> and a nested type <c>Kind</c>, so the enum is a
/// type of its own here.
/// </para>
/// </summary>
public enum RequestKind
{
    BUG,
    UI_CHANGE,
    IMPROVEMENT,
    PROJECT
}
