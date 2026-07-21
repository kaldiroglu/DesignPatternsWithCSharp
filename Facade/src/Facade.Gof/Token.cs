namespace dev.kaldiroglu.Facade.Gof;

/// <summary>Kinds of lexical token produced by the <see cref="Scanner"/>.</summary>
public enum TokenKind
{
    Ident, Int, Assign, Plus, Minus, Return, Newline, Eof
}

/// <summary>
/// Subsystem class (GoF p. 185): a lexical token. Part of the compiler subsystem
/// that the <see cref="Compiler"/> facade hides from ordinary clients.
/// </summary>
public record Token(TokenKind Kind, string Text)
{
    public int AsInt() => int.Parse(Text);

    public override string ToString() =>
        Kind + (Text.Length == 0 ? "" : $"('{Text}')");
}
