using dev.kaldiroglu.Observer.Hw.AccountLog;
using dev.kaldiroglu.Observer.Hw.Auction;
using dev.kaldiroglu.Observer.Price.Solution;
// Several examples have a class called Main or Test, as the Java original does, and three
// namespaces share a name with their main class (Publisher, Auction, Inbox). Aliases name
// them apart.
using PriceMain = dev.kaldiroglu.Observer.Price.Solution.Main;
using GofMain = dev.kaldiroglu.Observer.Gof.Main;
using PaymentTest = dev.kaldiroglu.Observer.Payment.Test;
using PublisherTest = dev.kaldiroglu.Observer.Publisher.Test;
using Auction = dev.kaldiroglu.Observer.Hw.Auction.Auction;
using Inbox = dev.kaldiroglu.Observer.Hw.Inbox.Inbox;

namespace dev.kaldiroglu.Observer.Demo;

/// <summary>
/// Runs the Observer examples.
/// <para>
/// <c>price</c>, <c>gof</c>, <c>publisher</c> and <c>payment</c> are the Java original's
/// <c>main</c> methods and print the same output; <c>publisher</c> prints the current date,
/// so only the date differs between two runs. <c>price-event</c> runs
/// <see cref="PriceFeedWithEvent"/>, which has no Java counterpart. The three homework
/// exercises have no <c>main</c> in Java and are only in this runner.
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
        ["price"] = ("A STOCK PRICE", PriceMain.Run),
        ["price-event"] = ("A STOCK PRICE", PriceWithEvent),
        ["gof"] = ("GOF'S CLOCK TIMER", GofMain.Run),
        ["publisher"] = ("MAGAZINES AND SUBSCRIBERS", PublisherTest.Run),
        ["payment"] = ("AN INVOICE WITH AN OBSERVABLE BASE CLASS", PaymentTest.Run),
        ["hw-accountlog"] = ("HOMEWORK", AccountLogHomework),
        ["hw-auction"] = ("HOMEWORK", AuctionHomework),
        ["hw-inbox"] = ("HOMEWORK", InboxHomework)
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

    // ------------------------------------------------------------ homework

    /// <summary>
    /// Deniz has 1000. A deposit of 500, a withdrawal of 5000 that is refused, and a
    /// withdrawal of 200. The refused one is never recorded.
    /// </summary>
    private static void AccountLogHomework()
    {
        var account = new Account("Deniz", 1000);
        var log = new TransactionLog();
        account.AddListener(log);
        account.Deposit(500);
        try
        {
            account.Withdraw(5000);
        }
        catch (ArgumentException refused)
        {
            Console.WriteLine("Refused: " + refused.Message);
        }
        account.Withdraw(200);
        Console.WriteLine("Transactions: " + log.Transactions.Count);
        foreach (var transaction in log.Transactions)
        {
            Console.WriteLine("  " + transaction);
        }
    }

    /// <summary>
    /// Ali (budget 150) and Can (budget 300) watch. Ali bids 100, Can bids 200 and 250. Can's
    /// 200 is over Ali's budget, so Ali stops watching while he is being told about it.
    /// </summary>
    private static void AuctionHomework()
    {
        var auction = new Auction();
        var ali = new Bidder("Ali", 150);
        var can = new Bidder("Can", 300);
        auction.Watch(ali);
        auction.Watch(can);
        auction.Bid("Ali", 100);
        auction.Bid("Can", 200);
        auction.Bid("Can", 250);
        Console.WriteLine("Ali heard: " + Show(ali.Heard));
        Console.WriteLine("Can heard: " + Show(can.Heard));
        Console.WriteLine("Watchers:  " + auction.WatcherCount);
        Console.WriteLine("Highest:   " + auction.Leader + " " + auction.Highest);
    }

    /// <summary>
    /// Two views of one inbox: a badge that pulls the unread count, and a list that uses the
    /// pushed message. Two messages arrive, then everything is read.
    /// </summary>
    private static void InboxHomework()
    {
        var inbox = new Inbox();
        var screen = new List<string>();
        inbox.OnNewMessage(message => screen.Add("badge " + inbox.Unread));
        inbox.OnNewMessage(message => screen.Add("list: " + message.Subject));
        inbox.Receive(new Inbox.Message("Ayse", "Lunch?"));
        inbox.Receive(new Inbox.Message("Mert", "Report"));
        Console.WriteLine("Views: " + Show(screen));
        inbox.ReadAll();
        Console.WriteLine("Unread after reading all: " + inbox.Unread);
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
