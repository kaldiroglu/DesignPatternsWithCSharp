namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>The document that <see cref="Application"/> creates and keeps.</summary>
public abstract class Document
{
    protected Document(string name)
    {
        Name = name;
    }

    /// <summary>The document's name. A subclass sets it; any caller can read it.</summary>
    public string Name { get; protected set; }

    internal abstract void Open();

    internal abstract void Read();

    internal abstract void Save();

    internal abstract void Close();
}
