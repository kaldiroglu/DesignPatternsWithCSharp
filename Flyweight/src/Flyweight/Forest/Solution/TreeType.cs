namespace dev.kaldiroglu.Flyweight.Forest.Solution;

/// <summary>
/// ConcreteFlyweight: everything that is true of a kind of tree rather than of one tree. Its
/// name, its color and its texture. Immutable, and created only by <see cref="TreeFactory"/>.
/// The position is not here: <see cref="Draw"/> receives it as a parameter, so one instance
/// can be drawn at ten thousand different places without holding any of them.
/// </summary>
public sealed class TreeType
{
    private readonly string _name;
    private readonly string _color;
    private readonly byte[] _texture;

    internal TreeType(string name, string color, byte[] texture)
    {
        _name = name;
        _color = color;
        _texture = (byte[])texture.Clone(); // loaded once, for every tree of this kind
    }

    /// <summary>Extrinsic state arrives as arguments.</summary>
    public void Draw(int x, int y)
    {
        Console.WriteLine($"Drawing {_color} {_name} at ({x},{y}) with {_texture.Length} texture bytes");
    }

    public string GetName() => _name;

    public string GetColor() => _color;

    public int TextureBytes() => _texture.Length;
}
