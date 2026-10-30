namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>The shared state and behavior of every element: a name, a parent and a flag.</summary>
public abstract class StorageElement : IStorage
{
    protected string name;
    protected Directory? parent;
    protected bool directory;

    protected StorageElement(string name, Directory? parent)
    {
        this.name = name;
        this.parent = parent;
        parent?.Add(this);
    }

    public void Rename(string newName) => Name = newName;

    public void Save() => Console.WriteLine("Saving the element.");

    public void Delete()
    {
        parent?.Delete(this);
        parent = null;
    }

    /// <summary>A copy of this element, in no directory until it is added to one.</summary>
    public virtual IStorage Copy()
    {
        var copiedElement = (StorageElement)MemberwiseClone();
        copiedElement.parent = null;
        return copiedElement;
    }

    public void Move(Directory target)
    {
        Delete();
        parent = target;
        target.Add(this);
    }

    public bool IsDirectory => directory;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public Directory? Parent
    {
        get => parent;
        set => parent = value;
    }

    public override string ToString() => name;
}
