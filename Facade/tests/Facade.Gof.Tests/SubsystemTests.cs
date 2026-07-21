using dev.kaldiroglu.Facade.Gof;

using Xunit;

namespace dev.kaldiroglu.Facade.Gof.Tests;

/// <summary>The subsystem classes remain usable directly — the facade does not hide them.</summary>
public class SubsystemTests
{
    [Fact]
    public void ScannerProducesTokens()
    {
        var scanner = new Scanner("x = 12 + y");
        Assert.Equal(TokenKind.Ident, scanner.Next().Kind);
        Assert.Equal(TokenKind.Assign, scanner.Next().Kind);
        Token number = scanner.Next();
        Assert.Equal(TokenKind.Int, number.Kind);
        Assert.Equal(12, number.AsInt());
        Assert.Equal(TokenKind.Plus, scanner.Next().Kind);
        Assert.Equal(TokenKind.Ident, scanner.Next().Kind);
        Assert.Equal(TokenKind.Eof, scanner.Next().Kind);
    }

    [Fact]
    public void SubsystemUsableWithoutFacade()
    {
        var scanner = new Scanner("return 1 + 2\n");
        var builder = new ProgramNodeBuilder();
        new Parser().Parse(scanner, builder);

        var output = new BytecodeStream();
        builder.GetRootNode().Traverse(new StackMachineCodeGenerator(output));

        Assert.Equal(new[] { "PUSH 1", "PUSH 2", "ADD", "RETURN" }, output.Instructions);
    }
}
