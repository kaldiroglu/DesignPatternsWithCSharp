namespace dev.kaldiroglu.Strategy.Gof;

/// <summary>
/// One thing on a line of text — a word, an image, a rule.
/// <para>
/// GoF's Strategy chapter (p. 315) is set in a document editor called Lexi, and the thing
/// being laid out is a stream of components. All a line-breaking algorithm needs to know
/// about one is how wide it is and whether the line may be broken after it.
/// </para>
/// </summary>
/// <param name="Text">what it says, for reading the result back</param>
/// <param name="Width">how much room it takes</param>
/// <param name="Breakable">whether a line may end here</param>
public sealed record Component(string Text, int Width, bool Breakable)
{
    /// <summary>A word, which a line may be broken after.</summary>
    public static Component Word(string text) => new(text, text.Length, true);

    /// <summary>Something that must not be split from what follows — a figure with its caption.</summary>
    public static Component Glued(string text) => new(text, text.Length, false);
}
