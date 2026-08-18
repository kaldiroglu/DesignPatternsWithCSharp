namespace dev.kaldiroglu.Bridge.Retrofit;

/// <summary>
/// The Implementor: a vendor's own client library, in the vendor's own vocabulary.
/// <para>
/// Nothing here is designed for us. These are the calls the engine already had when the standard
/// arrived — open a session, pull rows, hand the session back — and they are what a type 2 JDBC
/// driver finds underneath it: a native client library that predates the standard and will
/// outlive this year's version of it.
/// </para>
/// <para>
/// Note what is <i>not</i> here: no <c>Query</c>, no <c>Report</c>, nothing shaped like the
/// interface we are required to expose. That is the point. If this interface mirrored the
/// required one there would be no bridge, only two names for the same thing.
/// </para>
/// </summary>
public interface IVendorClient
{
    /// <summary>What this engine calls itself in a log line.</summary>
    string Name { get; }

    /// <summary>Open a session and return whatever handle this vendor uses for one.</summary>
    string Open(string database);

    /// <summary>Pull rows for a statement written in this vendor's own dialect.</summary>
    IReadOnlyList<string> Pull(string handle, string statement);

    /// <summary>Give the session back.</summary>
    void Release(string handle);
}
