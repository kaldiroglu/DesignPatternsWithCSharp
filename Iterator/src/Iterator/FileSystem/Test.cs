using dev.kaldiroglu.Iterator.FileSystem.Iterator;

namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>
/// The <b>Client</b>: the Java original's <c>Test</c> class with its <c>main</c> method. Run it
/// with <c>dotnet run --project src/Iterator.Demo -- filesystem</c>.
/// </summary>
public static class Test
{
    public static void Run()
    {
        Directory userDir = new Directory("/Users/akin");
        userDir.List();

        Directory devDir = new Directory("Dev", userDir); // /Users/akin/Dev directory
        File file1 = new File("Readme.txt", devDir);    // /Users/akin/Dev/Readme.txt file
        File file2 = new File("Report.docs", devDir);   // /Users/akin/Dev/Report.docx file
        File file3 = new File("Selam.java", devDir);    // /Users/akin/Dev/Selam.java file

        Directory reportDir = new Directory("Reports", devDir); // /Users/akin/Reports directory
        File file4 = new File("ImportantReport.docs", reportDir);   // /Users/akin/Dev/Report.docx file
        File file5 = new File("SelamTest.java", reportDir);         // /Users/akin/Dev/Selam.java file

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
