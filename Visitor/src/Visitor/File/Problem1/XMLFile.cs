namespace dev.kaldiroglu.Visitor.File.Problem1;

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

    public bool Validate()
    {
        Console.WriteLine(name + " is being validated.");
        double random = Random.Shared.NextDouble();
        if (random < 0.80)
        {
            Console.WriteLine("It is a valid XML file");
            return true;
        }
        else
        {
            Console.WriteLine("It is not a valid XML file");
            return false;
        }
    }
}
