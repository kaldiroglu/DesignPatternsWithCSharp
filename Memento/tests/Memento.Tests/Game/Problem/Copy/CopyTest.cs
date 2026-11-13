namespace dev.kaldiroglu.Memento.Tests.Game.Problem.Copy;

// These directives are inside the namespace on purpose. The test namespace Tests.Game
// would otherwise hide the class Game.
using System.Reflection;
using dev.kaldiroglu.Memento.Game.Problem.Copy;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;
using static dev.kaldiroglu.Memento.Tests.Printed;

/// <summary>Stage three: the game keeps a copy of the player, and the copy shares the inventory list.</summary>
public class CopyTest
{
    /// <summary>After loading the player has health 100 at the bridge, but carries [shield, potion], not [sword, shield].</summary>
    [Fact]
    public void TheWrongInventory()
    {
        Player player = new Player();
        Game game = new Game();
        player.PickUp("sword");
        player.PickUp("shield");
        player.MoveTo("bridge");
        game.Checkpoint(player);
        player.Drop("sword");
        player.PickUp("potion");
        player.MoveTo("cave");
        player.TakeDamage(100);
        game.Load(player);
        Assert.Equal("health 100, at bridge, carrying [shield, potion]", player.ToString());
    }

    /// <summary>A copy shares the list: what the player picks up, the copy picks up too.</summary>
    [Fact]
    public void TheListIsShared()
    {
        Player player = new Player();
        Player copy = new Player(player);
        player.PickUp("sword");
        Assert.Equal("health 100, at start, carrying [sword]", copy.ToString());
    }

    /// <summary>Stage three keeps the rules: the player has no public setters.</summary>
    [Fact]
    public void NoPublicSetters()
    {
        Assert.Empty(PublicSettersOf(typeof(Player)));
    }

    /// <summary>Main prints the play: after loading the player carries <c>[shield, potion]</c>.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "At the checkpoint: health 100, at bridge, carrying [sword, shield]",
            "In the cave:       health 0, at cave, carrying [shield, potion]",
            "After loading:     health 100, at bridge, carrying [shield, potion]",
            "Health and place are back, but the inventory is the one from the cave."
        }, By(Main.Run));
    }

    /// <summary>The game keeps a whole Player, so every public method of Player works on the save.</summary>
    [Fact]
    public void TheSaveIsAWholePlayer()
    {
        FieldInfo checkpoint = typeof(Game).GetField("checkpoint", BindingFlags.Instance | BindingFlags.NonPublic)!;
        Assert.Equal(typeof(Player), checkpoint.FieldType);
        var methods = typeof(Player)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => m.Name)
            .Order(StringComparer.Ordinal)
            .ToList();
        Assert.Equal(["Drop", "MoveTo", "PickUp", "RestoreFrom", "TakeDamage", "ToString"], methods);
    }

    /// <summary>The load screen reads the save: health, position and inventory.</summary>
    [Fact]
    public void TheGameReadsTheSave()
    {
        Assert.Equal(["Saved game: health 100, at bridge, carrying [sword, shield]"], By(ReadTheSaveMain.Run));
    }
}
