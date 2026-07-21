namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// <b>Facade</b> participant (GoF p. 185).
/// </summary>
/// <remarks>
/// The compiler subsystem is made of many classes — <see cref="Scanner"/>,
/// <see cref="Parser"/>, <see cref="ProgramNodeBuilder"/>, the
/// <see cref="ProgramNode"/> tree, <see cref="CodeGenerator"/>s and
/// <see cref="BytecodeStream"/>. Most clients just want to turn source into
/// bytecode. <c>Compiler</c> offers that single entry point and orchestrates the
/// subsystem. It does not hide the subsystem: a client needing finer control can
/// still use those classes directly.
/// </remarks>
public class Compiler
{
    /// <summary>Compiles source into bytecode using the default (stack machine) target.</summary>
    public BytecodeStream Compile(string sourceCode)
    {
        var output = new BytecodeStream();
        Compile(sourceCode, new StackMachineCodeGenerator(output));
        return output;
    }

    /// <summary>Compiles source, emitting into the given generator's stream.</summary>
    public void Compile(string sourceCode, CodeGenerator generator)
    {
        var scanner = new Scanner(sourceCode);
        var builder = new ProgramNodeBuilder();

        new Parser().Parse(scanner, builder);

        builder.GetRootNode().Traverse(generator);
    }
}
