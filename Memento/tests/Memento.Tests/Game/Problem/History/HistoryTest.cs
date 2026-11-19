namespace dev.kaldiroglu.Memento.Tests.Game.Problem.History;

using System.Reflection;
using dev.kaldiroglu.Memento.Game.Problem.History;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Methods;
using static dev.kaldiroglu.Memento.Tests.Printed;

/// <summary>Stage two: the player keeps its own checkpoints.</summary>
public class HistoryTest
{
    /// <summary>Loading is correct, and nothing outside sees the fields.</summary>
    [Fact]
    public void LoadingIsCorrect()
    {
        Player player = new Player();
        player.PickUp("sword");
        player.PickUp("shield");
        player.MoveTo("bridge");
        player.SaveCheckpoint();
        player.Drop("sword");
        player.PickUp("potion");
        player.MoveTo("cave");
        player.TakeDamage(100);
        player.LoadCheckpoint();
        Assert.Equal("health 100, at bridge, carrying [sword, shield]", player.ToString());
        Assert.Empty(PublicSettersOf(typeof(Player)));
        // No public getter: neither a property getter (get_Name) nor a method named Get...
        Assert.DoesNotContain(PublicMethodsOf(typeof(Player)),
            name => name.StartsWith("get_", StringComparison.Ordinal)
                    || name.StartsWith("Get", StringComparison.Ordinal));
    }

    /// <summary>The player class now also keeps the save slots, in a Stack.</summary>
    [Fact]
    public void ThePlayerKeepsTheSlots()
    {
        Assert.Contains(
            typeof(Player).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly),
            f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(Stack<>));
    }

    /// <summary>The game can only say save or load, and loading always gives back the last save.</summary>
    [Fact]
    public void TheGameGetsOnlyTheLastSave()
    {
        Player player = new Player();
        player.MoveTo("bridge");
        player.SaveCheckpoint();
        player.MoveTo("cave");
        player.SaveCheckpoint();
        player.MoveTo("tower");
        player.LoadCheckpoint();
        Assert.Equal("health 100, at cave, carrying []", player.ToString());
        player.LoadCheckpoint();
        Assert.Equal("health 100, at cave, carrying []", player.ToString());

        var saveMethods = typeof(Player)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => m.Name.Contains("Checkpoint"))
            .OrderBy(m => m.Name, StringComparer.Ordinal)
            .ToList();
        Assert.Equal(new[] { "LoadCheckpoint", "SaveCheckpoint" }, saveMethods.Select(m => m.Name));
        // No argument to pick a save, and no save handed back: the game never sees one.
        Assert.All(saveMethods, m =>
        {
            Assert.Empty(m.GetParameters());
            Assert.Equal(typeof(void), m.ReturnType);
        });
    }

    /// <summary>Main prints the play: loading is correct, and the player keeps the saves.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "At the checkpoint: health 100, at bridge, carrying [sword, shield]",
            "In the cave:       health 0, at cave, carrying [shield, potion]",
            "After loading:     health 100, at bridge, carrying [sword, shield]",
            "The save slots are a field of Player, not of the game."
        }, By(Main.Run));
    }
}
