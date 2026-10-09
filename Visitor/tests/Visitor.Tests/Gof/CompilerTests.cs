using System.Reflection;
using Xunit;
using G = global::dev.kaldiroglu.Visitor.Gof;
using P = global::dev.kaldiroglu.Visitor.Gof.Problem;
using S = global::dev.kaldiroglu.Visitor.Gof.Solution;

namespace dev.kaldiroglu.Visitor.Tests.Gof;

/// <summary>
/// GoF's compiler (Design Patterns, pp. 331-344), before and after the pattern, on the
/// program y = 2, x = y + 1, z = w + x.
/// </summary>
public class CompilerTests
{
    private static readonly string[] Code =
        ["PUSH 2", "STORE y", "LOAD y", "PUSH 1", "ADD", "STORE x", "LOAD w", "LOAD x", "ADD", "STORE z"];

    private static List<S.INode> Program() =>
    [
        new S.AssignmentNode("y", new S.ConstantNode(2)),
        new S.AssignmentNode("x", new S.AddNode(new S.VariableRefNode("y"), new S.ConstantNode(1))),
        new S.AssignmentNode("z", new S.AddNode(new S.VariableRefNode("w"), new S.VariableRefNode("x")))
    ];

    private static List<P.INode> ProgramBefore() =>
    [
        new P.AssignmentNode("y", new P.ConstantNode(2)),
        new P.AssignmentNode("x", new P.AddNode(new P.VariableRefNode("y"), new P.ConstantNode(1))),
        new P.AssignmentNode("z", new P.AddNode(new P.VariableRefNode("w"), new P.VariableRefNode("x")))
    ];

    /// <summary>Methods written in the type itself, without property accessors.</summary>
    private static IEnumerable<string> DeclaredMethodNames(Type type) =>
        type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                        BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => m.Name);

    [Fact]
    public void TypeChecking()
    {
        var checker = new S.TypeCheckingVisitor();
        Program().ForEach(statement => statement.Accept(checker));

        Assert.Equal(["w is used before it is assigned"], checker.Errors);
    }

    [Fact]
    public void CodeGeneration()
    {
        var generator = new S.CodeGeneratingVisitor();
        Program().ForEach(statement => statement.Accept(generator));

        Assert.Equal(Code, generator.Code);
        Assert.Equal(["LOAD w", "LOAD x", "ADD", "STORE z"], Code[6..10]);
    }

    [Fact]
    public void PrettyPrinting()
    {
        var lines = new List<string>();
        foreach (var statement in Program())
        {
            var printer = new S.PrettyPrintingVisitor();
            statement.Accept(printer);
            lines.Add(printer.Text);
        }

        Assert.Equal(["y = 2", "x = y + 1", "z = w + x"], lines);
    }

    [Fact]
    public void BeforeThePatternTheSameResults()
    {
        var assigned = new HashSet<string>();
        var errors = new List<string>();
        var code = new List<string>();
        var lines = new List<string>();
        foreach (var statement in ProgramBefore())
        {
            lines.Add(statement.PrettyPrint());
            statement.TypeCheck(assigned, errors);
            statement.GenerateCode(code);
        }

        Assert.Equal(["y = 2", "x = y + 1", "z = w + x"], lines);
        Assert.Equal(["w is used before it is assigned"], errors);
        Assert.Equal(Code, code);
    }

    [Fact]
    public void FourNodesThreeJobs()
    {
        Type[] nodes = [typeof(P.AssignmentNode), typeof(P.VariableRefNode), typeof(P.ConstantNode), typeof(P.AddNode)];
        string[] jobNames = ["TypeCheck", "GenerateCode", "PrettyPrint"];

        Assert.Equal(3, typeof(P.INode).GetMethods().Length);
        foreach (var node in nodes)
        {
            var jobs = DeclaredMethodNames(node).Where(jobNames.Contains).ToList();
            Assert.True(jobs.Count == 3, node.Name);
        }
    }

    [Fact]
    public void NodesOnlyAccept()
    {
        Assert.Equal(["Accept"], typeof(S.INode).GetMethods().Select(m => m.Name));
        Assert.Equal(["VisitAdd", "VisitAssignment", "VisitConstant", "VisitVariableRef"],
            typeof(S.INodeVisitor).GetMethods().Select(m => m.Name).Order(StringComparer.Ordinal));
    }

    [Fact]
    public void MainOutput()
    {
        var lines = Printed.By(G.Main.Run);

        string[] body =
        [
            "  y = 2", "  x = y + 1", "  z = w + x",
            "  errors: [w is used before it is assigned]",
            "  code:   [PUSH 2, STORE y, LOAD y, PUSH 1, ADD, STORE x, LOAD w, LOAD x, ADD, STORE z]"
        ];
        Assert.Equal(["Before the pattern", .. body, "With visitors", .. body], lines);
    }
}
