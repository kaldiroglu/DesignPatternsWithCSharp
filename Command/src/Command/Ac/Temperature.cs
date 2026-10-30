namespace dev.kaldiroglu.Command.Ac;

public class Temperature
{
    public Temperature(int value)
    {
        Value = value;
    }

    /// <summary>
    /// The temperature. C# does not allow a member to share its enclosing type's name, so the
    /// property is <c>Value</c>.
    /// </summary>
    public int Value { get; }
}
