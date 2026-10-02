// Several namespaces reuse a class name, as the Java original does: two Compositions, two
// Checkouts, two Moneys and three Sorters. Aliases name them apart.
using dev.kaldiroglu.Strategy.Freight;
using dev.kaldiroglu.Strategy.Gof;
using dev.kaldiroglu.Strategy.Gof.Solution;
using dev.kaldiroglu.Strategy.Hw.LateFee;
using dev.kaldiroglu.Strategy.Hw.Seating;
using dev.kaldiroglu.Strategy.Hw.Validation;
using dev.kaldiroglu.Strategy.Pricing.Domain;
using dev.kaldiroglu.Strategy.Pricing.Problem;
using dev.kaldiroglu.Strategy.Pricing.Solution;
using dev.kaldiroglu.Strategy.Sorting.Pattern;
using FreightMoney = dev.kaldiroglu.Strategy.Freight.Money;
using Money = dev.kaldiroglu.Strategy.Pricing.Domain.Money;
using NaiveComposition = dev.kaldiroglu.Strategy.Gof.Problem.Composition;
using Composition = dev.kaldiroglu.Strategy.Gof.Solution.Composition;
using StageThreeCheckout = dev.kaldiroglu.Strategy.Pricing.Problem.Checkout;
using Checkout = dev.kaldiroglu.Strategy.Pricing.Solution.Checkout;
using NaiveSorter = dev.kaldiroglu.Strategy.Sorting.Problem.Sorter;

namespace dev.kaldiroglu.Strategy.Demo;

/// <summary>
/// Runs each Strategy example once and prints the figures the course quotes about it.
/// <para>
/// The Java original has no <c>main</c> methods for Strategy; its figures are asserted by
/// tests. This runner prints the same figures from the same inputs, so they can be read
/// against those tests. Each example runs on its own — <c>dotnet run -- freight</c> — and with
/// no argument all of them run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["gof"] = ("GOF'S COMPOSITORS", Compositors),
        ["pricing-problem"] = ("THE CHECKOUT", PricingProblem),
        ["pricing-solution"] = ("THE CHECKOUT", PricingSolution),
        ["sorting"] = ("THE SORTER", Sorting),
        ["freight"] = ("FREIGHT QUOTING", Freight),
        ["hw-seating"] = ("HOMEWORK", Seating),
        ["hw-latefee"] = ("HOMEWORK", LateFees),
        ["hw-validation"] = ("HOMEWORK", Validation)
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

    // ------------------------------------------------------------ GoF's compositors

    private const string Paragraph =
        "A document editor breaks a stream of text into lines "
        + "and there are many algorithms for it";

    private const int Measure = 26;

    private static Composition DocumentWith(ICompositor compositor)
    {
        var document = new Composition(Measure, compositor);
        foreach (var word in Paragraph.Split(' '))
        {
            document.Insert(Component.Word(word));
        }
        return document;
    }

    private static void Compositors()
    {
        Console.WriteLine($"GoF's motivation sentence, in a {Measure}-column measure.");

        foreach (ICompositor compositor in new ICompositor[] { new SimpleCompositor(), new TeXCompositor() })
        {
            var layout = DocumentWith(compositor).Repair();
            Console.WriteLine($"\n{compositor.Name}: {layout.LineCount} lines, worst gap {layout.WorstSlack}");
            foreach (var line in layout.Render())
            {
                Console.WriteLine($"  |{line.PadRight(Measure)}|");
            }
        }

        var rows = DocumentWith(new ArrayCompositor(6)).Repair();
        Console.WriteLine($"\nArrayCompositor(6): {rows.Lines[0].Count} to the first row, "
                          + $"{rows.WidthOf(0)} wide in a {Measure}-column measure");

        var document = DocumentWith(new SimpleCompositor());
        document.SetCompositor(new TeXCompositor());
        Console.WriteLine($"\nThe same document, compositor replaced: now {document.CompositorName}");

        var words = Paragraph.Split(' ').Select(Component.Word).ToList();
        var fast = new NaiveComposition(words, Measure, false).Repair().Render();
        var quality = new NaiveComposition(words, Measure, true).Repair().Render();
        Console.WriteLine("The naive Composition lays the paragraph out the same way: "
                          + $"fast {Same(fast, DocumentWith(new SimpleCompositor()).Repair().Render())}, "
                          + $"quality {Same(quality, DocumentWith(new TeXCompositor()).Repair().Render())}");
    }

    private static string Same(IReadOnlyList<string> a, IReadOnlyList<string> b) =>
        a.SequenceEqual(b) ? "identical" : "DIFFERENT";

    // --------------------------------------------------------------- the checkout

    private static Basket StudentBasket() =>
        Basket.Of(Customer.Student("Ceyda"), new Line("java-book", "book", Money.Of("400.00"), 3));

    private static void PricingProblem()
    {
        var basket = StudentBasket();
        Console.WriteLine("A student buys three books at 400.00.\n");

        Console.WriteLine("Stage one, a branch per campaign (the campaign is a string):");
        var switching = new SwitchingCheckout();
        foreach (var campaign in new[] { "NONE", "STUDENT", "BLACKFRIDAY", "BUY2GET1" })
        {
            Console.WriteLine("  " + switching.Ring(basket, campaign));
        }
        try
        {
            switching.Ring(basket, "BLACK_FRIDAY");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"  a typo is a run-time failure: {e.Message.Split(" (")[0]}");
        }

        Console.WriteLine("\nStage two, an enum the compiler checks:");
        var enumerated = new EnumCheckout();
        foreach (var campaign in Enum.GetValues<Campaign>())
        {
            Console.WriteLine("  " + enumerated.Ring(basket, campaign));
        }

        Console.WriteLine("\nStage three, a class per campaign:");
        foreach (StageThreeCheckout checkout in new StageThreeCheckout[]
                 { new PlainCheckout(), new StudentCheckout(), new BlackFridayCheckout() })
        {
            Console.WriteLine("  " + checkout.Ring(basket));
        }

        var till = new Till();
        Console.WriteLine("\nThe reversal: the best campaign for this basket");
        Console.WriteLine("  " + till.BestFor(basket));
        Console.WriteLine($"  and the caller had to name {till.CampaignsNamedHere} campaign classes to find it");
    }

    private static CampaignBook Today() =>
        new(new ShelfPrice(),
            PercentageOff.Student(),
            PercentageOff.Staff(),
            TieredPercentageOff.BlackFriday(),
            new CheapestOfEveryThird("book"));

    private static void PricingSolution()
    {
        var basket = StudentBasket();
        var book = Today();

        Console.WriteLine($"One till, {book.Size} campaigns, one basket:");
        foreach (var receipt in book.QuoteAll(basket))
        {
            Console.WriteLine("  " + receipt);
        }
        Console.WriteLine($"Best for this basket: {book.BestFor(basket).Name}");

        var threeBooks = Basket.Of(Customer.Student("Ceyda"),
            new Line("poetry", "book", Money.Of("300.00"), 3));
        Console.WriteLine("\nThree books at 300.00, under the Black Friday threshold:");
        Console.WriteLine($"  BLACK_FRIDAY charges    {TieredPercentageOff.BlackFriday().PriceFor(threeBooks)}");
        Console.WriteLine($"  BUY_TWO_GET_ONE charges {new CheapestOfEveryThird("book").PriceFor(threeBooks)}");
        Console.WriteLine($"  Best for this basket: {book.BestFor(threeBooks).Name}");

        var till = new Checkout(new ShelfPrice());
        Console.WriteLine($"\nA till on {till.RuleName} charges {till.Ring(basket).Paid}.");
        till.SetRule(PercentageOff.Student());
        Console.WriteLine($"The same till, Thursday morning, on {till.RuleName}, charges {till.Ring(basket).Paid}.");
    }

    // ------------------------------------------------------------------ the sorter

    private static double[] Shuffled(int size)
    {
        var random = new Random(42);          // fixed seed: the same array every run
        var list = new double[size];
        for (var i = 0; i < size; i++)
        {
            list[i] = random.NextDouble() * 1000;
        }
        return list;
    }

    private static void Sorting()
    {
        var naive = new NaiveSorter();
        var context = new SortingContext();

        Console.WriteLine("size        naive        context      sorted");
        foreach (var size in new[] { 10, 99, 100, 5_000 })
        {
            var a = Shuffled(size);
            var b = (double[])a.Clone();
            naive.Sort(a);
            context.Sort(b);
            var sorted = a.SequenceEqual(b) && a.Zip(a.Skip(1)).All(pair => pair.First <= pair.Second);
            Console.WriteLine($"{size,-11} {naive.LastUsed,-12} {context.LastUsed,-12} {sorted}");
        }

        Console.WriteLine($"\nThresholds: bubble below {SortingContext.BubbleLimit}, "
                          + $"the library from {SortingContext.QuickLimit}.");
        Console.WriteLine($"For a billion elements, without allocating them: "
                          + $"{context.SorterFor(1_000_000_000).Name}");
    }

    // -------------------------------------------------------------- freight quoting

    private static void Freight()
    {
        var pillow = new Shipment("TR", "TR", 900, 50, 40, 30);
        var books = new Shipment("TR", "TR", 4200, 25, 20, 10);

        var zones = new ByZone("UPS",
            new Dictionary<string, FreightMoney>
            {
                ["TR"] = FreightMoney.Of("60.00"),
                ["DE"] = FreightMoney.Of("240.00")
            },
            FreightMoney.Of("12.00"), 15);

        var board = new CarrierBoard(
            new ByDesi("Yurtici", FreightMoney.Of("38.00"), 1),
            new ByWeightBand("Aras",
            [
                new ByWeightBand.Band(1000, FreightMoney.Of("45.00")),
                new ByWeightBand.Band(5000, FreightMoney.Of("70.00")),
                new ByWeightBand.Band(10000, FreightMoney.Of("110.00"))
            ], FreightMoney.Of("190.00")),
            zones,
            new FlatRate("Marketplace", FreightMoney.Of("89.90")));

        Console.WriteLine($"pillow: {pillow.Grams} g on the scale, {pillow.VolumeCm3} cm3, "
                          + $"{pillow.ChargeableGrams} g chargeable");
        Console.WriteLine($"books:  {books.Grams} g on the scale, {books.VolumeCm3} cm3, "
                          + $"{books.ChargeableGrams} g chargeable\n");

        var forPillow = board.QuoteAll(pillow);
        var forBooks = board.QuoteAll(books);
        Console.WriteLine($"{"carrier",-12} {"pillow",8} {"books",8}");
        for (var i = 0; i < forPillow.Count; i++)
        {
            Console.WriteLine($"{forPillow[i].Carrier,-12} {forPillow[i].Price,8} {forBooks[i].Price,8}");
        }

        Console.WriteLine($"\nCheapest for the pillow: {board.CheapestFor(pillow)}");
        Console.WriteLine($"Cheapest for the books:  {board.CheapestFor(books)}");

        zones.SetFuelSurchargePercent(30);
        Console.WriteLine($"\nThe fuel feed moves UPS to 30%: books now {zones.Quote(books)} with UPS; "
                          + $"no other card is touched.");
    }

    // ------------------------------------------------------------------ homework

    private static void Seating()
    {
        var cabin = SeatPlan.Empty(3, 4);   // 3 rows, A to D
        var desk = new BookingDesk(new FirstAvailable());
        foreach (ISeatingPolicy policy in new ISeatingPolicy[]
                 { new FirstAvailable(), new WindowPreferred(), new KeepTogether() })
        {
            desk.SetPolicy(policy);
            Console.WriteLine($"{desk.PolicyName,-17} seats a party of two in {string.Join(", ", desk.Seat(cabin, 2))}");
        }

        var scattered = cabin.WithTaken(["1A", "1B", "1C", "2A", "2B", "2C", "3A", "3B", "3C", "3D"]);
        Console.WriteLine($"\nTwo seats free, never two in a row: {string.Join(", ", scattered.Free())}");
        Console.WriteLine($"FIRST_AVAILABLE gives {string.Join(", ", new FirstAvailable().Allocate(scattered, 2))}; "
                          + $"KEEP_TOGETHER gives {new KeepTogether().Allocate(scattered, 2).Count} seats");
    }

    private static void LateFees()
    {
        var tenDaysLate = new Loan("Design Patterns", 10, 50);
        var desk = new ReturnsDesk(new StandardFee());
        foreach (IFeeRule rule in new IFeeRule[] { new StandardFee(), new CappedFee(300), new GraceThenDouble(3) })
        {
            desk.SetRule(rule);
            Console.WriteLine($"{desk.RuleName,-17} charges {desk.Charge(tenDaysLate)} for ten days late");
        }
    }

    private static void Validation()
    {
        var relaxed = new SignUpForm(new MinimumLength(8));
        var strict = new SignUpForm(new MinimumLength(12), new MixedCharacters(),
            NoCommonWords.TheUsualSuspects());

        Console.WriteLine($"relaxed ({relaxed.RuleCount} rule) accepts hunter2024: {relaxed.Accepts("hunter2024")}");
        Console.WriteLine($"strict ({strict.RuleCount} rules) accepts hunter2024: {strict.Accepts("hunter2024")}");

        var complaints = strict.Complaints("password");
        Console.WriteLine($"\nstrict on \"password\", {complaints.Count} complaints:");
        foreach (var complaint in complaints)
        {
            Console.WriteLine("  " + complaint);
        }
        Console.WriteLine($"\nstrict accepts kedi-42-balkon!: {strict.Accepts("kedi-42-balkon!")}");
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
