namespace dev.kaldiroglu.Bridge.Retrofit;

/// <summary>
/// The system from last decade, and the reason none of this can be rewritten.
/// <para>
/// It works, it is fast, and it has callers all over the company that use it directly and are
/// not going to stop. <see cref="ReportDirectly"/> is one of them: a caller that predates the
/// standard entirely and must keep working untouched.
/// </para>
/// </summary>
public class LegacyEngine : IVendorClient
{
    private readonly List<string> _calls = [];
    private int _sessions;

    public string Name => "legacy";

    public string Open(string database)
    {
        _sessions++;
        return $"legacy-session-{_sessions}:{database}";
    }

    public IReadOnlyList<string> Pull(string handle, string statement)
    {
        _calls.Add(statement);
        return new[] { $"{handle} | {statement.ToUpperInvariant()}" };
    }

    public void Release(string handle) => _sessions--;

    /// <summary>A caller that has existed for ten years and knows nothing about any standard.</summary>
    public string ReportDirectly(string statement)
    {
        var handle = Open("payroll");
        var row = Pull(handle, statement)[0];
        Release(handle);
        return row;
    }

    /// <summary>So a test can show which statements actually reached the engine.</summary>
    public IReadOnlyList<string> StatementsSeen => _calls.AsReadOnly();

    public int OpenSessions => _sessions;
}
