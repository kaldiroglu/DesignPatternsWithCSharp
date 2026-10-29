// Several example namespaces have a runnable class with the same name (Main).
// Aliases name them apart.
using UsersMain = dev.kaldiroglu.Introduction.Problems.Users.Main;
using PricingStageOne = dev.kaldiroglu.Introduction.Problems.Pricing.StageOneMain;
using Pm1Main = dev.kaldiroglu.Introduction.Proxy.Pm.Pm1.Main;
using Pm2Main = dev.kaldiroglu.Introduction.Proxy.Pm.Pm2.Main;
using Pm3Main = dev.kaldiroglu.Introduction.Proxy.Pm.Pm3.Main;

namespace dev.kaldiroglu.Introduction.Demo;

/// <summary>
/// Runs the Introduction examples.
/// <para>
/// Each example runs on its own — <c>dotnet run -- pm1</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["user"] = ("ONE METHOD DECIDES EVERYTHING ABOUT A USER", UsersMain.Run),
        ["pricing"] = ("ONE BRANCH FOR EACH WAY OF DOING THE JOB", PricingStageOne.Run),
        ["pm1"] = ("CALLS TO THE PRIME MINISTER", Pm1Main.Run),
        ["pm2"] = ("CALLS TO THE PRIME MINISTER", Pm2Main.Run),
        ["pm3"] = ("CALLS TO THE PRIME MINISTER", Pm3Main.Run)
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

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
