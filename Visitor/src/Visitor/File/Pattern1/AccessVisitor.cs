namespace dev.kaldiroglu.Visitor.File.Pattern1;

/// <summary>A second operation on the same files: anyone may read a text file, only an admin an XML file.</summary>
public class AccessVisitor(bool admin) : IVisitor
{
    public bool Visit(TextFile file)
    {
        Console.WriteLine("Anyone may read the file: " + file.Name);
        return true;
    }

    public bool Visit(XMLFile file)
    {
        Console.WriteLine((admin ? "An admin may read the file: " : "Only an admin may read the file: ") + file.Name);
        return admin;
    }
}
