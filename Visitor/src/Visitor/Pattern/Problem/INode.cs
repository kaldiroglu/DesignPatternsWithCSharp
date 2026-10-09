namespace dev.kaldiroglu.Visitor.Pattern.Problem;

/// <summary>
/// An earlier outline of GoF's compiler nodes, before the pattern: every node carries every
/// operation. The methods are empty, as in the Java, so there is nothing to run.
/// </summary>
public interface INode
{
    void TypeCheck();

    void GenerateCode();

    void PrettyPrint();
}
