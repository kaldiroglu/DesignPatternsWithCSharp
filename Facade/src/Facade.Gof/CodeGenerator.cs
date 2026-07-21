namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): abstract base for code generators. GoF lists
/// <c>StackMachineCodeGenerator</c> and <c>RISCCodeGenerator</c> as subclasses;
/// both are provided here. Nodes call these <c>Emit*</c> operations while being
/// traversed, and each generator targets a different machine.
/// </summary>
public abstract class CodeGenerator
{
    protected readonly BytecodeStream Output;

    protected CodeGenerator(BytecodeStream output) => Output = output;

    public abstract void EmitPushConstant(int value);
    public abstract void EmitLoad(string variable);
    public abstract void EmitStore(string variable);
    public abstract void EmitAdd();
    public abstract void EmitSubtract();
    public abstract void EmitReturn();
}
