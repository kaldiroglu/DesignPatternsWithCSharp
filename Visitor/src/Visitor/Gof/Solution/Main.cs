namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>
/// Runs the same three statements through visitors. The nodes only accept a visitor; each
/// compiler job is one visitor class.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- gof-solution</c>.
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
        var checker = new TypeCheckingVisitor();
        var generator = new CodeGeneratingVisitor();
        foreach (INode statement in program)
        {
            var printer = new PrettyPrintingVisitor();
            statement.Accept(printer);
            Console.WriteLine(printer.Text);
            statement.Accept(checker);
            statement.Accept(generator);
        }
        Console.WriteLine("Errors: " + Show(checker.Errors));
        Console.WriteLine("Code:   " + Show(generator.Code));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
