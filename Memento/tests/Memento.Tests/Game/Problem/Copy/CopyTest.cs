namespace dev.kaldiroglu.Memento.Tests.Game.Problem.Copy;

// These directives are inside the namespace on purpose. The test namespace Tests.Game
// would otherwise hide the class Game.
using dev.kaldiroglu.Memento.Game.Problem.Copy;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;

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
}
