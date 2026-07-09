namespace DevKaldiroglu.DP.Structural.Composite.Solution;

public interface IFileSystemNode
{
    string Name { get; }
    long Size { get; }
}

public class File : IFileSystemNode
{
    public string Name { get; }
    public long Size { get; }
    public File(string name, long size) { Name = name; Size = size; }
}

public class Directory : IFileSystemNode
{
    private readonly List<IFileSystemNode> _children = new();
    public string Name { get; }
    public Directory(string name) { Name = name; }

    public Directory Add(IFileSystemNode child) { _children.Add(child); return this; }

    public long Size
    {
        get
        {
            long total = 0;
            foreach (var c in _children) total += c.Size;
            return total;
        }
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        IFileSystemNode root = new Directory("root")
            .Add(new File("readme.txt", 100))
            .Add(new Directory("src")
                .Add(new File("Main.cs", 500))
                .Add(new File("Utils.cs", 300)));

        Print(root);
    }

    private static void Print(IFileSystemNode n) =>
        Console.WriteLine($"{n.Name} — {n.Size} bytes");
}
