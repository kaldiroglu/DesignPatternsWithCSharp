namespace dev.kaldiroglu.Visitor.File.Domain;

public class XMLFile : File
{
    public XMLFile(string name) : base(name)
    {
    }

    public override void Open()
    {
        Console.WriteLine();
        Console.WriteLine(name + " is opened.");
    }

    public override void Read()
    {
        Console.WriteLine(name + " is read.");
    }

    public override void Close()
    {
        Console.WriteLine(name + " is closed.");
    }
}
