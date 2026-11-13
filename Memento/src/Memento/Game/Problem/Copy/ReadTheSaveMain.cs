namespace dev.kaldiroglu.Memento.Game.Problem.Copy;

/// <summary>
/// The game saves at the bridge and shows its load screen. The save is a whole Player, so
/// the game can read every field of it.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Memento.Demo -- game-problem-copy-read-the-save</c>.
/// </remarks>
public static class ReadTheSaveMain
{
    public static void Run()
    {
        Player player = new Player();
        Game game = new Game();
        player.PickUp("sword");
        player.PickUp("shield");
        player.MoveTo("bridge");
        game.Checkpoint(player);
        Console.WriteLine(game.LoadScreen());
    }
}
