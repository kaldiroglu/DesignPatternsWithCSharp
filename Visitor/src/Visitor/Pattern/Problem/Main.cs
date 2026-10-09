namespace dev.kaldiroglu.Visitor.Pattern.Problem;

/// <summary>
/// Shows the shape of the earlier syntax tree example. Its methods are empty, so running
/// them prints nothing; the full version is in <c>Visitor.Gof.Problem</c>.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- pattern-problem</c>. The lines name the
/// methods as C# names them.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<INode> nodes = [new Assignment(), new VariableReference()];
        foreach (INode node in nodes)
        {
            node.TypeCheck();
            node.GenerateCode();
            node.PrettyPrint();
            Console.WriteLine(node.GetType().Name
                    + " has " + nameof(INode.TypeCheck) + ", " + nameof(INode.GenerateCode)
                    + " and " + nameof(INode.PrettyPrint));
        }
        Console.WriteLine("The methods of this version are empty: it shows only where the operations live.");
    }
}
