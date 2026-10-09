namespace dev.kaldiroglu.Visitor.File.Pattern1;

public class TextFile : File
{
    public TextFile(string name) : base(name)
    {
    }

    public override void Open()
    {
        Console.WriteLine("\nOpening the file: " + name);
    }

    public override void Read()
    {
        Console.WriteLine("Reading the file: " + name);
    }

    public override void Close()
    {
        Console.WriteLine("Closing the file: " + name);
    }

    public override bool Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
