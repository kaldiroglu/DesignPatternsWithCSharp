namespace dev.kaldiroglu.Visitor.File.Problem2;

/// <summary>
/// Three files, each a text file or an XML file at random. The type tests and the casts
/// have moved into <see cref="FileOperator"/>.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. The output changes from run to run. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- file-problem2</c>.
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

        FileOperator fo = new FileOperator();
        fo.Operate(files);
    }
}
