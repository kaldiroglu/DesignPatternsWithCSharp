namespace dev.kaldiroglu.Memento.Tests.Game.Problem.Setters;

// These directives are inside the namespace on purpose. The test namespace Tests.Game
// would otherwise hide the class Game.
using dev.kaldiroglu.Memento.Game.Problem.Setters;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;

/// <summary>Stage one: the game reads every field out and writes every field back.</summary>
public class SettersTest
{
    private static Player PlayedAndLoaded()
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
        return player;
    }

    /// <summary>It works: loading gives back health 100, the bridge, sword and shield.</summary>
    [Fact]
    public void LoadingIsCorrect()
    {
        Player player = PlayedAndLoaded();
        Assert.Equal(100, player.Health);
        Assert.Equal("bridge", player.Position);
        Assert.Equal(new[] { "sword", "shield" }, player.Inventory);
    }

    /// <summary>
    /// Any code may now set Health to 999, and the player cannot refuse. The Java setters
    /// setHealth, setInventory and setPosition are the setters of the C# properties, named
    /// set_Health, set_Inventory and set_Position.
    /// </summary>
    [Fact]
    public void AnyoneMaySetHealth()
    {
        Player player = PlayedAndLoaded();
        player.Health = 999;
        Assert.Equal(999, player.Health);
        Assert.Equal(new[] { "set_Health", "set_Inventory", "set_Position" }, PublicSettersOf(typeof(Player)));
    }
}
