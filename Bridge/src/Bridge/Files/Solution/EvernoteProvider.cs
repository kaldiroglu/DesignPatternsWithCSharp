namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>A ConcreteImplementor: the Evernote document store.</summary>
public class EvernoteProvider : InMemoryProvider
{
    public EvernoteProvider() : base("Evernote")
    {
    }
}
