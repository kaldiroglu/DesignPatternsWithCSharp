namespace dev.kaldiroglu.Visitor.File.Problem1;

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

    public bool CheckFormat()
    {
        Console.WriteLine("Checking the format of the file: " + name);
        double random = Random.Shared.NextDouble();
        if (random < 0.80)
        {
            Console.WriteLine("It is a formatted TXT file");
            return true;
        }
        else
        {
            Console.WriteLine("It is not a formatted TXT file");
            return false;
        }
    }
}
