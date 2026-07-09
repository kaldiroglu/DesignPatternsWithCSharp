namespace DevKaldiroglu.DP.Structural.Composite.Problem;

public class File
{
    public string Name { get; }
    public long Size { get; }
    public File(string name, long size) { Name = name; Size = size; }
}

public class Directory
{
    private readonly List<object> _children = new();
    public string Name { get; }
    public Directory(string name) { Name = name; }

    public void Add(object child)
    {
        if (child is not (File or Directory)) throw new ArgumentException("Unknown child type");
        _children.Add(child);
    }

    public long Size()
    {
        long total = 0;
        foreach (var c in _children)
        {
            if (c is File f)        total += f.Size;
            else if (c is Directory d) total += d.Size();
        }
        return total;
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var root = new Directory("root");
        root.Add(new File("readme.txt", 100));
        var src = new Directory("src");
        src.Add(new File("Main.cs", 500));
        src.Add(new File("Utils.cs", 300));
        root.Add(src);
        Console.WriteLine($"Root total size: {root.Size()}");
    }
}
