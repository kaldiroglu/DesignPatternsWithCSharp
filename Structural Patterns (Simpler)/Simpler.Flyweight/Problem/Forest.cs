namespace DevKaldiroglu.DP.Structural.Flyweight.Problem;

public class Tree
{
    private readonly int _x, _y;
    private readonly string _name;
    private readonly string _color;
    private readonly byte[] _texture; // each Tree carries its own copy

    public Tree(int x, int y, string name, string color, byte[] texture)
    {
        _x = x; _y = y; _name = name; _color = color; _texture = texture;
    }

    public void Draw() =>
        Console.WriteLine($"Drawing {_color} {_name} at ({_x},{_y}) with {_texture.Length} texture bytes");
}

public class Forest
{
    private readonly List<Tree> _trees = new();

    public void Plant(int x, int y, string name, string color, byte[] texture) =>
        _trees.Add(new Tree(x, y, name, color, texture));

    public int Size => _trees.Count;
}

public static class ProblemDemo
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
    }
}
