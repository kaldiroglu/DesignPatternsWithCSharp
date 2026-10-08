namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>A <b>ConcreteClass</b> for documents: a drawing reads shapes.</summary>
public sealed class DrawDocument(string name) : Document(name)
{
    protected internal override void DoRead(List<string> events)
    {
        events.Add("read shapes from " + Name);
    }
}
