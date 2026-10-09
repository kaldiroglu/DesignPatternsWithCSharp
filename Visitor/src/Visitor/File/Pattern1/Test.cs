namespace dev.kaldiroglu.Visitor.File.Pattern1;

/// <summary>
/// Three files, each a text file or an XML file at random. Each file accepts the visitor,
/// and the visitor checks it. There are no type tests and no casts.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. The output changes from run to run. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- file-pattern1</c>.
/// </remarks>
public static class Test
{
    private static readonly List<File> files = new();

    public static void Run()
    {
        File? file = null;
        IVisitor visitor = new FileVisitor();

        for (int i = 0; i < 3; i++)
        {
            double random = Random.Shared.NextDouble();
            if (random < 0.5)
                file = new XMLFile("File-" + i + ".xml");
            else
                file = new TextFile("File-" + i + ".txt");
            files.Add(file);
        }

        foreach (File aFile in files)
        {
            aFile.Open();
            bool condition = aFile.Accept(visitor);
            if (condition)
                aFile.Read();
            aFile.Close();
        }
    }
}
