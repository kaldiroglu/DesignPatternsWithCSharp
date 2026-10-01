namespace dev.kaldiroglu.Command.Ac;

public class Temperature
{
    public Temperature(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Java's <c>getTemperature()</c> and <c>setTemperature()</c>. C# does not allow a member
    /// to share its enclosing type's name, so the property is <c>Value</c>.
    /// </summary>
    public int Value { get; set; }
}
