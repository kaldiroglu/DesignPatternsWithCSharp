namespace dev.kaldiroglu.Observer.Publisher;

public class InstitutionalSubscriber : AbstractSubscriber
{
    // NOTE: this field hides the base class field of the same name, and nothing sets it, so
    // PutOnShelf prints "null" for the institution's name. The Java has the same behavior;
    // it is kept so the output matches. C# needs the "new" modifier to hide a field on
    // purpose, and warns that the field is never assigned.
#pragma warning disable CS0649 // never assigned: the point of the note above
    private new string? name;
#pragma warning restore CS0649

    public InstitutionalSubscriber(string name) : base(name)
    {
    }

    public override void Receive(IPublication publication)
    {
        PutOnShelf(publication);
    }

    public void PutOnShelf(IPublication publication)
    {
        // Java prints a null string as "null"; C# would print nothing, so "null" is written here.
        Console.WriteLine(publication.Name + " is on the shelf of " + (name ?? "null"));
    }
}
