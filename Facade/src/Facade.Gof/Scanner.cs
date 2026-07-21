namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): the lexical analyzer. Turns raw source text into
/// a stream of <see cref="Token"/>s consumed by the <see cref="Parser"/>.
/// </summary>
public class Scanner
{
    private readonly List<Token> _tokens = new();
    private int _position;

    public Scanner(string source) => Tokenize(source);

    /// <summary>Returns the next token and advances.</summary>
    public Token Next()
    {
        Token token = Peek();
        if (_position < _tokens.Count - 1)
        {
            _position++;
        }
        return token;
    }

    /// <summary>Returns the next token without consuming it.</summary>
    public Token Peek() => _tokens[_position];

    private void Tokenize(string source)
    {
        int i = 0;
        while (i < source.Length)
        {
            char c = source[i];
            if (c == '\n')
            {
                _tokens.Add(new Token(TokenKind.Newline, ""));
                i++;
            }
            else if (c is ' ' or '\t' or '\r')
            {
                i++;
            }
            else if (char.IsDigit(c))
            {
                int start = i;
                while (i < source.Length && char.IsDigit(source[i])) i++;
                _tokens.Add(new Token(TokenKind.Int, source[start..i]));
            }
            else if (char.IsLetter(c))
            {
                int start = i;
                while (i < source.Length && char.IsLetterOrDigit(source[i])) i++;
                string word = source[start..i];
                _tokens.Add(word == "return"
                    ? new Token(TokenKind.Return, "return")
                    : new Token(TokenKind.Ident, word));
            }
            else if (c == '=') { _tokens.Add(new Token(TokenKind.Assign, "=")); i++; }
            else if (c == '+') { _tokens.Add(new Token(TokenKind.Plus, "+")); i++; }
            else if (c == '-') { _tokens.Add(new Token(TokenKind.Minus, "-")); i++; }
            else throw new ArgumentException($"Unexpected character: '{c}'");
        }
        _tokens.Add(new Token(TokenKind.Eof, ""));
    }
}
