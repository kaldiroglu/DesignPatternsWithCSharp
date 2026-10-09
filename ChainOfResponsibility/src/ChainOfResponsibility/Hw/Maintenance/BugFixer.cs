namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>Takes bugs.</summary>
public sealed class BugFixer : Developer
{
    public BugFixer(string name) : base(name)
    {
    }

    protected override bool Suits(Request request) => request.Kind == RequestKind.BUG;
}
