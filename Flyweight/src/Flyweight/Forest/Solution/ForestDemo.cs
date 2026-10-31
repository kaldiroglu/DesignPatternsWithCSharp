using System.Globalization;

namespace dev.kaldiroglu.Flyweight.Forest.Solution;

/// <summary>The same forest, planted through a factory. Same trees on screen, two textures in memory.</summary>
public static class ForestDemo
{
    public const int TreesPerKind = 5_000;
    public const int TextureSize = 4_096;

    public static void Run()
    {
        var loader = new TextureLoader(TextureSize);
        var forest = new Forest(new TreeFactory(loader));

        for (int i = 0; i < TreesPerKind; i++)
        {
            forest.Plant(i, i, "Oak", "Green");
            forest.Plant(i, -i, "Pine", "DarkGreen");
        }

        Console.WriteLine($"Trees planted        : {forest.Size()}");
        Console.WriteLine($"Distinct TreeTypes   : {forest.DistinctTypes()}");
        Console.WriteLine($"Textures loaded      : {loader.LoadCount()}");
        Console.WriteLine($"Texture bytes held   : {forest.TextureBytes().ToString("N0", CultureInfo.InvariantCulture)}");
        Console.WriteLine();
        Console.WriteLine($"The same {forest.Size()} trees are on screen. Two textures are in memory.");
    }
}
