namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>A ConcreteImplementor: the FileNet document store.</summary>
public class FileNetProvider : InMemoryProvider
{
    public FileNetProvider() : base("FileNet")
    {
    }
}
