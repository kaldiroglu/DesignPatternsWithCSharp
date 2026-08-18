namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// One axis of the problem: who owns the document, and therefore how long it is kept.
/// <para>
/// Finance must keep an audit trail; insurance must not keep more than it needs. The two rules
/// pull in opposite directions, which is why neither can be a property of a store.
/// </para>
/// </summary>
public enum Department
{
    /// <summary>Seven years of audit trail, which here is the last five versions.</summary>
    Finance,

    /// <summary>Data minimization: no more than the last two versions may be retained.</summary>
    Insurance
}

public static class DepartmentRetention
{
    public static int RetainedVersions(this Department department) => department switch
    {
        Department.Finance => 5,
        Department.Insurance => 2,
        _ => throw new ArgumentOutOfRangeException(nameof(department))
    };
}
