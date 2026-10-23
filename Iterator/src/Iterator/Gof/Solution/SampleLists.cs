namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// The same three employees, in a list stored in an array and in a chain list.
/// </summary>
/// <remarks>
/// In this namespace <c>List&lt;T&gt;</c> is GoF's list.
/// </remarks>
internal static class SampleLists
{
    private static readonly string[] Names = ["Ayse", "Deniz", "Elif"];

    internal static List<Employee> List()
    {
        var list = new List<Employee>();
        foreach (var name in Names) list.Append(new Employee(name));
        return list;
    }

    internal static ChainList<Employee> Chain()
    {
        var chain = new ChainList<Employee>();
        foreach (var name in Names) chain.Append(new Employee(name));
        return chain;
    }
}
