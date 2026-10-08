namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>The shared state and behavior of every element: a name, a parent and a flag.</summary>
public abstract class StorageElement : IStorage
{
    protected string name;
    protected IStorage? parent;
    protected bool directory;

    protected StorageElement(string name, IStorage? parent)
    {
        this.name = name;
        this.parent = parent;
        if (parent != null)
            ((Directory)parent).Add(this);
    }

    public void Rename(string newName) => Name = newName;

    public void Save() => Console.WriteLine("Saving the element.");

    public void Delete() => ((Directory)parent!).Delete(this);

    /// <summary>A shallow copy of this element.</summary>
    public virtual IStorage Copy() => (IStorage)MemberwiseClone();

    public virtual void Move(Directory target)
    {
        ((Directory)parent!).Delete(this);
        target.Add(this);
    }

    public bool IsDirectory => directory;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public IStorage? Parent
    {
        get => parent;
        set => parent = value;
    }

    public override string ToString() => name;
}
