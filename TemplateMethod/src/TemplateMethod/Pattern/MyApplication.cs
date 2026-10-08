namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>Writes the two abstract steps of the template method.</summary>
public class MyApplication : Application
{
    internal override bool CanOpenDocument(string fileName)
    {
        Console.WriteLine("Checking the document " + fileName);
        return true;
    }

    internal override Document CreateDocument(string fileName)
    {
        Console.WriteLine("Opening document " + fileName);
        return new MyDocument(fileName);
    }
}
