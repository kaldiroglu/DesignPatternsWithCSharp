namespace dev.kaldiroglu.Flyweight.Forest.Solution;

/// <summary>A forest whose trees know where they are and nothing else.</summary>
public class Forest
{
    private readonly List<Tree> _trees = [];
    private readonly TreeFactory _factory;

    public Forest(TreeFactory factory)
    {
        _factory = factory;
    }

    public void Plant(int x, int y, string name, string color)
    {
        _trees.Add(new Tree(x, y, _factory.GetType(name, color)));
    }

    public void Render()
    {
        foreach (Tree tree in _trees)
        {
            tree.Draw();
        }
    }

    public int Size() => _trees.Count;

    public int DistinctTypes() => _factory.DistinctTypes();

    public long TextureBytes() => _factory.TextureBytes();
}
