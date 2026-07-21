namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): the parser. Reads tokens from a
/// <see cref="Scanner"/> and drives a <see cref="ProgramNodeBuilder"/> to build
/// the parse tree.
/// </summary>
/// <remarks>
/// Grammar (one statement per line):
/// <code>
///   statement  := IDENT '=' expression | 'return' expression
///   expression := term (('+' | '-') term)*
///   term       := INT | IDENT
/// </code>
/// </remarks>
public class Parser
{
    public void Parse(Scanner scanner, ProgramNodeBuilder builder)
    {
        while (scanner.Peek().Kind != TokenKind.Eof)
        {
            if (scanner.Peek().Kind == TokenKind.Newline)
            {
                scanner.Next();
                continue;
            }
            builder.AddStatement(ParseStatement(scanner, builder));
        }
    }

    private static ProgramNode ParseStatement(Scanner scanner, ProgramNodeBuilder builder)
    {
        if (scanner.Peek().Kind == TokenKind.Return)
        {
            scanner.Next();
            return builder.NewReturn(ParseExpression(scanner, builder));
        }
        Token identifier = Expect(scanner, TokenKind.Ident);
        Expect(scanner, TokenKind.Assign);
        return builder.NewAssignment(identifier.Text, ParseExpression(scanner, builder));
    }

    private static ProgramNode ParseExpression(Scanner scanner, ProgramNodeBuilder builder)
    {
        ProgramNode left = ParseTerm(scanner, builder);
        while (scanner.Peek().Kind is TokenKind.Plus or TokenKind.Minus)
        {
            char op = scanner.Next().Kind == TokenKind.Plus ? '+' : '-';
            ProgramNode right = ParseTerm(scanner, builder);
            left = builder.NewBinaryOperator(op, left, right);
        }
        return left;
    }

    private static ProgramNode ParseTerm(Scanner scanner, ProgramNodeBuilder builder)
    {
        Token token = scanner.Next();
        return token.Kind switch
        {
            TokenKind.Int => builder.NewConstant(token.AsInt()),
            TokenKind.Ident => builder.NewVariable(token.Text),
            _ => throw new InvalidOperationException($"Expected a number or variable but found {token}")
        };
    }

    private static Token Expect(Scanner scanner, TokenKind kind)
    {
        Token token = scanner.Next();
        if (token.Kind != kind)
        {
            throw new InvalidOperationException($"Expected {kind} but found {token}");
        }
        return token;
    }
}
