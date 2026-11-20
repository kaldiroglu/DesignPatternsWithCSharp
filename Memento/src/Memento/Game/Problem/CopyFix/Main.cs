namespace dev.kaldiroglu.Memento.Game.Problem.CopyFix;

/// <summary>
/// The play from the deck with the quick fix. Loading gives back [sword, shield]. After the
/// load the player picks up a key, and the save does not change.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Memento.Demo -- game-problem-copy-fix</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Player player = new Player();
        Game game = new Game();
        player.PickUp("sword");
        player.PickUp("shield");
        player.MoveTo("bridge");
        game.Checkpoint(player);

        player.Drop("sword");
        player.PickUp("potion");
        player.MoveTo("cave");
        player.TakeDamage(100);

        game.Load(player);
        Console.WriteLine("After loading:          " + player);

        player.PickUp("key");
        Console.WriteLine("After picking up a key: " + player);
        Console.WriteLine("The save still holds:   " + game.LoadScreen());
    }
}
