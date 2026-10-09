namespace dev.kaldiroglu.Memento.Game.Problem.History;

/// <summary>
/// The play from the deck, with the player keeping its own checkpoints. Loading is correct,
/// but the save slots now live in the player class.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- game-history</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Player player = new Player();
        player.PickUp("sword");
        player.PickUp("shield");
        player.MoveTo("bridge");
        player.SaveCheckpoint();
        Console.WriteLine("At the checkpoint: " + player);

        player.Drop("sword");
        player.PickUp("potion");
        player.MoveTo("cave");
        player.TakeDamage(100);
        Console.WriteLine("In the cave:       " + player);

        player.LoadCheckpoint();
        Console.WriteLine("After loading:     " + player);
        Console.WriteLine("The save slots are a field of Player, not of the game.");
    }
}
