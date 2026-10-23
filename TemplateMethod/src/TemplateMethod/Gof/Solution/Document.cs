namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>
/// GoF's <c>Document</c>. Opening is the same for every kind of document; reading is not,
/// so <see cref="DoRead"/> is a primitive operation that each kind writes.
/// <para>
/// GoF's naming convention: primitive operations start with "Do" — <c>DoRead</c>,
/// <c>DoCreateDocument</c> — so that a reader can see which methods a subclass must write.
/// That is GoF implementation issue 3 (naming conventions).
/// </para>
/// </summary>
public abstract class Document
{
    protected Document(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public bool IsOpen { get; private set; }

    public void Open()
    {
        IsOpen = true;
    }

    /// <summary>
    /// A primitive operation: read the file's contents in this document's own way.
    /// <para>
    /// <c>protected internal</c>, not <c>protected</c>: <see cref="Application"/> calls it, and
    /// is not a subclass of <c>Document</c>. <c>protected internal</c> opens it to the whole
    /// assembly as well.
    /// </para>
    /// </summary>
    protected internal abstract void DoRead(List<string> events);
}
