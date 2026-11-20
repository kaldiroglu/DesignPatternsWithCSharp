namespace dev.kaldiroglu.Memento.Tests.Game.Problem.CopyFix;

// These directives are inside the namespace on purpose. The test namespace Tests.Game
// would otherwise hide the class Game.
using System.Reflection;
using dev.kaldiroglu.Memento.Game.Problem.CopyFix;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Printed;

/// <summary>Stage three with the quick fix: the list is copied when saving and when loading.</summary>
public class CopyFixTest
{
    /// <summary>Main: loading gives back [sword, shield], and a key picked up after the load stays out of the save.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(
            [
                "After loading:          health 100, at bridge, carrying [sword, shield]",
                "After picking up a key: health 100, at bridge, carrying [sword, shield, key]",
                "The save still holds:   Saved game: health 100, at bridge, carrying [sword, shield]",
            ],
            By(Main.Run));
    }

    /// <summary>The fix is two lines: the inventory line of the copy constructor and of RestoreFrom.</summary>
    [Fact]
    public void TheFixIsTwoLines()
    {
        static List<string> Lines(string path) => CodeOf(path).Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.Length > 0 && !line.StartsWith("namespace"))
            .ToList();

        var before = Lines("Game/Problem/Copy/Player.cs");
        var after = Lines("Game/Problem/CopyFix/Player.cs");

        Assert.Equal(before.Count, after.Count);
        var changed = before.Zip(after)
            .Where(pair => pair.First != pair.Second)
            .Select(pair => pair.First + " -> " + pair.Second)
            .ToList();
        Assert.Equal(
            [
                "inventory = other.inventory; -> inventory = new List<string>(other.inventory);",
                "inventory = copy.inventory; -> inventory = new List<string>(copy.inventory);",
            ],
            changed);
    }

    /// <summary>The game can still read and change the save, because it is still a whole Player.</summary>
    [Fact]
    public void TheSaveIsStillAPlayer()
    {
        var field = typeof(Game).GetField("checkpoint", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(typeof(Player), field!.FieldType);
    }
}
