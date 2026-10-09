namespace dev.kaldiroglu.Visitor.Factory;

/// <summary>
/// The <b>Visitor</b>: one method for employees and one for the boss. <see cref="Boss"/> is
/// not an <see cref="Employee"/>, so a visitor can visit classes that have no common parent.
/// </summary>
public interface IVisitor
{
    void Visit(Employee employee);

    void Visit(Boss boss);
}
