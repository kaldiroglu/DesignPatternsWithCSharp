namespace dev.kaldiroglu.Visitor.Gof.Problem;

/// <summary>
/// Runs three statements through a syntax tree whose node classes carry every compiler job:
/// pretty-printing, type checking and code generation.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<INode> program =
        [
            new AssignmentNode("y", new ConstantNode(2)),
            new AssignmentNode("x", new AddNode(new VariableRefNode("y"), new ConstantNode(1))),
            new AssignmentNode("z", new AddNode(new VariableRefNode("w"), new VariableRefNode("x")))
        ];
        var assigned = new HashSet<string>();
        var errors = new List<string>();
        var code = new List<string>();
        foreach (INode statement in program)
        {
            Console.WriteLine(statement.PrettyPrint());
            statement.TypeCheck(assigned, errors);
            statement.GenerateCode(code);
        }
        Console.WriteLine("Errors: " + Show(errors));
        Console.WriteLine("Code:   " + Show(code));
        Console.WriteLine("Each of the three jobs is written in all four node classes.");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
