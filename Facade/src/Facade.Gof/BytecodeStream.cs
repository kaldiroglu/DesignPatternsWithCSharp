namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): the output of compilation. A
/// <see cref="CodeGenerator"/> writes instructions here.
/// </summary>
public class BytecodeStream
{
    private readonly List<string> _instructions = new();

    public void Put(string instruction) => _instructions.Add(instruction);

    public IReadOnlyList<string> Instructions => _instructions;

    public override string ToString() => string.Join("\n", _instructions);
}
