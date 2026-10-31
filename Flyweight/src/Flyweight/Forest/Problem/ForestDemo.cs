using System.Globalization;

namespace dev.kaldiroglu.Flyweight.Forest.Problem;

/// <summary>
/// Plants a forest the obvious way and reports what it cost. The numbers are smaller than a
/// real game's, so that the example can be run.
/// </summary>
public static class ForestDemo
{
    public const int TreesPerKind = 5_000;
    public const int TextureSize = 4_096;

    public static void Run()
    {
        TextureLoader.Reset();
        var forest = new Forest();

        var oakTexture = new byte[TextureSize];
        var pineTexture = new byte[TextureSize];

        for (int i = 0; i < TreesPerKind; i++)
        {
            forest.Plant(i, i, "Oak", "Green", oakTexture);
            forest.Plant(i, -i, "Pine", "DarkGreen", pineTexture);
        }

        Console.WriteLine($"Trees planted        : {forest.Size()}");
        Console.WriteLine("Distinct kinds       : 2");
        Console.WriteLine($"Textures loaded      : {TextureLoader.LoadCount()}");
        Console.WriteLine($"Texture bytes held   : {forest.TextureBytes().ToString("N0", CultureInfo.InvariantCulture)}");
        Console.WriteLine();
        Console.WriteLine("Two kinds of tree, and every one of them paid for its own texture.");
    }
}
