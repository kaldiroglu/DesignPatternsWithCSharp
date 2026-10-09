// Several examples have a class called Main or Test, as the Java original does. Aliases name
// them apart.
using GameSettersMain = dev.kaldiroglu.Memento.Game.Problem.Setters.Main;
using GameHistoryMain = dev.kaldiroglu.Memento.Game.Problem.History.Main;
using GameCopyMain = dev.kaldiroglu.Memento.Game.Problem.Copy.Main;
using GameMain = dev.kaldiroglu.Memento.Game.Solution.Main;
using GofProblemMain = dev.kaldiroglu.Memento.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Memento.Gof.Solution.Main;
using GofMain = dev.kaldiroglu.Memento.Gof.Main;
using EditorMain = dev.kaldiroglu.Memento.Hw.Editor.Main;
using RollbackMain = dev.kaldiroglu.Memento.Hw.Rollback.Main;
using IncrementalMain = dev.kaldiroglu.Memento.Hw.Incremental.Main;
using GuiTest = dev.kaldiroglu.Memento.Gui.Test;
using Pattern1Test = dev.kaldiroglu.Memento.Pattern1.Test;
using Pattern2Test = dev.kaldiroglu.Memento.Pattern2.Test;

namespace dev.kaldiroglu.Memento.Demo;

/// <summary>
/// Runs the Memento examples.
/// <para>
/// Every example is one of the Java original's <c>main</c> methods and prints the same
/// output. <c>pattern1</c> and <c>pattern2</c> run two threads each for about twenty
/// seconds, so their lines depend on the timing of the threads.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["game-setters"] = ("A GAME CHECKPOINT", GameSettersMain.Run),
        ["game-history"] = ("A GAME CHECKPOINT", GameHistoryMain.Run),
        ["game-copy"] = ("A GAME CHECKPOINT", GameCopyMain.Run),
        ["game"] = ("A GAME CHECKPOINT", GameMain.Run),
        ["gof-problem"] = ("GOF'S CONSTRAINT SOLVER", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S CONSTRAINT SOLVER", GofSolutionMain.Run),
        ["gof"] = ("GOF'S CONSTRAINT SOLVER", GofMain.Run),
        ["hw-editor"] = ("HOMEWORK", EditorMain.Run),
        ["hw-rollback"] = ("HOMEWORK", RollbackMain.Run),
        ["hw-incremental"] = ("HOMEWORK", IncrementalMain.Run),
        ["gui"] = ("THE EARLIER EXAMPLES", GuiTest.Run),
        ["pattern1"] = ("THE EARLIER EXAMPLES", Pattern1Test.Run),
        ["pattern2"] = ("THE EARLIER EXAMPLES", Pattern2Test.Run)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine($"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (name, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Heading(group);
                lastGroup = group;
            }

            Section(name);
            run();
        }
    }

    // ---------------------------------------------------------------- output

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
