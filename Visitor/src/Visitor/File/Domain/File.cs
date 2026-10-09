namespace dev.kaldiroglu.Visitor.File.Domain;

public abstract class File
{
    protected string name;

    public File(string name)
    {
        this.name = name;
    }

    public abstract void Open();

    public abstract void Read();

    public abstract void Close();
}
