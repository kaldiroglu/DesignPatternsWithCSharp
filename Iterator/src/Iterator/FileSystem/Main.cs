namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>
/// The <b>Client</b>. Run it with <c>dotnet run --project src/Iterator.Demo -- filesystem</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        Directory userDir = new Directory("/Users/akin");
        userDir.List();

        Directory devDir = new Directory("Dev", userDir); // /Users/akin/Dev directory
        File file1 = new File("Readme.txt", devDir);    // /Users/akin/Dev/Readme.txt file
        File file2 = new File("Report.docs", devDir);   // /Users/akin/Dev/Report.docs file
        File file3 = new File("Selam.java", devDir);    // /Users/akin/Dev/Selam.java file

        Directory reportDir = new Directory("Reports", devDir); // /Users/akin/Dev/Reports directory
        File file4 = new File("ImportantReport.docs", reportDir);   // /Users/akin/Dev/Reports/ImportantReport.docs file
        File file5 = new File("SelamTest.java", reportDir);         // /Users/akin/Dev/Reports/SelamTest.java file

        Console.WriteLine("\nIterating");
        DirectoryIterator<IStorage> iterator = devDir.GetEnumerator();
        while (iterator.MoveNext())
            Console.WriteLine(iterator.Current);

        Console.WriteLine("\nIterating");
        iterator = reportDir.GetEnumerator();
        while (iterator.MoveNext())
            Console.WriteLine(iterator.Current);
    }
}
