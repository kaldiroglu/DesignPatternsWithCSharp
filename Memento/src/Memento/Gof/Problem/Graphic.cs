namespace dev.kaldiroglu.Memento.Gof.Problem;

/// <summary>A box on the canvas. Only its x position matters here.</summary>
public sealed class Graphic
{
    public Graphic(string name, int x)
    {
        Name = name;
        X = x;
    }

    public string Name { get; }

    public int X { get; private set; }

    public void Move(int dx)
    {
        X += dx;
    }
}
