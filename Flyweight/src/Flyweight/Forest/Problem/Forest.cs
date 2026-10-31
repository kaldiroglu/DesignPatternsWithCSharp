namespace dev.kaldiroglu.Flyweight.Forest.Problem;

/// <summary>A forest of self-contained trees. Every call to <see cref="Plant"/> allocates a whole one.</summary>
public class Forest
{
    private readonly List<Tree> _trees = [];

    public void Plant(int x, int y, string name, string color, byte[] texture)
    {
        _trees.Add(new Tree(x, y, name, color, texture));
    }

    public void Render()
    {
        foreach (Tree tree in _trees)
        {
            tree.Draw();
        }
    }

    public int Size() => _trees.Count;

    /// <summary>Total texture bytes held by the forest: the number the solution has to beat.</summary>
    public long TextureBytes() => _trees.Sum(tree => (long)tree.TextureBytes());
}
