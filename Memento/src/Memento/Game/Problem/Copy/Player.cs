using System.Globalization;

namespace dev.kaldiroglu.Memento.Game.Problem.Copy;

/// <summary>
/// Stage three: the player can make a copy of itself, and take its values back from a copy.
/// <para>
/// The game keeps the copies, so the player has no save slots and no public setters. Two
/// things go wrong.
/// </para>
/// <list type="bullet">
///   <item>The copy constructor copies the reference to the inventory list, not the list: the
///   copy and the player share one list. Whatever the player picks up or drops after the
///   checkpoint, the checkpoint picks up or drops too. Copying the list fixes this.</item>
///   <item>The checkpoint is a whole <c>Player</c>, so whoever keeps it can call every public
///   method on it: <c>ToString()</c> reads the save, and <c>PickUp</c> changes it. Copying the
///   list does not fix this.</item>
/// </list>
/// </summary>
public sealed class Player
{
    private int health = 100;
    private string position = "start";
    private List<string> inventory = new List<string>();

    public Player()
    {
    }

    /// <summary>A copy of another player — the list is shared, not copied.</summary>
    public Player(Player other)
    {
        health = other.health;
        position = other.position;
        // This copies the reference, not the list, on purpose: the copy and the player share
        // one list. This is the fault that stage three shows.
        inventory = other.inventory;
    }

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

    /// <summary>Takes the values back from a copy, field by field, as the copy constructor does.</summary>
    public void RestoreFrom(Player copy)
    {
        health = copy.health;
        position = copy.position;
        inventory = copy.inventory;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"health {health}, at {position}, carrying [{string.Join(", ", inventory)}]");
}
