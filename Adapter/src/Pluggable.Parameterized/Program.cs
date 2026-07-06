namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;
public static class Program {
    public static void Main(string[] args) {
        if (args.Length > 0 && args[0] == "demo") PluggableDemo.Run();
        else ApplianceDemo.Run();
    }
}
