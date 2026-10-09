namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>The context the rules are interpreted against: one product in a shop.</summary>
public sealed record Product(string Name, string Category, int Price);
