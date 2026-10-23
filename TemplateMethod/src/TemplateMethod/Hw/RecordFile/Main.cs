namespace dev.kaldiroglu.TemplateMethod.Hw.RecordFile;

/// <summary>
/// Reads a file of customers, then a file with a bad line. The template method closes the
/// reader even when a line fails; the subclass only parses one line.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- hw-recordfile</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var reader = new CustomerFileReader();
        Console.WriteLine("Customers: "
                + Show(reader.ReadAll(new StringReader("Ayse;Istanbul\n\nDeniz ; Izmir\n"))));

        var bad = new ClosingReader("Ayse;Istanbul\nnot a customer\n");
        try
        {
            reader.ReadAll(bad);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Bad line: " + e.Message);
        }
        Console.WriteLine("Reader closed after the bad line: " + (bad.Closed ? "true" : "false"));
    }

    /// <summary>
    /// Prints the customers as <c>[Customer[name=Ayse, city=Istanbul]]</c>. A C# record
    /// prints <c>Customer { Name = Ayse, City = Istanbul }</c>, so the text is built here.
    /// </summary>
    private static string Show(IEnumerable<Customer> customers) =>
        "[" + string.Join(", ", customers.Select(c => "Customer[name=" + c.Name + ", city=" + c.City + "]")) + "]";

    /// <summary>A reader that remembers whether it was closed.</summary>
    private sealed class ClosingReader(string text) : StringReader(text)
    {
        public bool Closed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            Closed = true;
            base.Dispose(disposing);
        }
    }
}
