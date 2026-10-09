namespace dev.kaldiroglu.Memento.Game.Problem.Setters;

/// <summary>Stage one: the game copies the fields out, and writes them back.</summary>
public sealed class Game
{
    private int savedHealth;
    private string? savedPosition;
    private IReadOnlyList<string>? savedInventory;

    public void Checkpoint(Player player)
    {
        savedHealth = player.Health;
        savedPosition = player.Position;
        savedInventory = player.Inventory;
    }

    public void Load(Player player)
    {
        player.Health = savedHealth;
        player.Position = savedPosition!;
        player.Inventory = savedInventory!;
    }
}
