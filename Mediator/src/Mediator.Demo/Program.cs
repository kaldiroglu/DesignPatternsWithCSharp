// Most examples have a class called Main or Test, as the Java original does. Aliases name
// them apart.
using ChatDirectMain = dev.kaldiroglu.Mediator.Chat.Problem.Direct.Main;
using ChatDirectoryMain = dev.kaldiroglu.Mediator.Chat.Problem.Directory.Main;
using ChatBusMain = dev.kaldiroglu.Mediator.Chat.Problem.Bus.Main;
using ChatMain = dev.kaldiroglu.Mediator.Chat.Solution.Main;
using GofProblemMain = dev.kaldiroglu.Mediator.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Mediator.Gof.Solution.Main;
using GofMain = dev.kaldiroglu.Mediator.Gof.Main;
using BankQueueMain = dev.kaldiroglu.Mediator.Hw.BankQueue.Main;
using AirTrafficMain = dev.kaldiroglu.Mediator.Hw.AirTraffic.Main;
using BookingFormMain = dev.kaldiroglu.Mediator.Hw.BookingForm.Main;
using TrafficTest = dev.kaldiroglu.Mediator.Traffic.Test;

namespace dev.kaldiroglu.Mediator.Demo;

/// <summary>
/// Runs the Mediator examples.
/// <para>
/// Every example is one of the Java original's <c>main</c> methods and prints the same
/// output. <c>traffic</c> runs five cars on five threads, so the order of its lines changes
/// from run to run.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof</c> — and with no argument all of them
/// run in the order the course presents them. <c>traffic</c> is last, because its threads go
/// on printing after its <c>Run()</c> returns.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["chat-direct"] = ("A TEAM CHAT AND ITS PRIVATE MESSAGES", ChatDirectMain.Run),
        ["chat-directory"] = ("A TEAM CHAT AND ITS PRIVATE MESSAGES", ChatDirectoryMain.Run),
        ["chat-bus"] = ("A TEAM CHAT AND ITS PRIVATE MESSAGES", ChatBusMain.Run),
        ["chat"] = ("A TEAM CHAT AND ITS PRIVATE MESSAGES", ChatMain.Run),
        ["gof-problem"] = ("GOF'S FONT DIALOG", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S FONT DIALOG", GofSolutionMain.Run),
        ["gof"] = ("GOF'S FONT DIALOG", GofMain.Run),
        ["hw-bankqueue"] = ("HOMEWORK", BankQueueMain.Run),
        ["hw-airtraffic"] = ("HOMEWORK", AirTrafficMain.Run),
        ["hw-bookingform"] = ("HOMEWORK", BookingFormMain.Run),
        ["traffic"] = ("A TRAFFIC POLICE OFFICER AT A JUNCTION", TrafficTest.Run)
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
