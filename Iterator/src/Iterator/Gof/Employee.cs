namespace dev.kaldiroglu.Iterator.Gof;

/// <summary>GoF's sample code walks a list of employees and prints each one.</summary>
public sealed record Employee(string Name)
{
    public override string ToString() => Name;
}
