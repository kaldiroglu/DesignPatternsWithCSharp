namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>The document that <see cref="Application"/> creates and keeps.</summary>
public abstract class Document
{
    protected Document(string name)
    {
        Name = name;
    }

    /// <summary>A protected field with a public getter in Java.</summary>
    public string Name { get; protected set; }

    internal abstract void Open();

    internal abstract void Read();

    internal abstract void Save();

    internal abstract void Close();
}
