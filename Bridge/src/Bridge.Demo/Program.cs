using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Factory;
using dev.kaldiroglu.Bridge.Notifications.Solution.Shared;
using dev.kaldiroglu.Bridge.Retrofit;

// Imported outright, not aliased: an extension method is only found through a real `using`, and
// Department.RetainedVersions() is one. Aliases bring in types, never extensions.
using dev.kaldiroglu.Bridge.Files.Problem;
using Basic = dev.kaldiroglu.Bridge.Basic;
using Classic = dev.kaldiroglu.Bridge.Notifications.Solution.Classic;
using FilesProblem = dev.kaldiroglu.Bridge.Files.Problem;
using FilesSolution = dev.kaldiroglu.Bridge.Files.Solution;
using Gof = dev.kaldiroglu.Bridge.Gof;
using NotificationsProblem = dev.kaldiroglu.Bridge.Notifications.Problem;
using ShapeSolution = dev.kaldiroglu.Bridge.Shape.Solution;
using Violation = dev.kaldiroglu.Bridge.Violation;

namespace dev.kaldiroglu.Bridge.Demo;

/// <summary>
/// Runs every Bridge example against the same question: what does the next change cost?
/// <para>
/// Each example runs on its own — <c>dotnet run -- violation</c> — so none of them needs a line
/// commented out to be seen alone. With no argument, all of them run in the order the deck
/// presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["violation"] = ("WHY NOT INHERITANCE", RunViolation),
        ["basic"] = ("THE PATTERN, REDUCED TO ITS BONES", RunBasic),
        ["notifications-problem"] = ("AN ORDER SYSTEM — the naive designs", RunNotificationsProblem),
        ["notifications"] = ("AN ORDER SYSTEM — the bridge", RunNotifications),
        ["notifications-factory"] = ("AN ORDER SYSTEM — who chooses the implementor", RunFactory),
        ["notifications-shared"] = ("AN ORDER SYSTEM — sharing an implementor", RunShared),
        ["shapes"] = ("SHAPES OVER TWO WINDOW SYSTEMS", RunShapes),
        ["window"] = ("GOF'S OWN EXAMPLE", RunWindow),
        ["files-problem"] = ("A DOCUMENT STORE — the naive designs", RunFilesProblem),
        ["files"] = ("A DOCUMENT STORE — the bridge", RunFiles),
        ["retrofit"] = ("A STANDARD OVER A WORKING SYSTEM", RunRetrofit)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine(
                    $"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (_, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Scenario.Heading(group);
                lastGroup = group;
            }

            run();
        }
    }

    // ------------------------------------------------------- why not inheritance

    private static void RunViolation()
    {
        Scenario.Section("a caller holding AType, expecting each DoIt() to print");
        Violation.AType[] everything = [new Violation.AType(42, true), new Violation.ASubType(42, true)];

        foreach (var each in everything)
        {
            Console.Write($"  {each.GetType().Name} -> ");
            each.DoIt();
            Console.WriteLine();
        }

        Console.WriteLine("""
                  One line of output, two objects. The second printed nothing, threw
                  nothing, and logged nothing. A caller cannot tell, and cannot defend
                  itself except by testing the type — which is the thing polymorphism
                  was supposed to remove.

                  The fix is not a better override. It is to stop using inheritance to
                  supply an implementation.
                """);
    }

    // --------------------------------------------- the pattern, reduced to its bones

    private static void RunBasic()
    {
        Scenario.Section("every abstraction over every implementation, from four classes");
        Basic.Pattern.IAnAbstractionImplementation[] implementations =
        [
            new Basic.Pattern.AConcreteImplementation1(), new Basic.Pattern.AConcreteImplementation2()
        ];

        foreach (var implementation in implementations)
        {
            new Basic.Pattern.Client(new Basic.Pattern.ASubAbstraction(implementation)).Start();
            new Basic.Pattern.Client(new Basic.Pattern.AnotherSubAbstraction(implementation)).Start();
        }

        Console.WriteLine();
        Console.WriteLine("  2 abstractions + 2 implementations = 4 classes, 4 combinations.");

        Scenario.Section("the same four combinations, one class per cell");
        Basic.Problem.IAnAbstraction[] everyCombination =
        [
            new Basic.Problem.AConcreteImplementation1(), new Basic.Problem.AConcreteImplementation2(),
            new Basic.Problem.AnotherConcreteImplementation1(),
            new Basic.Problem.AnotherConcreteImplementation2()
        ];

        foreach (var combination in everyCombination)
        {
            new Basic.Problem.Client(combination).Start();
        }

        Console.WriteLine();
        Console.WriteLine("  4 leaf classes plus the 2 refinements they extend = 6.");
        Console.WriteLine("  A third implementation costs 2 more. The pattern would cost 1.");
    }

    // ------------------------------------------------------- the order system

    private static void RunNotificationsProblem()
    {
        var log = new TransportLog();
        var transports = new Transports(log);
        var akin = Recipient.Of("Akin", ChannelKind.Email);

        Scenario.Section("1. one class, a switch on each axis");
        var notifier = new NotificationsProblem.SwitchingNotifier(transports);
        notifier.Send(NotificationsProblem.SwitchingNotifier.Kind.Simple, ChannelKind.Email,
            akin, Scenario.ShortMessage());
        notifier.Send(NotificationsProblem.SwitchingNotifier.Kind.Urgent, ChannelKind.Sms,
            akin, Scenario.ShortMessage());
        Console.WriteLine($"  works: {log.SendCount} messages on the wire");
        Console.WriteLine("  but 3 kinds x 3 channels = 9 branches, in one method");

        Console.WriteLine("\n  the 160-character rule is stated in two of the three SMS branches, so:");
        try
        {
            notifier.Send(NotificationsProblem.SwitchingNotifier.Kind.Digest, ChannelKind.Sms,
                akin, Scenario.LongMessage());
            Console.WriteLine("  (no error)");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"  {e.GetType().Name}: {e.Message}");
            Console.WriteLine("  nobody removed the rule from the digest branch — it was never added");
        }

        Scenario.Section("2. a class for every (kind, channel) pair");
        var pairLog = new TransportLog();
        var pairTransports = new Transports(pairLog);
        new NotificationsProblem.UrgentEmailNotification(pairTransports).Send(akin, Scenario.ShortMessage());
        new NotificationsProblem.UrgentSmsNotification(pairTransports).Send(akin, Scenario.ShortMessage());
        Console.WriteLine($"  works: {pairLog.SendCount} messages, 2 classes");
        Console.WriteLine("""
                  The retry loop — the only thing 'urgent' means — is written twice, in
                  two classes that cannot share it, and will be written a third time
                  when push arrives.

                  3 kinds x 3 channels = 9 classes; a fourth channel is 3 more.
                """);

        Scenario.Section("3. the notification inherits the channel");
        var bora = Recipient.Of("Bora", ChannelKind.Sms);
        var result = new NotificationsProblem.EmailBoundUrgentNotification(new Transports(new TransportLog()))
            .Send(bora, Scenario.ShortMessage());
        Console.WriteLine($"  Bora's stored preference : {bora.Preferred}");
        Console.WriteLine($"  the notification used    : {result.Channel}");
        Console.WriteLine($"  delivered                : {result.Delivered}");
        Console.WriteLine("""

                  Nothing failed. The stored preference simply did not reach the object,
                  and the result says the send succeeded. Avoiding that mismatch is now
                  every caller's job — and no compiler checks whether a call site did it.
                """);
    }

    private static void RunNotifications()
    {
        var log = new TransportLog();
        var transports = new Transports(log);
        var akin = Recipient.Of("Akin", ChannelKind.Email);

        Scenario.Section("3 kinds x 3 channels, from 3 + 3 classes");
        Classic.INotificationChannel[] channels =
        [
            new Classic.EmailChannel(transports), new Classic.SmsChannel(transports),
            new Classic.PushChannel(transports)
        ];

        foreach (var channel in channels)
        {
            var simple = new Classic.SimpleNotification(channel).Notify(akin, Scenario.ShortMessage());
            var urgent = new Classic.UrgentNotification(channel).Notify(akin, Scenario.ShortMessage());
            var digest = new Classic.DigestNotification(channel)
                .Add(Scenario.ShortMessage()).Notify(akin, Scenario.ShortMessage());
            Console.WriteLine(
                $"  {channel.Name,-6} simple={simple.Delivered} urgent={urgent.Delivered} "
                + $"digest={digest.Delivered}");
        }

        Console.WriteLine($"  {log.SendCount} messages, and every combination worked");

        Scenario.Section("the channel's rules are asked for, never assumed");
        var message = Scenario.LongMessage();
        var onEmail = new Classic.UrgentNotification(new Classic.EmailChannel(transports)).Notify(akin, message);
        var onSms = new Classic.UrgentNotification(new Classic.SmsChannel(transports)).Notify(akin, message);
        var onPush = new Classic.UrgentNotification(new Classic.PushChannel(transports)).Notify(akin, message);

        Console.WriteLine("  one UrgentNotification class, one long message:");
        Console.WriteLine($"    over email : {onEmail.BodySent.Length} characters");
        Console.WriteLine($"    over sms   : {onSms.BodySent.Length} characters  "
                          + $"(truncated: {onSms.Truncated(message.Body)})");
        Console.WriteLine($"    over push  : {onPush.BodySent.Length} characters");
        Console.WriteLine("""

                  The notification asked MaxBodyLength. It never asked which channel it
                  was holding — and there is no type test anywhere in that namespace.
                """);

        Scenario.Section("the channel can change on a notification that already exists");
        var swapLog = new TransportLog();
        var swapTransports = new Transports(swapLog);
        Classic.Notification notification =
            new Classic.UrgentNotification(new Classic.EmailChannel(swapTransports));
        Console.WriteLine($"  channel            : {notification.ChannelName}");
        notification.Notify(akin, Scenario.ShortMessage());

        notification.SetChannel(new Classic.SmsChannel(swapTransports));
        Console.WriteLine($"  after SetChannel   : {notification.ChannelName}   (the same object)");
        notification.Notify(akin, Scenario.ShortMessage());
        Console.WriteLine($"  on the wire        : {swapLog.SendCountFor("email")} email, "
                          + $"{swapLog.SendCountFor("sms")} sms");
    }

    private static void RunFactory()
    {
        Scenario.Section("each recipient is reached the way they asked to be");
        var log = new TransportLog();
        var transports = new Transports(log);
        var service = new NotificationService(new PreferenceChannelFactory(transports));

        Recipient[] people =
        [
            Recipient.Of("Akin", ChannelKind.Email),
            Recipient.Of("Bora", ChannelKind.Sms),
            Recipient.Of("Ceyda", ChannelKind.Push)
        ];

        foreach (var person in people)
        {
            var r = service.Send(c => new Classic.UrgentNotification(c), person, Scenario.ShortMessage());
            Console.WriteLine($"  {person.Name,-6} prefers {person.Preferred,-5} -> sent over {r.Channel}");
        }

        Console.WriteLine("""

                  One notification kind, one call site, three channels — chosen from a
                  value that came out of a database while the program was running.
                """);

        Scenario.Section("when a vendor goes down");
        var reroutedLog = new TransportLog();
        var reroutedTransports = new Transports(reroutedLog);
        var factory = new PreferenceChannelFactory(reroutedTransports);
        factory.Register(ChannelKind.Push, new Classic.EmailChannel(reroutedTransports));

        var result = new NotificationService(factory).Send(c => new Classic.UrgentNotification(c),
            Recipient.Of("Ceyda", ChannelKind.Push), Scenario.ShortMessage());

        Console.WriteLine($"  Ceyda prefers Push, push is re-pointed at email -> {result.Channel}");
        Console.WriteLine("  one line of configuration, and no notification kind was touched");
    }

    private static void RunShared()
    {
        Scenario.Section("three notifications, one channel object");
        var log = new TransportLog();
        var pooled = new PooledChannel(new Classic.EmailChannel(new Transports(log)));
        var akin = Recipient.Of("Akin", ChannelKind.Email);

        Classic.Notification[] notifications =
        [
            new Classic.SimpleNotification(pooled.Acquire()),
            new Classic.UrgentNotification(pooled.Acquire()),
            new Classic.DigestNotification(pooled.Acquire())
        ];

        foreach (var notification in notifications)
        {
            notification.Notify(akin, Scenario.ShortMessage());
        }

        Console.WriteLine($"  abstractions sharing the channel : {pooled.Users}");
        Console.WriteLine($"  messages through the one channel : {pooled.MessagesSent}");
        Console.WriteLine($"  transport connections opened     : {log.ConnectionsOpened}");
        Console.WriteLine("""

                  What it costs: the implementor now holds shared state. It has to be
                  thread-safe, its failures belong to everybody using it at once, and it
                  cannot hold anything specific to one abstraction.
                """);
    }

    // ------------------------------------------------------------------ shapes

    private static void RunShapes()
    {
        var mac = new ShapeSolution.MacOSDrawer();
        var x = new ShapeSolution.XWindowsDrawer();

        Scenario.Section("a circle on MacOS");
        ShapeSolution.IShape circle = new ShapeSolution.Circle("circle", mac, 50, 50, 20);
        circle.Draw();
        Console.WriteLine($"  -> {mac.Calls.Count} device call(s)");

        Scenario.Section("the same circle object, moved to XWindows at run time");
        circle.SetDrawer(x);
        circle.Draw();
        Console.WriteLine($"  -> {x.Calls.Count} device call(s), because XWindows has no arc");

        Scenario.Section("a rectangle, then a triangle, both on MacOS");
        mac.ResetCalls();
        new ShapeSolution.Rectangle("rectangle", mac, 10, 10, 40, 20).Draw();
        new ShapeSolution.Triangle("triangle", mac, 0, 0, 20, 0, 10, 15).Draw();
        Console.WriteLine($"  -> {mac.Calls.Count} device call(s): 4 lines + 3 lines");
        Console.WriteLine("\n  Triangle was added after both drawers were written.");
        Console.WriteLine("  Neither drawer changed. That is what m + n buys.");
    }

    // ---------------------------------------------------------- GoF's own example

    private static void RunWindow()
    {
        Scenario.Section("the same icon window, once per platform — the platform is a class");
        Console.WriteLine("\nnew XIconWindow(...)          new PMIconWindow(...)");
        Gof.Display.SideBySide(
            Gof.Problem.Window.Render(new Gof.Problem.XIconWindow(24, 5, "readme.txt")),
            Gof.Problem.Window.Render(new Gof.Problem.PMIconWindow(24, 5, "readme.txt")));
        Console.WriteLine("""

                  3 window kinds x 2 platforms = 6 classes; a third platform = 9.
                  The X drawing code is written three times, and the compiler will not
                  tell you which copy you missed.
                """);

        Scenario.Section("the same two pictures — the platform is an object the window holds");
        Gof.Solution.IWindowImp x = new Gof.Solution.XWindowImp();
        Gof.Solution.IWindowImp pm = new Gof.Solution.PMWindowImp();
        Console.WriteLine("\nnew IconWindow(.., x)         new IconWindow(.., pm)");
        Gof.Display.SideBySide(
            Gof.Solution.Window.Render(new Gof.Solution.IconWindow(24, 5, "readme.txt", x)),
            Gof.Solution.Window.Render(new Gof.Solution.IconWindow(24, 5, "readme.txt", pm)));

        Scenario.Section("the platform can change while the program runs");
        var window = new Gof.Solution.IconWindow(24, 5, "readme.txt", x);
        Console.WriteLine($"  window.Platform  = {window.Platform}");
        window.SetImp(pm);
        Console.WriteLine($"  window.SetImp(pm) = {window.Platform}   (the same object)");

        Scenario.Section("what each platform was actually asked to do");
        Gof.Solution.IWindowImp traced = new Gof.Solution.PMWindowImp();
        new Gof.Solution.IconWindow(24, 5, "readme.txt", traced).DrawContents(new Gof.Canvas(24, 5));
        foreach (var call in traced.Journal)
        {
            Console.WriteLine($"  PM  {call}");
        }

        Gof.Solution.IWindowImp tracedX = new Gof.Solution.XWindowImp();
        new Gof.Solution.IconWindow(24, 5, "readme.txt", tracedX).DrawContents(new Gof.Canvas(24, 5));
        Console.WriteLine();
        foreach (var call in tracedX.Journal)
        {
            Console.WriteLine($"  X   {call}");
        }

        Console.WriteLine("""

                  The window asked for two rectangles and one string, both times.
                  Presentation Manager has no rectangle call, so its implementor built
                  each one from a polyline — seven calls against three. The window never
                  found out, and could not have acted on it if it had.
                """);
    }

    // ---------------------------------------------------------- the document store

    private static void RunFilesProblem()
    {
        const string path = "claims/2026/case-4021";

        Scenario.Section("1. one class, a switch on each axis");
        var stores = new FilesProblem.VendorStores();
        var manager = new FilesProblem.SwitchingFileManager(stores);

        for (var draft = 1; draft <= 8; draft++)
        {
            foreach (var department in Enum.GetValues<FilesProblem.Department>())
            {
                foreach (var store in Enum.GetValues<FilesProblem.Store>())
                {
                    manager.Save(department, store, path, $"draft {draft}");
                }
            }
        }

        Console.WriteLine("  after eight drafts of the same document:");
        Report(stores, "Evernote", $"finance/{path}", FilesProblem.Department.Finance);
        Report(stores, "Evernote", $"insurance/{path}", FilesProblem.Department.Insurance);
        Report(stores, "FileNet", $"insurance!{path}", FilesProblem.Department.Insurance);
        Console.WriteLine("""
                  The last line is the whole problem. Insurance may keep two versions and
                  is holding eight, because the FileNet branch never applied the rule.
                  Nothing threw. The vendor is happy. Only an auditor would find it.
                """);

        Scenario.Section("3. the store becomes the base class");
        var inherited = new FilesProblem.VendorStores();
        var bound = new FilesProblem.EvernoteBoundFinanceManager(inherited);
        for (var draft = 1; draft <= 8; draft++)
        {
            bound.Save(path, $"draft {draft}");
        }

        Report(inherited, "Evernote", $"finance/{path}", FilesProblem.Department.Finance);
        Console.WriteLine("""
                  The retention rule is written once now, which is a real improvement.
                  But the store is in the base class, so it was chosen when this code was
                  compiled. There is no line you can write here that moves this manager
                  to SharePoint.
                """);
    }

    private static void Report(FilesProblem.VendorStores stores, string vendor, string address,
        FilesProblem.Department department)
    {
        var held = stores.VersionsHeld(vendor, address);
        var allowed = department.RetainedVersions();
        var flag = held > allowed ? "   <-- over the limit" : "";
        Console.WriteLine($"    {vendor,-10} {address,-34} kept {held}, allowed {allowed}{flag}");
    }

    private static void RunFiles()
    {
        FilesSolution.IFileProvider[] stores =
        [
            new FilesSolution.EvernoteProvider(), new FilesSolution.SharePointProvider(),
            new FilesSolution.FileNetProvider()
        ];

        foreach (var store in stores)
        {
            FilesSolution.FileManager finance = new FilesSolution.FinanceFileManager(store);
            FilesSolution.FileManager insurance = new FilesSolution.InsuranceFileManager(store);

            for (var i = 1; i <= 8; i++)
            {
                finance.Save("q3-report", $"finance draft {i}");
                insurance.Save("policy-4417", $"insurance draft {i}");
            }

            Console.WriteLine($"{store.Name,-12} finance keeps {finance.RetainedVersions}, "
                              + $"live versions {Live(finance.Versions("q3-report"), finance.RetainedVersions)}");
            Console.WriteLine($"{store.Name,-12} insurance keeps {insurance.RetainedVersions}, "
                              + $"live versions {Live(insurance.Versions("policy-4417"), insurance.RetainedVersions)}\n");
        }

        // The move that makes it a Bridge rather than a choice made once.
        FilesSolution.FileManager moved =
            new FilesSolution.FinanceFileManager(new FilesSolution.EvernoteProvider());
        moved.Save("memo", "written on Evernote");
        moved.SetProvider(new FilesSolution.FileNetProvider());
        moved.Save("memo", "written on FileNet");
        Console.WriteLine("The same manager object, moved between stores at run time:");
        Console.WriteLine($"  {moved.Read("memo")}");

        Console.WriteLine();
        Console.WriteLine("2 departments + 3 stores = 5 classes, 6 combinations.");
        Console.WriteLine("A fourth store is one class, and no retention rule is touched.");
    }

    private static string Live(IReadOnlyList<int> versions, int keep) =>
        "[" + string.Join(", ", versions.Skip(Math.Max(0, versions.Count - keep))) + "]";

    // -------------------------------------------- a standard over a working system

    private static void RunRetrofit()
    {
        Scenario.Section("the engine's own callers, before and after the regulation");
        var engine = new LegacyEngine();
        Console.WriteLine($"  reportDirectly  : {engine.ReportDirectly("select headcount")}");

        Scenario.Section("the required interface, over an engine never designed for it");
        RegulatoryReport quarterly = new QuarterlyReport(engine);
        Console.WriteLine($"  engine          : {quarterly.EngineName}");
        Console.WriteLine($"  submit(2026-Q1) : {quarterly.Submit("2026-Q1")[0]}");
        Console.WriteLine($"  statements seen : {string.Join(", ", engine.StatementsSeen)}");

        Scenario.Section("next year's engine costs one class, and no report is touched");
        RegulatoryReport audited = new AuditedReport(new PurchasedEngine());
        Console.WriteLine($"  engine          : {audited.EngineName}");
        Console.WriteLine($"  submit(2026-Q2) : {audited.Submit("2026-Q2")[0]}");

        audited.SetEngine(new LegacyEngine());
        Console.WriteLine($"  after SetEngine : {audited.EngineName}   (the same report object)");
        Console.WriteLine("""

                  Submit is composed from open, pull and release — the vocabulary the
                  engine already had. The word the regulation uses appears nowhere on
                  the engine's interface, which is what makes this a bridge rather than
                  the same interface written twice.
                """);
    }
}
