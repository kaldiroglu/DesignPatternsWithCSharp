using dev.kaldiroglu.Mediator.Hw.AirTraffic;
using dev.kaldiroglu.Mediator.Hw.BankQueue;
// The namespace Hw.BookingForm holds the class BookingForm, as the Java package bookingform
// does. The alias names the class, so the runner never has to choose between the two.
using BookingFormClass = dev.kaldiroglu.Mediator.Hw.BookingForm.BookingForm;
// Most examples have a class called Main or Test, as the Java original does. Aliases name
// them apart.
using ChatMain = dev.kaldiroglu.Mediator.Chat.Solution.Main;
using GofMain = dev.kaldiroglu.Mediator.Gof.Main;
using TrafficTest = dev.kaldiroglu.Mediator.Traffic.Test;

namespace dev.kaldiroglu.Mediator.Demo;

/// <summary>
/// Runs the Mediator examples.
/// <para>
/// <c>chat</c>, <c>gof</c> and <c>traffic</c> are the Java original's <c>main</c> methods and
/// print the same output. <c>traffic</c> runs five cars on five threads, so the order of its
/// lines changes from run to run. The three homework exercises have no <c>main</c> in Java
/// and are only in this runner.
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
        ["chat"] = ("A TEAM CHAT AND ITS PRIVATE MESSAGES", ChatMain.Run),
        ["gof"] = ("GOF'S FONT DIALOG", GofMain.Run),
        ["hw-bankqueue"] = ("HOMEWORK", BankQueueHomework),
        ["hw-airtraffic"] = ("HOMEWORK", AirTrafficHomework),
        ["hw-bookingform"] = ("HOMEWORK", BookingFormHomework),
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

    // ------------------------------------------------------------ homework

    /// <summary>
    /// Teller 1 is free before anyone comes, so Ayse is called at once. Mert and Deniz wait
    /// until Teller 2 and then Teller 1 are free again.
    /// </summary>
    private static void BankQueueHomework()
    {
        QueueManager queue = new QueueManager();
        Teller teller1 = new Teller("Teller 1", queue), teller2 = new Teller("Teller 2", queue);
        Customer ayse = new Customer("Ayse", queue), mert = new Customer("Mert", queue),
                deniz = new Customer("Deniz", queue);
        teller1.Free();
        ayse.Arrive();
        mert.Arrive();
        deniz.Arrive();
        teller2.Free();
        teller1.Free();
        foreach (string line in queue.Log)
        {
            Console.WriteLine(line);
        }
    }

    /// <summary>
    /// Three aircraft ask for the runway. The tower gives it to TK1 and keeps the other two
    /// waiting; each one gets the runway when the one before it clears it.
    /// </summary>
    private static void AirTrafficHomework()
    {
        ControlTower tower = new ControlTower();
        Aircraft tk1 = new Aircraft("TK1", "land", tower), pc2 = new Aircraft("PC2", "land", tower),
                aj3 = new Aircraft("AJ3", "take off", tower);
        tk1.Request();
        pc2.Request();
        aj3.Request();
        tk1.Clear();
        pc2.Clear();
        foreach (string line in tower.Log)
        {
            Console.WriteLine(line);
        }
    }

    /// <summary>
    /// Six people do not fit in the small room, so Book is disabled and the form warns. In the
    /// large room they fit, the warning goes, and the booking is made.
    /// </summary>
    private static void BookingFormHomework()
    {
        BookingFormClass form = new BookingFormClass();
        form.ChooseRoom("Small");
        form.ChooseDate("2026-10-12");
        form.SetAttendees(6);
        Console.WriteLine("book enabled " + Show(form.BookEnabled) + ", warning: " + form.Warning);
        form.ChooseRoom("Large");
        Console.WriteLine("book enabled " + Show(form.BookEnabled) + ", warning: '" + form.Warning + "'");
        form.ClickBook();
        Console.WriteLine(Show(form.Booked));
    }

    /// <summary>Prints a boolean the way Java does: <c>true</c> or <c>false</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";

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
