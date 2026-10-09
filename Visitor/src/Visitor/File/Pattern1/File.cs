namespace dev.kaldiroglu.Visitor.File.Pattern1;

public abstract class File
{
    protected string name;

    public File(string name)
    {
        this.name = name;
    }

    public string Name => name;

    public abstract bool Accept(IVisitor visitor);

    public abstract void Open();

    public abstract void Read();

    public abstract void Close();
}
