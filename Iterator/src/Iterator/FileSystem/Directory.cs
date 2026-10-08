namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>
/// The <b>Aggregate</b>: a directory holds files, shortcuts, aliases and other directories,
/// and gives out a <see cref="DirectoryIterator{Storage}"/> to list them.
/// </summary>
/// <remarks>
/// <para>
/// Inside this namespace <c>Directory</c> means this class, not <c>System.IO.Directory</c>.
/// </para>
/// <para>
/// Only the iterator can reach the elements. <see cref="Elements"/> is <c>internal</c> and
/// returns a read-only view, and <see cref="DirectoryIterator{Storage}"/> is in the same
/// namespace. Java's package-private access becomes <c>internal</c>, which is the whole
/// assembly rather than one namespace; C# has nothing closer.
/// </para>
/// </remarks>
public class Directory : StorageElement
{
    private readonly List<IStorage> _elements = [];

    public Directory(string name) : base(name, null)
    {
        directory = true;
    }

    public Directory(string name, StorageElement parent) : base(name, parent)
    {
        directory = true;
    }

    /// <summary>A shallow copy: the copy shares the same list of elements.</summary>
    public override IStorage Copy() => (IStorage)MemberwiseClone();

    public override void Move(Directory target)
    {
        parent = target;
        target.Add(this);
    }

    public void Add(IStorage element) => _elements.Add(element);

    public void Delete(IStorage element) => _elements.Remove(element);

    public void List()
    {
        Console.WriteLine("\nList of the directory: " + name);
        foreach (IStorage element in _elements)
        {
            var storageElement = (StorageElement)element;
            if (storageElement.IsDirectory)
                Console.Write($"{storageElement,-20} {"dir",10} \n");
            else
                Console.Write($"{storageElement,-20} \n");
        }
    }

    /// <summary>A read-only view of the elements, for the iterator only.</summary>
    internal IReadOnlyList<IStorage> Elements => _elements.AsReadOnly();

    /// <summary>
    /// Creates the iterator. This is Java's <c>iterator()</c>; .NET calls it
    /// <c>GetEnumerator()</c>.
    /// </summary>
    public DirectoryIterator<IStorage> GetEnumerator() => new(this);
}
