namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>Takes UI changes.</summary>
public sealed class UiDesigner : Developer
{
    public UiDesigner(string name) : base(name)
    {
    }

    protected override bool Suits(Request request) => request.Kind == RequestKind.UI_CHANGE;
}
