namespace dev.kaldiroglu.Visitor.Gof;

/// <summary>
/// Runs the same three statements through both designs. They print the same lines.
/// <code>
/// y = 2
/// x = y + 1
/// z = w + x
/// </code>
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- gof</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Console.WriteLine("Before the pattern");
        List<Problem.INode> before =
        [
            new Problem.AssignmentNode("y", new Problem.ConstantNode(2)),
            new Problem.AssignmentNode("x", new Problem.AddNode(
                    new Problem.VariableRefNode("y"), new Problem.ConstantNode(1))),
            new Problem.AssignmentNode("z", new Problem.AddNode(
                    new Problem.VariableRefNode("w"), new Problem.VariableRefNode("x")))
        ];
        var assigned = new HashSet<string>();
        var errors = new List<string>();
        var code = new List<string>();
        foreach (Problem.INode statement in before)
        {
            Console.WriteLine("  " + statement.PrettyPrint());
            statement.TypeCheck(assigned, errors);
            statement.GenerateCode(code);
        }
        Console.WriteLine("  errors: " + Show(errors));
        Console.WriteLine("  code:   " + Show(code));

        Console.WriteLine("With visitors");
        List<Solution.INode> after =
        [
            new Solution.AssignmentNode("y", new Solution.ConstantNode(2)),
            new Solution.AssignmentNode("x", new Solution.AddNode(
                    new Solution.VariableRefNode("y"), new Solution.ConstantNode(1))),
            new Solution.AssignmentNode("z", new Solution.AddNode(
                    new Solution.VariableRefNode("w"), new Solution.VariableRefNode("x")))
        ];
        var checker = new Solution.TypeCheckingVisitor();
        var generator = new Solution.CodeGeneratingVisitor();
        foreach (Solution.INode statement in after)
        {
            var printer = new Solution.PrettyPrintingVisitor();
            statement.Accept(printer);
            Console.WriteLine("  " + printer.Text);
            statement.Accept(checker);
            statement.Accept(generator);
        }
        Console.WriteLine("  errors: " + Show(checker.Errors));
        Console.WriteLine("  code:   " + Show(generator.Code));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
