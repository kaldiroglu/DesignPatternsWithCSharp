namespace dev.kaldiroglu.Visitor.File.Pattern1;

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

    public override bool Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
