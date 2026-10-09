using dev.kaldiroglu.Memento.Hw.Editor;
using dev.kaldiroglu.Memento.Hw.Incremental;
using dev.kaldiroglu.Memento.Hw.Rollback;
// Several examples have a class called Main or Test, as the Java original does. Aliases name
// them apart.
using GameMain = dev.kaldiroglu.Memento.Game.Solution.Main;
using GofMain = dev.kaldiroglu.Memento.Gof.Main;
using GuiTest = dev.kaldiroglu.Memento.Gui.Test;
using Pattern1Test = dev.kaldiroglu.Memento.Pattern1.Test;
using Pattern2Test = dev.kaldiroglu.Memento.Pattern2.Test;

namespace dev.kaldiroglu.Memento.Demo;

/// <summary>
/// Runs the Memento examples.
/// <para>
/// <c>game</c>, <c>gof</c>, <c>gui</c>, <c>pattern1</c> and <c>pattern2</c> are the Java
/// original's <c>main</c> methods and print the same output. <c>pattern1</c> and
/// <c>pattern2</c> run two threads each for about twenty seconds, so their lines depend on the
/// timing of the threads. The three homework exercises have no <c>main</c> in Java and are
/// only in this runner.
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
        ["game"] = ("A GAME CHECKPOINT", GameMain.Run),
        ["gof"] = ("GOF'S CONSTRAINT SOLVER", GofMain.Run),
        ["hw-editor"] = ("HOMEWORK", EditorHomework),
        ["hw-rollback"] = ("HOMEWORK", RollbackHomework),
        ["hw-incremental"] = ("HOMEWORK", IncrementalHomework),
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

    // ------------------------------------------------------------ homework

    /// <summary>
    /// Type "Hello", then " world". Undo twice goes back to the empty text; redo brings
    /// "Hello" back. The bar shows the cursor.
    /// </summary>
    private static void EditorHomework()
    {
        TextEditor editor = new TextEditor();
        History history = new History(editor);
        history.Type("Hello");
        history.Type(" world");
        Console.WriteLine(editor);
        history.Undo();
        Console.WriteLine(editor);
        history.Undo();
        Console.WriteLine(editor);
        history.Redo();
        Console.WriteLine(editor);
    }

    /// <summary>
    /// Ayse cannot pay 70, so the first batch is rolled back and Ali's 80 goes back to him.
    /// The second batch has every transfer covered, and is done.
    /// </summary>
    private static void RollbackHomework()
    {
        Account ali = new Account("Ali", 100), ayse = new Account("Ayse", 50), can = new Account("Can", 0);
        Batch failing = new Batch();
        failing.Add(ali, can, 80);
        failing.Add(ayse, can, 70);
        Console.WriteLine(failing.Run([ali, ayse, can]) + " -> " + ali + ", " + ayse + ", " + can);
        Batch ok = new Batch();
        ok.Add(ali, can, 80);
        ok.Add(ayse, can, 50);
        Console.WriteLine(ok.Run([ali, ayse, can]) + " -> " + ali + ", " + ayse + ", " + can);
    }

    /// <summary>
    /// A sheet of 100 by 100 cells. One edit sets two cells, so its memento holds two old
    /// values, not ten thousand. Undo puts both back.
    /// </summary>
    private static void IncrementalHomework()
    {
        Sheet sheet = new Sheet(100, 100);
        Sheet.IChange change = sheet.Set(new Dictionary<string, int> { ["R1C1"] = 5, ["R1C2"] = 7 });
        Console.WriteLine(Invariant($"cells {sheet.CellCount}, memento holds {change.Size}, R1C1={sheet.Get("R1C1")}"));
        sheet.Undo(change);
        Console.WriteLine(Invariant($"after undo R1C1={sheet.Get("R1C1")}, R1C2={sheet.Get("R1C2")}"));
    }

    private static string Invariant(FormattableString text) => FormattableString.Invariant(text);

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
