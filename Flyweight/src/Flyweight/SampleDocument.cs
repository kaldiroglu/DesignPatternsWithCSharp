namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// The small two-line document both demo entries use (GoF, p. 204). It is built out of shared
/// <see cref="CharacterGlyph"/> flyweights, and it counts the characters typed so that a run
/// can compare that count with the objects allocated.
/// </summary>
internal sealed class SampleDocument
{
    public string[] Lines { get; } =
        { "flyweight is a nice solution", "lightweight is also a nice solution" };

    public GlyphFactory Factory { get; } = new();

    public Column Document { get; }

    public int Occurrences { get; }

    public SampleDocument()
    {
        // Build the document tree: a Column of Rows of (shared) Characters.
        Document = Factory.CreateColumn();
        foreach (string line in Lines)
        {
            Row row = Factory.CreateRow();
            foreach (char c in line)
            {
                row.Insert(Factory.CreateCharacter(c)); // <- sharing happens here
                Occurrences++;
            }
            Document.Insert(row);
        }
    }
}
