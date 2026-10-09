namespace dev.kaldiroglu.Memento.Game.Problem.Copy;

/// <summary>Stage three: the game keeps a copy of the player as its checkpoint.</summary>
public sealed class Game
{
    private Player? checkpoint;

    public void Checkpoint(Player player)
    {
        checkpoint = new Player(player);
    }

    public void Load(Player player)
    {
        player.RestoreFrom(checkpoint!);
    }
}
