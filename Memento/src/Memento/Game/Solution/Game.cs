namespace dev.kaldiroglu.Memento.Game.Solution;

/// <summary>
/// The <b>Caretaker</b>: the game decides when to save and which checkpoint to load, and keeps
/// the checkpoints. It never looks inside one.
/// </summary>
public sealed class Game
{
    private readonly Stack<Player.ICheckpoint> checkpoints = new Stack<Player.ICheckpoint>();

    public void Checkpoint(Player player)
    {
        checkpoints.Push(player.Save());
    }

    public void LoadLatest(Player player)
    {
        player.Load(checkpoints.Peek());
    }

    public int CheckpointCount => checkpoints.Count;
}
