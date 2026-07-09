namespace DevKaldiroglu.DP.Structural.Flyweight.Solution;

public sealed class TreeType
{
    public string Name { get; }
    public string Color { get; }
    public byte[] Texture { get; }

    public TreeType(string name, string color, byte[] texture)
    {
        Name = name; Color = color; Texture = texture;
    }

    public void Draw(int x, int y) =>
        Console.WriteLine($"Drawing {Color} {Name} at ({x},{y}) with {Texture.Length} texture bytes");
}

public class TreeFactory
{
    private readonly Dictionary<string, TreeType> _types = new();

    public TreeType Get(string name, string color, byte[] texture)
    {
        var key = $"{name}|{color}";
        if (_types.TryGetValue(key, out var existing)) return existing;
        var created = new TreeType(name, color, texture);
        _types[key] = created;
        return created;
    }

    public int DistinctTypes => _types.Count;
}

public class Tree
{
    private readonly int _x, _y;
    private readonly TreeType _type;

    public Tree(int x, int y, TreeType type) { _x = x; _y = y; _type = type; }
    public void Draw() => _type.Draw(_x, _y);
}

public class Forest
{
    private readonly List<Tree> _trees = new();
    private readonly TreeFactory _factory = new();

    public void Plant(int x, int y, string name, string color, byte[] texture) =>
        _trees.Add(new Tree(x, y, _factory.Get(name, color, texture)));

    public int Size => _trees.Count;
    public int DistinctTypes => _factory.DistinctTypes;
}

public static class SolutionDemo
{
    public static void Run()
    {
        var forest = new Forest();
        var oakTexture  = new byte[1_000_000];
        var pineTexture = new byte[1_000_000];

        for (int i = 0; i < 10_000; i++)
        {
            forest.Plant(i,  i, "Oak",  "Green",     oakTexture);
            forest.Plant(i, -i, "Pine", "DarkGreen", pineTexture);
        }
        Console.WriteLine($"Trees planted: {forest.Size}");
        Console.WriteLine($"Distinct TreeTypes in memory: {forest.DistinctTypes}");
    }
}
