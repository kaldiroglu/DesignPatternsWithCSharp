using dev.kaldiroglu.Proxy.Gof;
using dev.kaldiroglu.Proxy.Network;
using dev.kaldiroglu.Proxy.Pm.Pm1;
using dev.kaldiroglu.Proxy.Pm.Pm2;
using dev.kaldiroglu.Proxy.Pm.Pm3;
using dev.kaldiroglu.Proxy.Pm.Pm4;

// Runs the three Proxy examples one after another. Each is self-contained; see
// the README for what each one demonstrates. One entry runs on its own -
// `dotnet run -- pm3-screening` - and prints one slide's output.

var examples = new (string Name, Action? Heading, Action Run)[]
{
    ("gof", () => Section("1. GoF virtual proxy — lazy image loading (Design Patterns, p. 207)"), GofDemo.Run),
    ("network", () => Section("2. Network protection proxy — access control + logging"), NetworkDemo.Run),
    ("pm1", () =>
    {
        Section("3. PM — evolving toward the Proxy pattern (pm1 -> pm2 -> pm3 -> pm4)");
        Console.WriteLine("--- pm1: no proxy, citizen talks to the PM directly ---");
    }, Pm1Demo.Run),
    ("pm2", () => Console.WriteLine("\n--- pm2: a hand-rolled proxy, not yet sharing an interface ---"), Pm2Demo.Run),
    ("pm3-secretary", () => Console.WriteLine("\n--- pm3: the GoF proxy, RealPM and ProxyPM share IPM ---"), Pm3Demo.Secretary),
    ("pm3-screening", null, Pm3Demo.Screening),
    ("pm4", () => Console.WriteLine("\n--- pm4: ProxyPM creates RealPM on the first forwarded call ---"), Pm4Demo.Run),
};

if (args.Length > 0)
{
    var name = args[0].ToLowerInvariant();
    var example = examples.FirstOrDefault(e => e.Name == name);
    if (example.Run is null)
    {
        Console.WriteLine($"unknown example '{name}'. One of: "
                          + string.Join(", ", examples.Select(e => e.Name)));
        return;
    }

    example.Run();
    return;
}

foreach (var (_, heading, run) in examples)
{
    heading?.Invoke();
    run();
}

static void Section(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 72));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 72));
}
