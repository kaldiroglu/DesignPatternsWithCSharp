namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// Shared state and behavior for anything that can sit in a directory.
/// </summary>
/// <remarks>
/// Note the type of the parent: <see cref="Directory"/>, not <see cref="IStorage"/>. Only a
/// directory can hold anything, so that is the type the field should have — declaring it as
/// <c>IStorage</c> would only buy casts back to <c>Directory</c> at every use.
/// </remarks>
public abstract class StorageElement : IStorage
{
    private string _name;
    private Directory? _parent;
    private DateTimeOffset _modified = DateTimeOffset.UnixEpoch;

    protected StorageElement(string name, Directory? parent)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name), "every element needs a name");
        _parent = parent;
    }

    /// <summary>
    /// Adds this element to its parent.
    /// </summary>
    /// <remarks>
    /// Called by the concrete constructors rather than by this one, and deliberately so: a
    /// subclass's own fields are not initialized until after the base constructor returns, so
    /// publishing <c>this</c> from here would hand the parent a half-built object.
    /// </remarks>
    protected void Attach() => _parent?.Add(this);

    public string GetName() => _name;

    public void Rename(string newName) =>
        _name = newName ?? throw new ArgumentNullException(nameof(newName));

    public virtual void Save() => Console.WriteLine($"Saving {_name}");

    /// <summary>Null-safe: a root has no parent to be detached from, and that is not an error.</summary>
    public void Delete()
    {
        if (_parent is not null)
        {
            _parent.Remove(this);
            _parent = null;
        }
    }

    /// <summary>
    /// Moves properly: leave the old parent, then join the new one.
    /// </summary>
    /// <remarks>
    /// Both halves happen here, in one place. Doing only one of them is how an element ends up
    /// believing it lives where it does not, or appearing in two directories at once.
    /// </remarks>
    public void Move(Directory target)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (ReferenceEquals(target, this))
        {
            throw new ArgumentException("an element cannot be moved into itself", nameof(target));
        }

        _parent?.Remove(this);
        _parent = target;
        target.Add(this);
    }

    /// <summary>When this element itself last changed. A directory takes the newest of its subtree.</summary>
    public virtual DateTimeOffset LastModified() => _modified;

    /// <summary>Records a modification time. Bubbles nothing: a directory asks its children instead.</summary>
    public void Touch(DateTimeOffset when) => _modified = when;

    /// <summary>A leaf is one element. <see cref="Directory"/> adds its children.</summary>
    public virtual int Count() => 1;

    /// <summary>A leaf is its own largest. <see cref="Directory"/> asks its children.</summary>
    public virtual IStorage? Largest() => this;

    public virtual IStorage? Find(string name) => _name == name ? this : null;

    public virtual IReadOnlyList<IStorage> FindAll(Func<IStorage, bool> test) =>
        test(this) ? [this] : [];

    public Directory? GetParent() => _parent;

    /// <summary>Internal: used by <see cref="Directory"/> when it adopts an element.</summary>
    internal void SetParent(Directory? parent) => _parent = parent;

    /// <summary>The path from the root down to this element.</summary>
    public string Path() => _parent is null ? _name : _parent.Path() + "/" + _name;

    public abstract long Size();

    public abstract IStorage Copy();

    public abstract string Render(string indent);

    public override string ToString() => _name;
}
