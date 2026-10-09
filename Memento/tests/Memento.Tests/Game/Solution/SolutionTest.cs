namespace dev.kaldiroglu.Memento.Tests.Game.Solution;

// These directives are inside the namespace on purpose. The test namespace Tests.Game
// would otherwise hide the class Game, and Problem would mean Tests.Game.Problem.
using System.Reflection;
using dev.kaldiroglu.Memento.Game.Solution;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;
using static dev.kaldiroglu.Memento.Tests.Printed;
using CopyPlayer = dev.kaldiroglu.Memento.Game.Problem.Copy.Player;
using SettersPlayer = dev.kaldiroglu.Memento.Game.Problem.Setters.Player;

/// <summary>The checkpoint as a memento. Every figure on the Part 3 slides is asserted here.</summary>
public class SolutionTest
{
    /// <summary>Loading gives back exactly what the player had: health 100, the bridge, sword and shield.</summary>
    [Fact]
    public void ThePromiseIsKept()
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
        game.LoadLatest(player);
        Assert.Equal("health 100, at bridge, carrying [sword, shield]", player.ToString());
        Assert.Equal(1, game.CheckpointCount);
    }

    /// <summary>A checkpoint can be loaded twice, because nothing the player does changes it.</summary>
    [Fact]
    public void ACheckpointDoesNotChange()
    {
        Player player = new Player();
        player.PickUp("sword");
        Player.ICheckpoint checkpoint = player.Save();
        player.PickUp("shield");
        player.Load(checkpoint);
        player.PickUp("potion");
        player.Load(checkpoint);
        Assert.Equal("health 100, at start, carrying [sword]", player.ToString());
    }

    /// <summary>
    /// A class outside Player cannot read the checkpoint's health. The Java checks that the
    /// fields health, inventory and position and the constructor of Checkpoint are private,
    /// and that it has no methods. In C# the class Checkpoint is private to Player and holds
    /// Health, Inventory and Position; outside Player it is seen only as the empty interface
    /// ICheckpoint, which has no getters and no methods at all.
    /// </summary>
    [Fact]
    public void TheCheckpointIsClosed()
    {
        Type? type = typeof(Player).GetNestedType("Checkpoint", BindingFlags.NonPublic);
        Assert.NotNull(type);
        Assert.True(type!.IsNestedPrivate);
        Assert.Equal(new[] { "Health", "Inventory", "Position" },
            type.GetProperties().Select(p => p.Name).OrderBy(n => n, StringComparer.Ordinal));

        Type seen = typeof(Player.ICheckpoint);
        Assert.True(seen.IsInterface);
        Assert.True(seen.IsAssignableFrom(type));
        Assert.Empty(seen.GetMembers());   // no getters, no methods at all

        // The caretaker, Game, keeps the checkpoints only as the empty interface.
        Assert.Equal(new[] { typeof(Stack<Player.ICheckpoint>) },
            typeof(Game).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Select(f => f.FieldType));
    }

    /// <summary>Public setters on Player: yes in stage one, no in stage three, no with the memento.</summary>
    [Fact]
    public void PublicSetters()
    {
        Assert.NotEmpty(PublicSettersOf(typeof(SettersPlayer)));
        Assert.Empty(PublicSettersOf(typeof(CopyPlayer)));
        Assert.Empty(PublicSettersOf(typeof(Player)));
    }

    /// <summary>Main runs the same play three ways: only the memento keeps the promise and the rules.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "Stage one, after loading:   health 100, at bridge, carrying [sword, shield]",
            "  and anyone may now write: health 999, at bridge, carrying [sword, shield]",
            "Stage three, after loading: health 100, at bridge, carrying [shield, potion]",
            "Memento, after loading:     health 100, at bridge, carrying [sword, shield]"
        }, By(Main.Run));
    }
}
