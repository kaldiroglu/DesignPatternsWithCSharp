using dev.kaldiroglu.Flyweight.Forest.Solution;
using Xunit;
using ProblemForest = dev.kaldiroglu.Flyweight.Forest.Problem.Forest;
using ProblemLoader = dev.kaldiroglu.Flyweight.Forest.Problem.TextureLoader;
using SolutionForest = dev.kaldiroglu.Flyweight.Forest.Solution.Forest;

namespace dev.kaldiroglu.Flyweight.Tests;

/// <summary>The forest example, measured: loads and bytes held, not an adjective.</summary>
public class ForestFlyweightTests
{
    private const int Texture = 4_096;
    private const int PerKind = 5_000;

    [Fact(DisplayName = "Without sharing, every tree loads its own texture")]
    public void ProblemLoadsOnceForEveryTree()
    {
        ProblemLoader.Reset();

        var forest = new ProblemForest();
        var oak = new byte[Texture];
        var pine = new byte[Texture];
        for (int i = 0; i < PerKind; i++)
        {
            forest.Plant(i, i, "Oak", "Green", oak);
            forest.Plant(i, -i, "Pine", "DarkGreen", pine);
        }

        Assert.Equal(10_000, forest.Size());
        Assert.Equal(10_000, ProblemLoader.LoadCount());
        Assert.Equal(10_000L * Texture, forest.TextureBytes());
    }

    [Fact(DisplayName = "With sharing, a texture is loaded once per kind")]
    public void SolutionLoadsOncePerKind()
    {
        var loader = new TextureLoader(Texture);
        var forest = new SolutionForest(new TreeFactory(loader));

        for (int i = 0; i < PerKind; i++)
        {
            forest.Plant(i, i, "Oak", "Green");
            forest.Plant(i, -i, "Pine", "DarkGreen");
        }

        Assert.Equal(10_000, forest.Size());
        Assert.Equal(2, forest.DistinctTypes());
        Assert.Equal(2, loader.LoadCount());
        Assert.Equal(2L * Texture, forest.TextureBytes());
    }

    [Fact(DisplayName = "Every tree of a kind points at the same TreeType")]
    public void TreesOfAKindShareOneType()
    {
        var factory = new TreeFactory(new TextureLoader(Texture));

        TreeType first = factory.GetType("Oak", "Green");
        TreeType second = factory.GetType("Oak", "Green");

        Assert.Same(first, second);
        Assert.Equal(2, factory.RequestCount());
        Assert.Equal(1, factory.DistinctTypes());
    }

    [Fact(DisplayName = "Color is part of the key, so a red oak is a different kind")]
    public void TheKeyCoversAllOfTheIntrinsicState()
    {
        var factory = new TreeFactory(new TextureLoader(Texture));

        TreeType green = factory.GetType("Oak", "Green");
        TreeType autumn = factory.GetType("Oak", "Red");

        Assert.Equal(2, factory.DistinctTypes());
        Assert.Equal("Green", green.GetColor());
        Assert.Equal("Red", autumn.GetColor());
    }

    [Fact(DisplayName = "The factory loads the texture, so a caller cannot supply a conflicting one")]
    public void TheFactoryOwnsTheIntrinsicState()
    {
        bool takesATexture = typeof(TreeFactory).GetMethods()
            .Where(m => m.Name == nameof(TreeFactory.GetType))
            .Any(m => m.GetParameters().Any(p => p.ParameterType == typeof(byte[])));

        Assert.False(takesATexture);
    }

    [Fact(DisplayName = "The texture is loaded per kind, named by the kind that needed it")]
    public void LoadsAreAttributable()
    {
        var loader = new TextureLoader(Texture);
        var forest = new SolutionForest(new TreeFactory(loader));

        forest.Plant(0, 0, "Oak", "Green");
        forest.Plant(1, 1, "Oak", "Green");
        forest.Plant(2, 2, "Pine", "DarkGreen");

        Assert.Equal(["Oak", "Pine"], loader.LoadedKinds());
    }
}
