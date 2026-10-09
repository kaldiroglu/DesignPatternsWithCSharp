namespace dev.kaldiroglu.Memento.Game.Solution;

/// <summary>
/// The same play through stage one, stage three and the memento.
/// <para>
/// The player picks up a sword and a shield, reaches the bridge and saves. Then it drops the
/// sword, picks up a potion, walks into the cave and takes 100 damage. The game loads the
/// checkpoint. The promise: loading gives back exactly what the player had at the
/// checkpoint — health 100, at the bridge, carrying the sword and the shield.
/// </para>
/// </summary>
public static class Main
{
    public static void Run()
    {
        // From inside Game.Solution, "Problem" is found as a sibling namespace.
        var p1 = new Problem.Setters.Player();
        var g1 = new Problem.Setters.Game();
        p1.PickUp("sword");
        p1.PickUp("shield");
        p1.MoveTo("bridge");
        g1.Checkpoint(p1);
        p1.Drop("sword");
        p1.PickUp("potion");
        p1.MoveTo("cave");
        p1.TakeDamage(100);
        g1.Load(p1);
        Console.WriteLine("Stage one, after loading:   " + p1);
        p1.Health = 999;
        Console.WriteLine("  and anyone may now write: " + p1);

        var p3 = new Problem.Copy.Player();
        var g3 = new Problem.Copy.Game();
        p3.PickUp("sword");
        p3.PickUp("shield");
        p3.MoveTo("bridge");
        g3.Checkpoint(p3);
        p3.Drop("sword");
        p3.PickUp("potion");
        p3.MoveTo("cave");
        p3.TakeDamage(100);
        g3.Load(p3);
        Console.WriteLine("Stage three, after loading: " + p3);

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
        game.LoadLatest(player);
        Console.WriteLine("Memento, after loading:     " + player);
    }
}
