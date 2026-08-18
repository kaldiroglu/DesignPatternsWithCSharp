namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// The other axis: which vendor actually holds the bytes.
/// <para>
/// Chosen by procurement, per site, and changed when a contract ends. Nothing about this list
/// is derived from <see cref="Department"/>, and nothing about <c>Department</c> is derived
/// from this one — which is the whole reason the two multiply.
/// </para>
/// </summary>
public enum Store
{
    Evernote,
    SharePoint,
    FileNet
}
