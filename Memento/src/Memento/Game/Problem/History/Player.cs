using System.Globalization;

namespace dev.kaldiroglu.Memento.Game.Problem.History;

/// <summary>
/// Stage two: the player keeps its own checkpoints.
/// <para>
/// Nothing outside sees the fields, and loading is correct. But the player class now also
/// manages save slots: how many to keep, which one to load, when to throw old ones away.
/// Those are decisions of the game, not of the player, and every other object the game wants
/// to save — enemies, doors, chests — needs the same code.
/// </para>
/// </summary>
public sealed class Player
{
    private sealed record Saved(int Health, string Position, IReadOnlyList<string> Inventory);

    private int health = 100;
    private string position = "start";
    private readonly List<string> inventory = new List<string>();
    private readonly Stack<Saved> checkpoints = new Stack<Saved>();

    public void TakeDamage(int amount)
    {
        health = Math.Max(0, health - amount);
    }

    public void MoveTo(string place)
    {
        position = place;
    }

    public void PickUp(string item)
    {
        inventory.Add(item);
    }

    public void Drop(string item)
    {
        inventory.Remove(item);
    }

    public void SaveCheckpoint()
    {
        checkpoints.Push(new Saved(health, position, inventory.ToList().AsReadOnly()));
    }

    public void LoadCheckpoint()
    {
        Saved saved = checkpoints.Peek();
        health = saved.Health;
        position = saved.Position;
        inventory.Clear();
        inventory.AddRange(saved.Inventory);
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"health {health}, at {position}, carrying [{string.Join(", ", inventory)}]");
}
