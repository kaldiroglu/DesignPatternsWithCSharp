namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>A ConcreteImplementor: the SharePoint document store.</summary>
public class SharePointProvider : InMemoryProvider
{
    public SharePointProvider() : base("SharePoint")
    {
    }
}
