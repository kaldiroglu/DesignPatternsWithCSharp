namespace dev.kaldiroglu.State.Hw.Document;

/// <summary>
/// What a user can ask a document to do.
/// <para>
/// The name is the same as .NET's <c>System.Action</c> delegate. Inside this namespace
/// <c>Action</c> means this enum, because a type in the current namespace wins over one
/// brought in by an implicit <c>using</c>.
/// </para>
/// </summary>
public enum Action
{
    SUBMIT,
    APPROVE,
    REJECT,
    ARCHIVE
}
