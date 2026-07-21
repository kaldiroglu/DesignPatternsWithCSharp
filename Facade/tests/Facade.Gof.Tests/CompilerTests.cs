using dev.kaldiroglu.Facade.Gof;

using Xunit;

namespace dev.kaldiroglu.Facade.Gof.Tests;

/// <summary>Verifies the Compiler facade over the compiler subsystem (GoF p. 185).</summary>
public class CompilerTests
{
    private readonly Compiler _compiler = new();

    [Fact]
    public void CompilesAssignment()
    {
        BytecodeStream output = _compiler.Compile("x = 3\n");
        Assert.Equal(new[] { "PUSH 3", "STORE x" }, output.Instructions);
    }

    [Fact]
    public void CompilesBinaryExpression()
    {
        BytecodeStream output = _compiler.Compile("z = x + y\n");
        Assert.Equal(new[] { "LOAD x", "LOAD y", "ADD", "STORE z" }, output.Instructions);
    }

    [Fact]
    public void CompilesWholeProgram()
    {
        const string source =
            """
            x = 3
            y = 4
            z = x + y - 1
            return z
            """;

        BytecodeStream output = _compiler.Compile(source);

        Assert.Equal(new[]
        {
            "PUSH 3", "STORE x",
            "PUSH 4", "STORE y",
            "LOAD x", "LOAD y", "ADD", "PUSH 1", "SUB", "STORE z",
            "LOAD z", "RETURN"
        }, output.Instructions);
    }

    [Fact]
    public void CompilesToRiscTarget()
    {
        var output = new BytecodeStream();
        _compiler.Compile("z = x + y\n", new RiscCodeGenerator(output));
        Assert.Equal(new[]
        {
            "LOAD R0, x",
            "LOAD R1, y",
            "ADD R2, R0, R1",
            "STORE z, R2"
        }, output.Instructions);
    }
}
