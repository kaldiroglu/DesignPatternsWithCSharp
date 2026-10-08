namespace dev.kaldiroglu.State.Hw.Document;

/// <summary>
/// The statuses of a document. They hold no rules; the <see cref="Workflow"/> does.
/// <para>
/// The constants keep the Java names, because <see cref="Document"/> prints them in its
/// error message (<c>IN_REVIEW</c> becomes <c>in_review</c>).
/// </para>
/// </summary>
public enum Status
{
    DRAFT,
    IN_REVIEW,
    PUBLISHED,
    ARCHIVED
}
