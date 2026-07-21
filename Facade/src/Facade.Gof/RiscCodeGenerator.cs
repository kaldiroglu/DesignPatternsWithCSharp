namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): generates code for a <b>RISC</b> (register)
/// machine. Values live in registers; each result is placed in a freshly
/// allocated register. This is the generator GoF's <c>Compiler::Compile</c> uses.
/// </summary>
public class RiscCodeGenerator(BytecodeStream output) : CodeGenerator(output)
{
    private readonly Stack<string> _registers = new();
    private int _nextRegister;

    private string Allocate()
    {
        string register = "R" + _nextRegister++;
        _registers.Push(register);
        return register;
    }

    public override void EmitPushConstant(int value) => Output.Put($"LOADI {Allocate()}, {value}");

    public override void EmitLoad(string variable) => Output.Put($"LOAD {Allocate()}, {variable}");

    public override void EmitStore(string variable) => Output.Put($"STORE {variable}, {_registers.Pop()}");

    public override void EmitAdd() => EmitBinary("ADD");

    public override void EmitSubtract() => EmitBinary("SUB");

    private void EmitBinary(string mnemonic)
    {
        string right = _registers.Pop();
        string left = _registers.Pop();
        Output.Put($"{mnemonic} {Allocate()}, {left}, {right}");
    }

    public override void EmitReturn() => Output.Put($"RET {_registers.Pop()}");
}
