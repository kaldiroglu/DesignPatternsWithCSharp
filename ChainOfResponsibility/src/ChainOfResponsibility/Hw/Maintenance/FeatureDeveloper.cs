namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>Takes improvements of up to ten days. A larger one goes on to the project team.</summary>
public sealed class FeatureDeveloper : Developer
{
    public FeatureDeveloper(string name) : base(name)
    {
    }

    protected override bool Suits(Request request) =>
        request.Kind == RequestKind.IMPROVEMENT && request.Days <= 10;
}
