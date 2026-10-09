using dev.kaldiroglu.Observer.Price.Solution;
// Several examples have a class called Main or Test, as the Java original does, and three
// namespaces share a name with their main class (Publisher, Auction, Inbox). Aliases name
// them apart.
using PriceMain = dev.kaldiroglu.Observer.Price.Solution.Main;
using PriceProblemMain = dev.kaldiroglu.Observer.Price.Problem.Main;
using GofMain = dev.kaldiroglu.Observer.Gof.Main;
using GofProblemMain = dev.kaldiroglu.Observer.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Observer.Gof.Solution.Main;
using PaymentTest = dev.kaldiroglu.Observer.Payment.Test;
using PublisherTest = dev.kaldiroglu.Observer.Publisher.Test;
using AccountLogMain = dev.kaldiroglu.Observer.Hw.AccountLog.Main;
using AuctionMain = dev.kaldiroglu.Observer.Hw.Auction.Main;
using InboxMain = dev.kaldiroglu.Observer.Hw.Inbox.Main;

namespace dev.kaldiroglu.Observer.Demo;

/// <summary>
/// Runs the Observer examples.
/// <para>
/// Every entry except <c>price-event</c> is one of the Java original's <c>main</c> methods
/// and prints the same output; <c>publisher</c> prints the current date, so only the date
/// differs between two runs. <c>price-event</c> runs <see cref="PriceFeedWithEvent"/>, which
/// has no Java counterpart.
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
        ["price-problem"] = ("A STOCK PRICE", PriceProblemMain.Run),
        ["price"] = ("A STOCK PRICE", PriceMain.Run),
        ["price-event"] = ("A STOCK PRICE", PriceWithEvent),
        ["gof-problem"] = ("GOF'S CLOCK TIMER", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S CLOCK TIMER", GofSolutionMain.Run),
        ["gof"] = ("GOF'S CLOCK TIMER", GofMain.Run),
        ["publisher"] = ("MAGAZINES AND SUBSCRIBERS", PublisherTest.Run),
        ["payment"] = ("AN INVOICE WITH AN OBSERVABLE BASE CLASS", PaymentTest.Run),
        ["hw-accountlog"] = ("HOMEWORK", AccountLogMain.Run),
        ["hw-auction"] = ("HOMEWORK", AuctionMain.Run),
        ["hw-inbox"] = ("HOMEWORK", InboxMain.Run)
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

    // ------------------------------------------------------- C# only

    /// <summary>
    /// The same four prices as the <c>price</c> example, sent with a C# <c>event</c> instead
    /// of a list of listeners. The chart, the ticker and the alert are the same classes; each
    /// one is attached with <c>+=</c> through its <c>PriceChanged</c> method. The result is
    /// the same as in the second line of <c>price</c>.
    /// </summary>
    private static void PriceWithEvent()
    {
        var feed = new PriceFeedWithEvent("ACME", 100);
        var chart = new Chart();
        var ticker = new Ticker();
        var alert = new PriceAlert(105);
        feed.PriceChanged += (_, change) => chart.PriceChanged(change);
        feed.PriceChanged += (_, change) => ticker.PriceChanged(change);
        feed.PriceChanged += (_, change) => alert.PriceChanged(change);
        var log = new List<string>();
        feed.PriceChanged += (_, change) => log.Add(change.OldPrice + "->" + change.NewPrice);

        feed.SetPrice(102);
        feed.SetPrice(106);
        feed.SetPrice(100);
        feed.SetPrice(103);
        Console.WriteLine("Event:     chart " + Show(chart.Points) + ", alert fired: "
                + (alert.Fired ? "true" : "false") + ", ticker " + ticker.Shown + ", lambda " + Show(log));
    }

    // ---------------------------------------------------------------- output

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show<T>(IEnumerable<T> items) => "[" + string.Join(", ", items) + "]";

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
