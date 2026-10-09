namespace dev.kaldiroglu.Visitor.File.Pattern1;

public interface IVisitor
{
    bool Visit(TextFile file);

    bool Visit(XMLFile file);
}
