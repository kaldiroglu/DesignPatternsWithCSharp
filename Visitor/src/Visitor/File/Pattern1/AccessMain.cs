namespace dev.kaldiroglu.Visitor.File.Pattern1;

/// <summary>The same files with a second visitor. No file class changes.</summary>
public static class AccessMain
{
    public static void Run()
    {
        List<File> files = [new TextFile("Notes.txt"), new XMLFile("Pay.xml")];
        IVisitor visitor = new AccessVisitor(false);

        foreach (File file in files)
        {
            file.Open();
            if (file.Accept(visitor))
                file.Read();
            file.Close();
        }
    }
}
