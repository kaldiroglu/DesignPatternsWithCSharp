namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>Where log lines go, so a test can count them.</summary>
public sealed class CallLog
{
    private readonly List<string> _lines = [];
    private readonly bool _echo;

    public CallLog() : this(false)
    {
    }

    public CallLog(bool echo) => _echo = echo;

    public void Record(string line)
    {
        _lines.Add(line);
        if (_echo)
        {
            Console.WriteLine("    log | " + line);
        }
    }

    public IReadOnlyList<string> Lines() => _lines.AsReadOnly();

    public int Size => _lines.Count;

    public void Clear() => _lines.Clear();
}
