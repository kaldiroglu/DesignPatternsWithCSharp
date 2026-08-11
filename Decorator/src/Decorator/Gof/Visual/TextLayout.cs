namespace dev.kaldiroglu.Decorator.Gof.Visual;

/// <summary>
/// Wraps a paragraph into fixed-width lines. Shared by both designs so that the only
/// difference between Problem and Solution is how embellishments are attached, not how
/// text is laid out.
/// </summary>
public static class TextLayout
{
    /// <summary>Greedily wraps text to width columns, padded and clipped to height rows.</summary>
    public static IReadOnlyList<string> Wrap(string text, int width, int height)
    {
        var lines = new List<string>();
        var line = new System.Text.StringBuilder();

        foreach (var word in text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.Length == 0)
            {
                line.Append(word);
            }
            else if (line.Length + 1 + word.Length <= width)
            {
                line.Append(' ').Append(word);
            }
            else
            {
                lines.Add(line.ToString());
                line = new System.Text.StringBuilder(word);
            }
        }

        if (line.Length > 0)
        {
            lines.Add(line.ToString());
        }

        var padded = new List<string>();
        for (var i = 0; i < height; i++)
        {
            var content = i < lines.Count ? lines[i] : "";
            if (content.Length > width)
            {
                content = content[..width];
            }

            padded.Add(content + new string(' ', width - content.Length));
        }

        return padded;
    }
}
