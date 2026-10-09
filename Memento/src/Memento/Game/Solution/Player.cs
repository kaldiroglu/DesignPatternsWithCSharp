using System.Globalization;

namespace dev.kaldiroglu.Memento.Game.Solution;

/// <summary>
/// The <b>Originator</b>: the player makes its own checkpoint, and loads one.
/// <para>
/// <see cref="Save"/> returns a checkpoint that holds a copy of the state: the health, the
/// position and a copy of the inventory list. Nothing the player does afterwards can change
/// it. Only the player can read it back, in <see cref="Load"/>.
/// </para>
/// </summary>
public sealed class Player
{
    /// <summary>
    /// The <b>Memento</b>, as the game sees it. It has no members: the game can keep it and
    /// hand it back, but cannot look inside or change it.
    /// </summary>
    /// <remarks>
    /// In the Java, the memento is a public nested class with private fields, and the player
    /// reads those fields because it encloses the class. In C# an enclosing class cannot read
    /// a nested class's private members. So the game gets this empty interface, and the class
    /// that holds the state, <see cref="Checkpoint"/>, is private to the player.
    /// </remarks>
    public interface ICheckpoint
    {
    }

    /// <summary>
    /// The state inside the memento. The class is private, so only the player can name it,
    /// create it and read it. Its members are public only inside the player.
    /// </summary>
    private sealed class Checkpoint : ICheckpoint
    {
        public int Health { get; }
        public string Position { get; }
        public IReadOnlyList<string> Inventory { get; }

        public Checkpoint(int health, string position, IReadOnlyList<string> inventory)
        {
            Health = health;
            Position = position;
            Inventory = inventory.ToList().AsReadOnly();
        }
    }

    private int health = 100;
    private string position = "start";
    private readonly List<string> inventory = new List<string>();

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

    public ICheckpoint Save()
    {
        return new Checkpoint(health, position, inventory);
    }

    /// <summary>
    /// Takes the state back from a checkpoint this player's class made. A checkpoint of any
    /// other class throws <see cref="InvalidCastException"/>.
    /// </summary>
    public void Load(ICheckpoint checkpoint)
    {
        Checkpoint saved = (Checkpoint)checkpoint;
        health = saved.Health;
        position = saved.Position;
        inventory.Clear();
        inventory.AddRange(saved.Inventory);
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture,
            $"health {health}, at {position}, carrying [{string.Join(", ", inventory)}]");
}
