namespace dev.kaldiroglu.Visitor.Gof.Problem;

/// <summary>
/// GoF's motivation, before the pattern: a compiler's syntax tree where every node class
/// carries every operation the compiler runs on it.
/// <para>
/// Type checking, code generation and pretty-printing are three unrelated jobs, and each one
/// is spread over four classes. A fourth job — say, a metrics count — is an edit to every
/// node class.
/// </para>
/// </summary>
public interface INode
{
    void TypeCheck(ISet<string> assigned, IList<string> errors);

    void GenerateCode(IList<string> code);

    string PrettyPrint();
}
