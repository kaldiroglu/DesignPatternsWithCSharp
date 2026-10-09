namespace dev.kaldiroglu.Visitor.File.Problem1;

/// <summary>
/// Three files, each a text file or an XML file at random. The client tests the type of each
/// file and casts it, to call <c>CheckFormat</c> or <c>Validate</c>.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. The output changes from run to run. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- file-problem1</c>.
/// </remarks>
public static class Test
{
    private static readonly List<File> files = new();

    public static void Run()
    {
        File? file = null;

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
            if (aFile is XMLFile)
            {
                XMLFile xmlFile = (XMLFile)aFile;
                bool valid = xmlFile.Validate();
                if (valid)
                {
                    xmlFile.Read();
                }
            }
            else
            {
                TextFile textFile = (TextFile)aFile;
                bool formatted = textFile.CheckFormat();
                if (formatted)
                {
                    textFile.Read();
                }
            }
            aFile.Close();
        }
    }
}
