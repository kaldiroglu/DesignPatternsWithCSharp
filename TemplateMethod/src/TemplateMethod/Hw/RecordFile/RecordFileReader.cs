namespace dev.kaldiroglu.TemplateMethod.Hw.RecordFile;

/// <summary>
/// Homework 3: read a file of records, one record per line.
/// <para>
/// Opening, reading line by line and closing are the same for every kind of record; only
/// <see cref="Parse"/> differs. The template method also gives a guarantee: the reader is
/// closed even when <c>Parse</c> fails on a bad line. A subclass cannot forget to close it,
/// because it never sees the reader.
/// </para>
/// </summary>
public abstract class RecordFileReader<T>
{
    /// <summary>
    /// The template method. Not <c>virtual</c>, so no subclass can override it (Java's
    /// <c>final</c>). The <c>using</c> statement closes the reader, as Java's
    /// try-with-resources does.
    /// </summary>
    public IReadOnlyList<T> ReadAll(TextReader source)
    {
        var records = new List<T>();
        using (source)
        {
            string? line;
            while ((line = source.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    records.Add(Parse(line));
                }
            }
        }
        return records.AsReadOnly();
    }

    /// <summary>A primitive operation: turn one line into one record.</summary>
    protected abstract T Parse(string line);
}
