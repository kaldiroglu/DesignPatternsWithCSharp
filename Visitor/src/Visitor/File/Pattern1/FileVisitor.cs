namespace dev.kaldiroglu.Visitor.File.Pattern1;

public class FileVisitor : IVisitor
{
    public bool Visit(TextFile file)
    {
        Console.WriteLine("Checking the format of the file: " + file.Name);
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

    public bool Visit(XMLFile file)
    {
        Console.WriteLine(file.Name + " is being validated.");
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
