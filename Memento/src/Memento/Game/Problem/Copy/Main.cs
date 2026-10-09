namespace dev.kaldiroglu.Memento.Game.Problem.Copy;

/// <summary>
/// The play from the deck, with the game keeping a copy of the player. The copy shares the
/// inventory list, so after loading the player carries [shield, potion], not [sword, shield].
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- game-copy</c>.
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
        Console.WriteLine("Health and place are back, but the inventory is the one from the cave.");
    }
}
