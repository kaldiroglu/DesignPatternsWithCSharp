namespace dev.kaldiroglu.Flyweight.Forest.Problem;

/// <summary>
/// A tree that owns everything about itself. Five fields, of which three (name, color and
/// texture) are the same for every tree of the same kind. Plant ten thousand oaks and you
/// have ten thousand textures. The texture is copied on purpose, which is what a loader
/// reading a texture per tree would really do, and <see cref="TextureLoader"/> counts it.
/// </summary>
public class Tree
{
    private readonly int _x;
    private readonly int _y;
    private readonly string _name;
    private readonly string _color;
    private readonly byte[] _texture;

    public Tree(int x, int y, string name, string color, byte[] texture)
    {
        _x = x;
        _y = y;
        _name = name;
        _color = color;
        _texture = TextureLoader.Load(texture); // this tree's own bytes
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing {_color} {_name} at ({_x},{_y}) with {_texture.Length} texture bytes");
    }

    /// <summary>Bytes retained by this one tree.</summary>
    public int TextureBytes() => _texture.Length;
}
