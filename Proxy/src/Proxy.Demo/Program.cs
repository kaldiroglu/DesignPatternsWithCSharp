using dev.kaldiroglu.Proxy.Gof;
using dev.kaldiroglu.Proxy.Network;
using dev.kaldiroglu.Proxy.Pm.Pm1;
using dev.kaldiroglu.Proxy.Pm.Pm2;
using dev.kaldiroglu.Proxy.Pm.Pm3;

// Runs the three Proxy examples one after another. Each is self-contained; see
// the README for what each one demonstrates.

Section("1. GoF virtual proxy — lazy image loading (Design Patterns, p. 207)");
GofDemo.Run();

Section("2. Network protection proxy — access control + logging");
NetworkDemo.Run();

Section("3. PM — evolving toward the Proxy pattern (pm1 -> pm2 -> pm3)");
Console.WriteLine("--- pm1: no proxy, citizen talks to the PM directly ---");
Pm1Demo.Run();
Console.WriteLine("\n--- pm2: a hand-rolled proxy, not yet sharing an interface ---");
Pm2Demo.Run();
Console.WriteLine("\n--- pm3: the GoF proxy, RealPM and ProxyPM share IPM ---");
Pm3Demo.Run();

static void Section(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 72));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 72));
}
