namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>A <b>ConcreteClass</b>: writes the two steps it must, and leaves the hook alone.</summary>
public sealed class DrawApplication : Application
{
    protected override bool CanOpenDocument(string name)
    {
        return name.EndsWith(".draw", StringComparison.Ordinal);
    }

    protected override Document DoCreateDocument(string name)
    {
        return new DrawDocument(name);
    }
}
