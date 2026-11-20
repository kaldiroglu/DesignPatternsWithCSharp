using System.Globalization;

namespace dev.kaldiroglu.Memento.Game.Problem.CopyFix;

/// <summary>
/// Stage three with the quick fix: the list is copied when the game saves and again when it
/// loads.
/// <para>
/// The two lines that differ from <c>Problem.Copy.Player</c> are the inventory lines in the
/// copy constructor and in <c>RestoreFrom</c>. Each one makes a new list.
/// </para>
/// <list type="bullet">
///   <item>Without the copy in the constructor, the save and the player share one list, so
///   what the player drops after the save is gone from the save too.</item>
///   <item>Without the copy in <c>RestoreFrom</c>, the player takes the save's own list when
///   it loads, so what it picks up after the load goes into the save too.</item>
/// </list>
/// <para>
/// The checkpoint is still a whole <c>Player</c>, so the game can still read and change it.
/// Copying the list does not fix that.
/// </para>
/// </summary>
public sealed class Player
{
    private int health = 100;
    private string position = "start";
    private List<string> inventory = new List<string>();

    public Player()
    {
    }

    /// <summary>A copy of another player, with its own copy of the list.</summary>
    public Player(Player other)
    {
        health = other.health;
        position = other.position;
        inventory = new List<string>(other.inventory);
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

    /// <summary>Takes the values back from a copy, field by field, and copies the list again.</summary>
    public void RestoreFrom(Player copy)
    {
        health = copy.health;
        position = copy.position;
        inventory = new List<string>(copy.inventory);
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"health {health}, at {position}, carrying [{string.Join(", ", inventory)}]");
}
