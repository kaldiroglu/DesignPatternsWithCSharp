namespace dev.kaldiroglu.Strategy.Gof;

/// <summary>
/// The result of laying components out: which components landed on which line.
/// </summary>
/// <param name="Lines">the components, grouped by the line they were placed on</param>
/// <param name="LineWidth">the column width they were fitted into</param>
public sealed record Layout(IReadOnlyList<IReadOnlyList<Component>> Lines, int LineWidth)
{
    public IReadOnlyList<IReadOnlyList<Component>> Lines { get; } = [.. Lines];

    public int LineCount => Lines.Count;

    /// <summary>How wide the text on one line came out.</summary>
    public int WidthOf(int line) =>
        Lines[line].Sum(component => component.Width)
        + Math.Max(0, Lines[line].Count - 1);   // one space between components

    /// <summary>The room left over on one line — what a good break minimizes.</summary>
    public int SlackOn(int line) => LineWidth - WidthOf(line);

    /// <summary>The worst gap left on any line but the last, which is where bad breaks show.</summary>
    public int WorstSlack
    {
        get
        {
            var worst = 0;
            for (var i = 0; i < Lines.Count - 1; i++)
            {
                worst = Math.Max(worst, SlackOn(i));
            }
            return worst;
        }
    }

    /// <summary>The text as it would be read, one line per entry.</summary>
    public IReadOnlyList<string> Render() =>
        [.. Lines.Select(line => string.Join(" ", line.Select(component => component.Text)))];
}
