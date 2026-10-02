namespace dev.kaldiroglu.Strategy.Gof.Solution;

/// <summary>
/// The <b>Strategy</b>, in GoF's own words and their own name for it.
/// <para>
/// Design Patterns, p. 315: "We can avoid these problems by defining classes that encapsulate
/// different line breaking algorithms. An algorithm that's encapsulated in this way is called
/// a <i>strategy</i>."
/// </para>
/// <para>
/// One method. It is handed the components and the width and answers where the lines break —
/// it does not own the text, does not know what a document is, and cannot decide whether it
/// should be the algorithm in use.
/// </para>
/// </summary>
public interface ICompositor
{
    /// <summary>What this algorithm calls itself, for a test or a status bar to read back.</summary>
    string Name { get; }

    /// <summary>Break the components into lines no wider than <paramref name="lineWidth"/>.</summary>
    Layout Compose(IReadOnlyList<Component> components, int lineWidth);
}
