// Lender.Problem1, Lender.Problem2 and Lender.Pattern each have a class named Main, as the
// Java original does. Aliases name them apart; a plain `using` would make `Main` ambiguous.
using LenderProblem1 = dev.kaldiroglu.Command.Lender.Problem1.Main;
using LenderProblem2 = dev.kaldiroglu.Command.Lender.Problem2.Main;
using LenderPattern = dev.kaldiroglu.Command.Lender.Pattern.Main;
using Person = dev.kaldiroglu.Command.Ac.Person;

namespace dev.kaldiroglu.Command.Demo;

/// <summary>
/// Runs the examples that have a <c>main</c> method in the Java original: the lender's two
/// problems and its pattern form, and the air conditioner.
/// <para>
/// Each example runs on its own — <c>dotnet run -- lender-pattern</c> — so none of them
/// needs a line commented out to be seen alone. With no argument, all of them run in the
/// order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["lender-problem1"] = ("THE LENDER", LenderProblem1.Run),
        ["lender-problem2"] = ("THE LENDER", LenderProblem2.Run),
        ["lender-pattern"] = ("THE LENDER", LenderPattern.Run),
        ["ac"] = ("THE AIR CONDITIONER", Person.Run)
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
