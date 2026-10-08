namespace dev.kaldiroglu.TemplateMethod.Hw.RecordFile;

/// <summary>Reads lines like <c>Ayse;Istanbul</c>. A line without ';' is an error.</summary>
public sealed class CustomerFileReader : RecordFileReader<Customer>
{
    protected override Customer Parse(string line)
    {
        string[] parts = SplitLikeJava(line);
        if (parts.Length != 2)
        {
            throw new ArgumentException("not a customer: " + line);
        }
        return new Customer(parts[0].Trim(), parts[1].Trim());
    }

    /// <summary>
    /// Java's <c>line.split(";")</c> drops empty parts at the end, so <c>"Ayse;"</c> is one
    /// part there and an error. C#'s <c>Split</c> keeps them, so they are removed here.
    /// </summary>
    private static string[] SplitLikeJava(string line)
    {
        string[] parts = line.Split(';');
        int count = parts.Length;
        while (count > 0 && parts[count - 1].Length == 0)
        {
            count--;
        }
        return parts[..count];
    }
}
