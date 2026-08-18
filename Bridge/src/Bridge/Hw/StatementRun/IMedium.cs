namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>
/// The Implementor: everything a document is allowed to ask of the thing it is rendered onto.
/// <para>
/// <b>Every name here is deliberate.</b> The obvious first draft of this interface has
/// <c>DrawBox</c>, <c>SetFont</c>, <c>NewPage</c> and <c>Margin</c> on it — and every one of
/// those is a question about <i>paper</i>. Hand that interface to a screen reader and it cannot
/// answer: a voice has no page, no font and no margin.
/// </para>
/// <para>
/// What survives the screen reader is the set of primitives that describe <i>meaning</i> rather
/// than ink. A heading is still a heading when it is spoken; a label-and-value pair is still a
/// label and a value. That is the test for whether something belongs on an Implementor, and
/// accessibility is the honest way to apply it, because the third medium is a real user rather
/// than a hypothetical platform.
/// </para>
/// </summary>
public interface IMedium
{
    void Heading(int level, string text);

    void Field(string label, string value);

    void Row(params string[] cells);

    void Total(string label, string amount);

    /// <summary>Everything rendered so far.</summary>
    string Output();
}
