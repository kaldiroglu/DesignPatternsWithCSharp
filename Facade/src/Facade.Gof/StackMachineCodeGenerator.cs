namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): generates code for a simple <b>stack machine</b>.
/// </summary>
public class StackMachineCodeGenerator(BytecodeStream output) : CodeGenerator(output)
{
    public override void EmitPushConstant(int value) => Output.Put($"PUSH {value}");
    public override void EmitLoad(string variable) => Output.Put($"LOAD {variable}");
    public override void EmitStore(string variable) => Output.Put($"STORE {variable}");
    public override void EmitAdd() => Output.Put("ADD");
    public override void EmitSubtract() => Output.Put("SUB");
    public override void EmitReturn() => Output.Put("RETURN");
}
