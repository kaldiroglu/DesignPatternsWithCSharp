namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>A document that prints each operation.</summary>
public class MyDocument(string name) : Document(name)
{
    internal override void Open()
    {
        Console.WriteLine("Document " + Name + " is opened.");
    }

    internal override void Read()
    {
        Console.WriteLine("Document " + Name + " is read.");
    }

    internal override void Save()
    {
        Console.WriteLine("Document " + Name + " is saved.");
    }

    internal override void Close()
    {
        Console.WriteLine("Document " + Name + " is closed.");
    }
}
