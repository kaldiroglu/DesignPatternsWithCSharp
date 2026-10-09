using System.Globalization;

namespace dev.kaldiroglu.Memento.Game.Problem.Setters;

/// <summary>
/// Stage one: the game saves the player by reading every field, and loads it by writing
/// every field back.
/// <para>
/// So every field needs a public getter and a public setter. Now any code can call
/// <c>Health = 999</c> — the rule "health only changes through damage and healing" is
/// gone. And the game's save code lists the fields one by one, so a field added later is not
/// saved until someone remembers to add it there too.
/// </para>
/// </summary>
public sealed class Player
{
    private int health = 100;
    private string position = "start";
    private List<string> inventory = new List<string>();

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

    // Needed only so that the game can save and load:
    public int Health { get => health; set => health = value; }
    public string Position { get => position; set => position = value; }

    /// <summary>A copy of the inventory, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Inventory
    {
        get => inventory.ToList().AsReadOnly();
        set => inventory = new List<string>(value);
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"health {health}, at {position}, carrying [{string.Join(", ", inventory)}]");
}
