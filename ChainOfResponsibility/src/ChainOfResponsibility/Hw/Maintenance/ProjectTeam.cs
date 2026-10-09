namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>Takes projects, and improvements too large for one developer.</summary>
public sealed class ProjectTeam : Developer
{
    public ProjectTeam(string name) : base(name)
    {
    }

    protected override bool Suits(Request request) =>
        request.Kind == RequestKind.PROJECT || request.Kind == RequestKind.IMPROVEMENT;
}
