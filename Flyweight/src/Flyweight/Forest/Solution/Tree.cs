namespace dev.kaldiroglu.Flyweight.Forest.Solution;

/// <summary>
/// One tree in the forest: a position, and a reference to the kind it is. Three fields where
/// the problem version had five, and the fields it dropped were the ones that were identical
/// on every tree of the same kind.
/// </summary>
public class Tree
{
    private readonly int _x;
    private readonly int _y;
    private readonly TreeType _type; // shared with every other tree of this kind

    public Tree(int x, int y, TreeType type)
    {
        _x = x;
        _y = y;
        _type = type;
    }

    /// <summary>Passes this tree's position, the extrinsic state, into the shared flyweight.</summary>
    public void Draw() => _type.Draw(_x, _y);

    public TreeType Type() => _type;
}
