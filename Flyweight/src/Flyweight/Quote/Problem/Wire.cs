using System.Text;

namespace dev.kaldiroglu.Flyweight.Quote.Problem;

/// <summary>
/// Decodes a field out of a network buffer, the way a real feed handler does. With string
/// literals the problem would not appear: <c>"AAPL"</c> is interned by the compiler, so a
/// million ticks share one object. A real handler receives bytes, and decoding them produces
/// a new string every time. Every decode here allocates, and every decode is counted.
/// </summary>
public static class Wire
{
    private static int _decoded;

    /// <summary>Decodes one field. A new string object every call.</summary>
    public static string Decode(string field)
    {
        _decoded++;
        return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(field));
    }

    public static int DecodeCount() => _decoded;

    public static void Reset() => _decoded = 0;
}
