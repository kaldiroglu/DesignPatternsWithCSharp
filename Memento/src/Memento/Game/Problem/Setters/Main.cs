namespace dev.kaldiroglu.Memento.Game.Problem.Setters;

/// <summary>
/// The play from the deck: save at the bridge, lose the sword in the cave, load. Loading is
/// correct, but the public setters let any code give the player 999 health.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- game-setters</c>.
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
        Console.WriteLine("At the checkpoint: " + player);

        player.Drop("sword");
        player.PickUp("potion");
        player.MoveTo("cave");
        player.TakeDamage(100);
        Console.WriteLine("In the cave:       " + player);

        game.Load(player);
        Console.WriteLine("After loading:     " + player);

        player.Health = 999;
        Console.WriteLine("Any code may call setHealth(999): " + player);
    }
}
